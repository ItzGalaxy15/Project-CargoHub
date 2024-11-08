var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddTransient<IClientProvider, ClientProvider>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IClientValidation, ClientValidation>();

builder.Services.AddTransient<ILocationProvider, LocationProvider>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<ILocationValidation, LocationValidation>();


builder.Services.AddTransient<IItemTypeProvider, ItemTypeProvider>();
builder.Services.AddScoped<IItemTypeService, ItemTypeService>();
builder.Services.AddScoped<IItemTypeValidation, ItemTypeValidation>();


// builder.Services.AddTransient<IOrderProvider, OrderProvider>();
// builder.Services.AddScoped<IOrderService, OrderService>();
// builder.Services.AddScoped<IOrderValidationService, OrderValidationService>();

builder.Services.AddTransient<ISupplierProvider, SupplierProvider>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<ISupplierValidationService, SupplierValidationService>();


// builder.Services.AddTransient<IShipmentProvider, ShipmentProvider>();
// builder.Services.AddScoped<IShipmentService, ShipmentService>();
// builder.Services.AddScoped<IShipmentValidationService, ShipmentValidationService>();

builder.Services.AddTransient<ITransferProvider, TransferProvider>();
builder.Services.AddScoped<ITransferService, TransferService>();
builder.Services.AddScoped<TransferValidationService, TransferValidationService>();

builder.Services.AddTransient<IItemProvider, ItemProvider>();
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<IItemValidationService, ItemValidationService>();

builder.Services.AddTransient<IWarehouseProvider, WarehouseProvider>();
builder.Services.AddScoped<IWarehouseService, WarehouseService>();
builder.Services.AddScoped<IWarehouseValidationService, WarehouseValidationService>();


builder.Services.AddTransient<IItemGroupProvider,ItemGroupProvider>();
builder.Services.AddScoped<IItemGroupService, ItemGroupService>();
builder.Services.AddScoped<IItemGroupValidationService, ItemGroupValidationService>();


builder.Services.AddSingleton<IInventoryProvider, InventoryProvider>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddTransient<IInventoryProvider, InventoryProvider>();


builder.Services.AddSingleton<IItemLineProvider, ItemLineProvider>();
builder.Services.AddScoped<IItemLineService, ItemLineService>();                            
builder.Services.AddScoped<IItemLineValidationService, ItemLineValidationService>();


var app = builder.Build();
app.Urls.Add("http://localhost:3000");

app.MapControllers();

app.UseApiKeyAuthorization();

app.Run();

