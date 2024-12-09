using apiV1.Interfaces;
using apiV1.Services;
using apiV1.Validations;
using apiV1.ValidationInterfaces;

namespace V1
{
    public static class ServiceConfigurationV1
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddTransient<IClientProvider, ClientProvider>();
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IClientValidationService, ClientValidationService>();

            services.AddTransient<ILocationProvider, LocationProvider>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<ILocationValidationService, LocationValidationService>();

            services.AddTransient<IItemTypeProvider, ItemTypeProvider>();
            services.AddScoped<IItemTypeService, ItemTypeService>();
            services.AddScoped<IItemTypeValidationService, ItemTypeValidationService>();

            services.AddTransient<IOrderProvider, OrderProvider>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderValidationService, OrderValidationService>();

            services.AddTransient<ISupplierProvider, SupplierProvider>();
            services.AddScoped<ISupplierService, SupplierService>();
            services.AddScoped<ISupplierValidationService, SupplierValidationService>();

            services.AddTransient<IShipmentProvider, ShipmentProvider>();
            services.AddScoped<IShipmentService, ShipmentService>();
            services.AddScoped<IShipmentValidationService, ShipmentValidationService>();

            services.AddTransient<ITransferProvider, TransferProvider>();
            services.AddScoped<ITransferService, TransferService>();
            services.AddScoped<ITransferValidationService, TransferValidationService>();

            services.AddTransient<IItemProvider, ItemProvider>();
            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<IItemValidationService, ItemValidationService>();

            services.AddTransient<IWarehouseProvider, WarehouseProvider>();
            services.AddScoped<IWarehouseService, WarehouseService>();
            services.AddScoped<IWarehouseValidationService, WarehouseValidationService>();

            services.AddTransient<IItemGroupProvider, ItemGroupProvider>();
            services.AddScoped<IItemGroupService, ItemGroupService>();
            services.AddScoped<IItemGroupValidationService, ItemGroupValidationService>();

            services.AddSingleton<IInventoryProvider, InventoryProvider>();
            services.AddScoped<IInventoryService, InventoryService>();
            services.AddScoped<IInventoryValidationService, InventoryValidationService>();

            services.AddSingleton<IItemLineProvider, ItemLineProvider>();
            services.AddScoped<IItemLineService, ItemLineService>();
            services.AddScoped<IItemLineValidationService, ItemLineValidationService>();
        }
    }
}