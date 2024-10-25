/* Voor nu in een random json file, omdat ik nog niet weet wat de juiste manier is
 * Misschien de api key encrypten, alleen dan weten we daarna nooit meer de originele value van de api key
 */

public static class ApiKeys {
    private static string path = "api/Authorization/ApiKeys/api_keys.json";
    private static readonly Dictionary<string, Dictionary<string, Dictionary<string, bool>>> ApiKeyEndpoints = 
        System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, Dictionary<string, bool>>>>(File.ReadAllText(path)) ??
        throw new Exception($"Could not read api keys: {path}"); // voor nu een exception, later misschien een default waarde (voor als CI/CD een rol gaat spelen)


    public static bool HasAccess(string apiKey, string endpoint, string method) {
        var access = ApiKeyEndpoints.GetValueOrDefault(apiKey);
        if (access is null) return false;

        var endpointAccess = access.GetValueOrDefault(endpoint);
        if (endpointAccess is null) return false;

        bool methodAccess = endpointAccess.GetValueOrDefault(method);
        return methodAccess;
    }
}


public class ApiKeyAuthorizationMiddleware
{
    private readonly RequestDelegate _next;

    public ApiKeyAuthorizationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    private bool IsAuthorized(HttpContext context)
    {
        string? ApiKey = context.Request.Headers["API_KEY"];

        if (ApiKey is null) return false;

        string[] endpointSplit = context.Request.Path.ToString().Split('/');
        if (endpointSplit.Length < 4) return false;

        string endpoint = context.Request.Path.ToString().Split('/')[3]; // .../api/vX/endpoint
        string method = context.Request.Method.ToLower();

        return ApiKeys.HasAccess(ApiKey, endpoint, method);
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Misschien hier de context.Request.Path.ToString().Split('/'),
        // en dan als het kleiner is dan 4, 404 returnen (NotFound), want invalid URL

        if (!IsAuthorized(context)){
            context.Response.StatusCode = 401;
            return;
        }

        await _next(context);
    }
}


public static class ApiKeyAuthorizationMiddlewareExtensions
{
    public static IApplicationBuilder UseApiKeyAuthorization(this IApplicationBuilder builder){
        return builder.UseMiddleware<ApiKeyAuthorizationMiddleware>();
    }
}
