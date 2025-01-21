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
        this.locationsFilePath = Path.Combine(AppContext.BaseDirectory, "data", "locations.json");
        this.itemsFilePath = Path.Combine(AppContext.BaseDirectory, "data", "items.json");

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

            // Console.WriteLine($"Middleware: API_KEY {apiKey} - Full access: {hasFullAccess}");

            // Step 2: Extract Resource and Method
            var pathSegments = context.Request.Path.Value?.Trim('/').Split('/');

            // Ensure the resource is correctly extracted as the third segment (after "api" and "v2")
            string resource = pathSegments != null && pathSegments.Length > 2 ? pathSegments[2] : null!;
            var method = context.Request.Method.ToUpper();

            // Step 3: Validate Resource
            if (string.IsNullOrEmpty(resource) || !permissions.ContainsKey(resource))
            {
                // Console.WriteLine($"Middleware: Resource '{resource}' not found in permissions for API_KEY {apiKey}.");
                context.Response.StatusCode = 403; // Forbidden
                await context.Response.WriteAsync($"Access denied: resource '{resource}' not allowed.");
                return;
            }

            var resourcePermissions = permissions[resource];
            var allowedMethods = resourcePermissions.methods?.ToObject<List<string>>() ?? new List<string>();

            // Step 4: Validate HTTP Method
            if (!allowedMethods.Contains(method))
            {
                // Console.WriteLine($"Middleware: Method '{method}' not allowed for resource '{resource}' and API_KEY {apiKey}.");
                context.Response.StatusCode = 403; // Forbidden
                await context.Response.WriteAsync($"Access denied: method '{method}' not allowed for resource '{resource}'.");
                return;
            }

            // Step 5: Additional Filtering for "own" Access
            if (resourcePermissions.access.ToString() == "own" && !hasFullAccess)
            {
                if (resource == "locations")
                {
                    // Console.WriteLine($"Middleware: Filtering locations for warehouse IDs: {string.Join(", ", warehouseIds)}");
                    var locations = JsonSerializer.Deserialize<List<Location>>(await File.ReadAllTextAsync(this.locationsFilePath));

                    // Filter locations based on warehouse_ids
                    var filteredLocations = locations!
                        .Where(location => warehouseIds!.Contains(location.WarehouseId))
                        .ToList();

                    context.Items["FilteredLocations"] = filteredLocations;

                    // Console.WriteLine($"Middleware: Filtered {filteredLocations.Count} locations for API_KEY {apiKey}.");
                }

                if (resource == "items")
                {
                    // Console.WriteLine($"Middleware: Filtering items for warehouse IDs: {string.Join(", ", warehouseIds)}");
                    var items = JsonSerializer.Deserialize<List<Item>>(await File.ReadAllTextAsync(this.itemsFilePath));

                    // Filter items based on warehouse_ids
                    var filteredItems = items!
                        .Where(item => warehouseIds!.Contains(item.SupplierId)) // Example: filtering by SupplierId
                        .ToList();

                    context.Items["FilteredItems"] = filteredItems;

                    // Console.WriteLine($"Middleware: Filtered {filteredItems.Count} items for API_KEY {apiKey}.");
                }
            }
            else if (hasFullAccess)
            {
                // Console.WriteLine($"Middleware: Full access granted for API_KEY {apiKey}.");

                // Add all locations or items directly without filtering
                if (resource == "locations")
                {
                    var locations = JsonSerializer.Deserialize<List<Location>>(await File.ReadAllTextAsync(this.locationsFilePath));
                    context.Items["FilteredLocations"] = locations;

                    // Console.WriteLine($"Middleware: Loaded all locations for API_KEY {apiKey}.");
                }

                if (resource == "items")
                {
                    var items = JsonSerializer.Deserialize<List<Item>>(await File.ReadAllTextAsync(this.itemsFilePath));
                    context.Items["FilteredItems"] = items;

                    // Console.WriteLine($"Middleware: Loaded all items for API_KEY {apiKey}.");
                }
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