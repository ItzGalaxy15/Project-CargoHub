var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSingleton<IClientProvider, ClientProvider>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IClientValidation, ClientValidation>();

builder.Services.AddSingleton<ILocationProvider, LocationProvider>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<ILocationValidation, LocationValidation>();

builder.Services.AddSingleton<IItemTypeProvider, ItemTypeProvider>();
builder.Services.AddScoped<IItemTypeService, ItemTypeService>();
builder.Services.AddScoped<IItemTypeValidation, ItemTypeValidation>();

builder.Services.AddSingleton<IOrderProvider, OrderProvider>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderValidationService, OrderValidationService>();

builder.Services.AddSingleton<ISupplierProvider, SupplierProvider>();
builder.Services.AddScoped<ISupplierService, SupplierService>();
builder.Services.AddScoped<ISupplierValidationService, SupplierValidationService>();

builder.Services.AddSingleton<IShipmentProvider, ShipmentProvider>();
builder.Services.AddScoped<IShipmentService, ShipmentService>();
builder.Services.AddScoped<IShipmentValidationService, ShipmentValidationService>();

builder.Services.AddSingleton<ITransferProvider, TransferProvider>();
builder.Services.AddScoped<ITransferService, TransferService>();

builder.Services.AddSingleton<IItemProvider, ItemProvider>();
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<IItemValidationService, ItemValidationService>();

builder.Services.AddSingleton<IWarehouseProvider, WarehouseProvider>();
builder.Services.AddScoped<IWarehouseService, WarehouseService>();
builder.Services.AddScoped<IWarehouseValidationService, WarehouseValidationService>();


builder.Services.AddSingleton<IItemGroupProvider,ItemGroupProvider>();
builder.Services.AddScoped<IItemGroupService, ItemGroupService>();
builder.Services.AddScoped<IItemGroupValidationService, ItemGroupValidationService>();

builder.Services.AddSingleton<IInventoryProvider, InventoryProvider>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IInventoryValidationService, InventoryValidationService>();

builder.Services.AddSingleton<IItemLineProvider, ItemLineProvider>();
builder.Services.AddScoped<IItemLineService, ItemLineService>();
builder.Services.AddScoped<IItemLineValidationService, ItemLineValidationService>();


var app = builder.Build();
app.Urls.Add("http://localhost:3000");

app.MapControllers();
app.Run();

