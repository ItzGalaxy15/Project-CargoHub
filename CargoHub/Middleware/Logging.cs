using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http.Extensions;

public class LoggingMiddleware
{
    private readonly RequestDelegate next;

    public LoggingMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext context, IOptions<LogFileOptions> logFileOptionsAccessor)
    {
        var logFileOptions = logFileOptionsAccessor?.Value ?? new LogFileOptions { LogPath = "Logs/RequestLogs.txt" };
        var logDirectory = Path.GetDirectoryName(logFileOptions.LogPath);

        if (!string.IsNullOrEmpty(logDirectory) && !Directory.Exists(logDirectory))
        {
            Directory.CreateDirectory(logDirectory);
        }

        if (!File.Exists(logFileOptions.LogPath))
        {
            await File.WriteAllTextAsync(logFileOptions.LogPath, string.Empty);
        }

        await File.AppendAllTextAsync(
            logFileOptions.LogPath,
            $"\n{DateTime.Now} - {context.Connection.RemoteIpAddress} requested {context.Request.Method} {context.Request.GetDisplayUrl()}");

        await this.next(context);

        await File.AppendAllTextAsync(
            logFileOptions.LogPath,
            $"\t | \tResponded with status code: {context.Response.StatusCode}");
    }
}

public static class LoggingMiddlewareExtensions
{
    public static IApplicationBuilder UseLoggingMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<LoggingMiddleware>();
    }
}
