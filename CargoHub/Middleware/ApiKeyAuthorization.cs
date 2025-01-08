using System.Text.Json;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

public class ApiKeyAuthorizationMiddleware
{
    private readonly RequestDelegate next;
    private readonly Dictionary<string, dynamic> apiKeys;
    private readonly string locationsFilePath;
    private readonly string itemsFilePath;

    public ApiKeyAuthorizationMiddleware(RequestDelegate next)
    {
        this.next = next;

        // Load API keys
        var apiKeysPath = Path.Combine(AppContext.BaseDirectory, "apiV2", "ApiKeys", "api_keys.json");
        if (!File.Exists(apiKeysPath))
        {
            throw new FileNotFoundException($"API keys file not found: {apiKeysPath}");
        }

        this.apiKeys = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(File.ReadAllText(apiKeysPath))!;

        // Define paths for locations.json and items.json
        this.locationsFilePath = Path.Combine(AppContext.BaseDirectory, "test_data", "locations.json");
        this.itemsFilePath = Path.Combine(AppContext.BaseDirectory, "test_data", "items.json");

        if (!File.Exists(this.locationsFilePath) || !File.Exists(this.itemsFilePath))
        {
            throw new FileNotFoundException("Locations or Items data file is missing.");
        }
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // Step 1: Validate API_KEY
            var apiKey = context.Request.Headers["API_KEY"].ToString();
            if (string.IsNullOrEmpty(apiKey) || !this.apiKeys.ContainsKey(apiKey))
            {
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync("Invalid or missing API key.");
                return;
            }

            var keyData = this.apiKeys[apiKey];
            var permissions = keyData.permissions;
            var warehouseIds = keyData.warehouse_ids?.ToObject<List<int>>();
            bool hasFullAccess = warehouseIds == null || warehouseIds!.Count == 0;

            Console.WriteLine($"Middleware: API_KEY {apiKey} - Full access: {hasFullAccess}");

            // Step 2: Filter Locations
            if (permissions.locations.access.ToString() == "own" && !hasFullAccess)
            {
                Console.WriteLine($"Middleware: Filtering locations for warehouse IDs: {string.Join(", ", warehouseIds)}");
                var locations = JsonSerializer.Deserialize<List<Location>>(await File.ReadAllTextAsync(this.locationsFilePath));
                var filteredLocations = locations!
                    .Where(location => warehouseIds!.Contains(location.WarehouseId))
                    .ToList();

                context.Items["FilteredLocations"] = filteredLocations;
                Console.WriteLine($"Middleware: Filtered {filteredLocations.Count} locations for API_KEY {apiKey}.");
            }
            else if (hasFullAccess)
            {
                Console.WriteLine($"Middleware: Full access granted to locations for API_KEY {apiKey}.");
                var locations = JsonSerializer.Deserialize<List<Location>>(await File.ReadAllTextAsync(this.locationsFilePath));
                context.Items["FilteredLocations"] = locations;
            }

            // Step 3: Filter Items
            if (permissions.items.access.ToString() == "own" && !hasFullAccess)
            {
                Console.WriteLine($"Middleware: Filtering items for warehouse IDs: {string.Join(", ", warehouseIds)}");
                var items = JsonSerializer.Deserialize<List<Item>>(await File.ReadAllTextAsync(this.itemsFilePath));
                var filteredItems = items!
                    .Where(item => warehouseIds!.Contains(item.SupplierId)) // Example: filtering by SupplierId
                    .ToList();

                context.Items["FilteredItems"] = filteredItems;
            }
            else if (hasFullAccess)
            {
                Console.WriteLine($"Middleware: Full access granted to items for API_KEY {apiKey}.");
                var items = JsonSerializer.Deserialize<List<Item>>(await File.ReadAllTextAsync(this.itemsFilePath));
                context.Items["FilteredItems"] = items;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Middleware: Error - {ex.Message}");
            context.Response.StatusCode = 500; // Internal Server Error
            await context.Response.WriteAsync("Server error: middleware failed to process the request.");
            return;
        }

        // Proceed to the next middleware
        await this.next(context);
    }
}

// Extension method for registering the middleware
public static class ApiKeyAuthorizationExtensions
{
    public static IApplicationBuilder UseApiKeyAuthorization(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ApiKeyAuthorizationMiddleware>();
    }
}