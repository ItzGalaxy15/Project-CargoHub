var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSingleton<IClientProvider, ClientProvider>();
builder.Services.AddScoped<IClientService, ClientService>();

var app = builder.Build();
app.Urls.Add("http://localhost:3000");

app.MapControllers();
app.Run();

