using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http.Extensions;
using V1;
using V2;

var builder = WebApplication.CreateBuilder(args);

// Add Swagger generator
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

ServiceConfigurationV1.ConfigureServices(builder.Services);
ServiceConfigurationV2.ConfigureServices(builder.Services);

builder.Services.Configure<LogFileOptions>(builder.Configuration.GetSection("LogFile"));

var app = builder.Build();
app.Urls.Add("http://localhost:3000");

// Enable Swagger
app.UseSwagger();
app.UseSwaggerUI();

app.UseApiKeyAuthorization();

// Logging middleware
app.Use(async (context, next) =>
{
    var logFileOptions = context.RequestServices.GetService<IOptions<LogFileOptions>>()?.Value ??
        new LogFileOptions { LogPath = "Logs/RequestLogs.txt" };

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

    await next.Invoke();

    await File.AppendAllTextAsync(
        logFileOptions.LogPath,
        $"\t | \tResponded with status code: {context.Response.StatusCode}");
});

app.MapControllers();

app.Run();
