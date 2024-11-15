using apiV1.Interfaces;
using apiV1.Services;
using apiV1.Validations;
using apiV1.ValidationInterfaces;

namespace V1
{
    public static class ServiceConfigurationV1
    {
        public static void ConfigureServices(IServiceCollection Services)
        {
            Services.AddControllersWithViews();

            Services.AddTransient<IClientProvider, ClientProvider>();
            Services.AddScoped<IClientService, ClientService>();
            Services.AddScoped<IClientValidationService, ClientValidationService>();

            Services.AddTransient<ILocationProvider, LocationProvider>();
            Services.AddScoped<ILocationService, LocationService>();
            Services.AddScoped<ILocationValidationService, LocationValidationService>();

            Services.AddTransient<IItemTypeProvider, ItemTypeProvider>();
            Services.AddScoped<IItemTypeService, ItemTypeService>();
            Services.AddScoped<IItemTypeValidationService, ItemTypeValidationService>();

            Services.AddTransient<IOrderProvider, OrderProvider>();
            Services.AddScoped<IOrderService, OrderService>();
            Services.AddScoped<IOrderValidationService, OrderValidationService>();

            Services.AddTransient<ISupplierProvider, SupplierProvider>();
            Services.AddScoped<ISupplierService, SupplierService>();
            Services.AddScoped<ISupplierValidationService, SupplierValidationService>();

            Services.AddTransient<IShipmentProvider, ShipmentProvider>();
            Services.AddScoped<IShipmentService, ShipmentService>();
            Services.AddScoped<IShipmentValidationService, ShipmentValidationService>();

            Services.AddTransient<ITransferProvider, TransferProvider>();
            Services.AddScoped<ITransferService, TransferService>();
            Services.AddScoped<ITransferValidationService, TransferValidationService>();

            Services.AddTransient<IItemProvider, ItemProvider>();
            Services.AddScoped<IItemService, ItemService>();
            Services.AddScoped<IItemValidationService, ItemValidationService>();

            Services.AddTransient<IWarehouseProvider, WarehouseProvider>();
            Services.AddScoped<IWarehouseService, WarehouseService>();
            Services.AddScoped<IWarehouseValidationService, WarehouseValidationService>();

            Services.AddTransient<IItemGroupProvider,ItemGroupProvider>();
            Services.AddScoped<IItemGroupService, ItemGroupService>();
            Services.AddScoped<IItemGroupValidationService, ItemGroupValidationService>();

            Services.AddSingleton<IInventoryProvider, InventoryProvider>();
            Services.AddScoped<IInventoryService, InventoryService>();
            Services.AddScoped<IInventoryValidationService, InventoryValidationService>();

            Services.AddSingleton<IItemLineProvider, ItemLineProvider>();
            Services.AddScoped<IItemLineService, ItemLineService>();                            
            Services.AddScoped<IItemLineValidationService, ItemLineValidationService>();
        }
    }
}