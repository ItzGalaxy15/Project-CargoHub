using V1;
using V2;

var builder = WebApplication.CreateBuilder(args);

// Add Swagger generator
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

ServiceConfigurationV1.ConfigureServices(builder.Services);
ServiceConfigurationV2.ConfigureServices(builder.Services);

var app = builder.Build();
app.Urls.Add("http://localhost:3000");

// Enable Swagger
app.UseSwagger();
app.UseSwaggerUI();

app.UseApiKeyAuthorization();

app.MapControllers();

app.Run();
