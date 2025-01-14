using System.Threading;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Options;

public class LoggingMiddleware
{
    private readonly RequestDelegate next;
    private static readonly SemaphoreSlim Semaphore = new SemaphoreSlim(1, 1);

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

        await Semaphore.WaitAsync();
        try
        {
            if (context.Request.Method == HttpMethods.Put || context.Request.Method == HttpMethods.Post || context.Request.Method == HttpMethods.Patch)
            {
                await File.AppendAllTextAsync(
                logFileOptions.LogPath,
                $"\n{DateTime.Now} - {context.Connection.RemoteIpAddress} requested {context.Request.Method} {context.Request.GetDisplayUrl()}");

                context.Request.EnableBuffering();

                var body = await new StreamReader(context.Request.Body).ReadToEndAsync();
                context.Request.Body.Position = 0;

                await this.next(context);

                await File.AppendAllTextAsync(
                    logFileOptions.LogPath,
                    $"\t | \tResponded with status code: {context.Response.StatusCode} \nRequest Body: {body}");
            }
        }
        finally
        {
            Semaphore.Release();
        }
    }
}

public static class LoggingMiddlewareExtensions
{
    public static IApplicationBuilder UseLoggingMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<LoggingMiddleware>();
    }
}