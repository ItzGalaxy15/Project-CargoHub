//using api.v1
using V1;
//using V2;

var builder = WebApplication.CreateBuilder(args);

//version control
ServiceConfigurationV1.ConfigureServices(builder.Services);
//ServiceConfigurationV2.ConfigureServices(builder.Services);

var app = builder.Build();
app.Urls.Add("http://localhost:3000");

app.MapControllers();

app.UseApiKeyAuthorization();

app.Run();

