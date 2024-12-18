using System.Text.Json;
using System.Runtime.CompilerServices;

public static class ApiKeys
{
    private static Dictionary<string, Dictionary<string, object>> apiKeyEndpoints = new Dictionary<string, Dictionary<string, object>>();

    public static void Initialize(string path)
    {
        apiKeyEndpoints = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, object>>>(File.ReadAllText(path)) ??
            throw new Exception($"Could not read api keys: {path}");
    }

    public static bool HasAccess(string apiKey, string endpoint, string method)
    {
        if (!apiKeyEndpoints.TryGetValue(apiKey, out var accessConfig) || accessConfig == null)
        {
            return false;
        }

        // Check for global access via `<resource>_access`
        string resourceAccessKey = $"{endpoint}_access";
        if (accessConfig.TryGetValue(resourceAccessKey, out var accessObj) && accessObj is JsonElement accessJson)
        {
            var accessList = JsonSerializer.Deserialize<List<string>>(accessJson.GetRawText());
            if (accessList != null && (accessList.Contains("*") || accessList.Contains(method.ToLower())))
            {
                return true; // Global access granted
            }
        }

        // Check for specific method access in `<resource>`
        if (accessConfig.TryGetValue(endpoint, out var methodAccessObj) && methodAccessObj is JsonElement methodAccessJson)
        {
            var permissions = JsonSerializer.Deserialize<Dictionary<string, bool>>(methodAccessJson.GetRawText());
            if (permissions != null && permissions.TryGetValue(method.ToLower(), out bool hasMethodAccess))
            {
                return hasMethodAccess;
            }
        }

        return false;
    }
}

public class ApiKeyAuthorizationMiddleware
{
    private readonly RequestDelegate next;

    public ApiKeyAuthorizationMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    private bool IsAuthorized(HttpContext context)
    {
        List<string> apiKeySingle = new List<string> { "f6g7h8i9j0" }; // apikeys that have only access to single
        string? apiKey = context.Request.Headers["API_KEY"];

        if (apiKey is null)
        {
            return false;
        }

        string[] endpointSplit = context.Request.Path.ToString().Split('/');
        if (endpointSplit.Length < 4)
        {
            return false;
        }

        string endpoint = endpointSplit[3]; // .../api/vX/endpoint
        string version = endpointSplit[2].ToUpper(); // .../api/vX
        string method = context.Request.Method.ToLower();
        if (version == "V2")
        {
            if (apiKeySingle.Contains(apiKey))
            {
                if (endpointSplit.Length <= 4)
                {
                    return false;
                }
            }
        }

        string path = $"api{version}/ApiKeys/api_keys.json";
        ApiKeys.Initialize(path);

        return ApiKeys.HasAccess(apiKey, endpoint, method);
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Misschien hier de context.Request.Path.ToString().Split('/'),
        // en dan als het kleiner is dan 4, 404 returnen (NotFound), want invalid URL
        if (!this.IsAuthorized(context))
        {
            context.Response.StatusCode = 401;
            return;
        }

        await this.next(context);
    }
}

public static class ApiKeyAuthorizationMiddlewareExtensions
{
    public static IApplicationBuilder UseApiKeyAuthorization(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ApiKeyAuthorizationMiddleware>();
    }
}
