var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSingleton<IClientProvider, ClientProvider>();
builder.Services.AddScoped<IClientService, ClientService>();

builder.Services.AddSingleton<IOrderProvider, OrderProvider>();
builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddSingleton<ISupplierProvider, SupplierProvider>();
builder.Services.AddScoped<ISupplierService, SupplierService>();

builder.Services.AddSingleton<IShipmentProvider, ShipmentProvider>();
builder.Services.AddScoped<IShipmentService, ShipmentService>();

builder.Services.AddSingleton<ITransferProvider, TransferProvider>();
builder.Services.AddScoped<ITransferService, TransferService>();

var app = builder.Build();
app.Urls.Add("http://localhost:3000");

app.MapControllers();
app.Run();

