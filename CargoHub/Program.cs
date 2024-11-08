var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddTransient<IClientProvider, ClientProvider>();
builder.Services.AddScoped<IClientService, ClientService>();

builder.Services.AddTransient<ILocationProvider, LocationProvider>();
builder.Services.AddScoped<ILocationService, LocationService>();

builder.Services.AddTransient<IItemTypeProvider, ItemTypeProvider>();
builder.Services.AddScoped<IItemTypeService, ItemTypeService>();

// builder.Services.AddTransient<IOrderProvider, OrderProvider>();
// builder.Services.AddScoped<IOrderService, OrderService>();
// builder.Services.AddScoped<IOrderValidationService, OrderValidationService>();

builder.Services.AddTransient<ISupplierProvider, SupplierProvider>();
builder.Services.AddScoped<ISupplierService, SupplierService>();

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

builder.Services.AddTransient<IItemGroupProvider,ItemGroupProvider>();
builder.Services.AddScoped<IItemGroupService, ItemGroupService>();

//builder.Services.AddSingleton<IInventoryProvider, InventoryProvider>(); error
//builder.Services.AddScoped<IInventoryService, InventoryService>();

//builder.Services.AddSingleton<IItemLineProvider, ItemLineProvider>(); erorr
//builder.Services.AddScoped<IItemLineService, ItemLineService>();

builder.Services.AddScoped<IClientValidation, ClientValidation>();
builder.Services.AddScoped<ILocationValidation, LocationValidation>();
builder.Services.AddScoped<IItemTypeValidation, ItemTypeValidation>();
// builder.Services.AddScoped<IOrderValidationService, OrderValidationService>();
builder.Services.AddScoped<ISupplierValidationService, SupplierValidationService>();
//builder.Services.AddScoped<IShipmentValidationService, ShipmentValidationService>();
builder.Services.AddScoped<ITransferValidationService, TransferValidationService>();
//builder.Services.AddScoped<IItemValidationService, ItemValidationService>();
builder.Services.AddScoped<IWarehouseValidationService, WarehouseValidationService>();
builder.Services.AddScoped<IItemGroupValidationService, ItemGroupValidationService>();

builder.Services.AddTransient<IInventoryProvider, InventoryProvider>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IInventoryValidationService, InventoryValidationService>();

builder.Services.AddTransient<IItemLineProvider, ItemLineProvider>();
builder.Services.AddScoped<IItemLineService, ItemLineService>();                               
builder.Services.AddScoped<IItemLineValidationService, ItemLineValidationService>();


var app = builder.Build();
app.Urls.Add("http://localhost:3000");

app.MapControllers();

app.UseApiKeyAuthorization();

app.Run();

