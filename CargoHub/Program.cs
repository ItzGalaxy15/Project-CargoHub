using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http.Extensions;
using V1;
using V2;
using Loadtest;

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
app.UseLoggingMiddleware();
app.MapControllers();

// app.Run();
var apptask = app.RunAsync();

var loadtesttask = Loadtest2.Main(args);

await Task.WhenAll(apptask, loadtesttask);