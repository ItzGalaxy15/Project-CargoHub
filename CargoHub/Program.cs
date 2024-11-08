var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSingleton<IClientProvider, ClientProvider>();
builder.Services.AddScoped<IClientService, ClientService>();

builder.Services.AddSingleton<ILocationProvider, LocationProvider>();
builder.Services.AddScoped<ILocationService, LocationService>();

builder.Services.AddSingleton<IItemTypeProvider, ItemTypeProvider>();
builder.Services.AddScoped<IItemTypeService, ItemTypeService>();

// builder.Services.AddSingleton<IOrderProvider, OrderProvider>(); errors
// builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddSingleton<ISupplierProvider, SupplierProvider>();
builder.Services.AddScoped<ISupplierService, SupplierService>();

// builder.Services.AddSingleton<IShipmentProvider, ShipmentProvider>();  errorr
// builder.Services.AddScoped<IShipmentService, ShipmentService>();

builder.Services.AddSingleton<ITransferProvider, TransferProvider>();
builder.Services.AddScoped<ITransferService, TransferService>();

//builder.Services.AddSingleton<IItemProvider, ItemProvider>(); errors
//builder.Services.AddScoped<IItemService, ItemService>();

builder.Services.AddSingleton<IWarehouseProvider, WarehouseProvider>();
builder.Services.AddScoped<IWarehouseService, WarehouseService>();

builder.Services.AddSingleton<IItemGroupProvider,ItemGroupProvider>();
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
//builder.Services.AddScoped<IInventoryValidationService, InventoryValidationService>();
//builder.Services.AddScoped<IItemLineValidationService, ItemLineValidationService>();


var app = builder.Build();
app.Urls.Add("http://localhost:3000");

app.MapControllers();

app.UseApiKeyAuthorization();

app.Run();

