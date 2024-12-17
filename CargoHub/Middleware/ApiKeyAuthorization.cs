/* Voor nu in een random json file, omdat ik nog niet weet wat de juiste manier is
 * Misschien de api key encrypten, alleen dan weten we daarna nooit meer de originele value van de api key
 */
using System.Runtime.CompilerServices;

public static class ApiKeys
{
    private static Dictionary<string, Dictionary<string, Dictionary<string, bool>>> apiKeyEndpoints = new Dictionary<string, Dictionary<string, Dictionary<string, bool>>>();

    public static void Initialize(string path)
    {
        apiKeyEndpoints = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, Dictionary<string, bool>>>>(File.ReadAllText(path)) ??
            throw new Exception($"Could not read api keys: {path}");
    }

    public static bool HasAccess(string apiKey, string endpoint, string method)
    {
        var access = apiKeyEndpoints.GetValueOrDefault(apiKey);
        if (access is null)
        {
            return false;
        }

        var endpointAccess = access.GetValueOrDefault(endpoint);
        if (endpointAccess is null)
        {
            return false;
        }

        bool methodAccess = endpointAccess.GetValueOrDefault(method);

        return methodAccess;
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
        List<string> apiKeySingle = new List<string> { "empty" }; // apikeys that have only access to single
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

        if (apiKeySingle.Contains(apiKey))
        {
            if (endpointSplit.Length <= 4)
            {
                return false;
            }
        }

        string endpoint = endpointSplit[3]; // .../api/vX/endpoint
        string version = endpointSplit[2].ToUpper(); // .../api/vX
        string method = context.Request.Method.ToLower();

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
