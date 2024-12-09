using apiV2.Interfaces;
using apiV2.Services;
using apiV2.Validations;
using apiV2.ValidationInterfaces;

namespace V2
{
    public static class ServiceConfigurationV2
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IClientValidationService, ClientValidationService>();

            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<ILocationValidationService, LocationValidationService>();

            services.AddScoped<IItemTypeService, ItemTypeService>();
            services.AddScoped<IItemTypeValidationService, ItemTypeValidationService>();

            services.AddScoped<IShipmentService, ShipmentService>();
            services.AddScoped<IShipmentValidationService, ShipmentValidationService>();

            services.AddScoped<IItemGroupService, ItemGroupService>();
            services.AddScoped<IItemGroupValidationService, ItemGroupValidationService>();

            services.AddScoped<IInventoryService, InventoryService>();
            services.AddScoped<IInventoryValidationService, InventoryValidationService>();

            services.AddScoped<ISupplierService, SupplierService>();
            services.AddScoped<ISupplierValidationService, SupplierValidationService>();

            services.AddScoped<IWarehouseService, WarehouseService>();
            services.AddScoped<IWarehouseValidationService, WarehouseValidationService>();

            services.AddScoped<ITransferService, TransferService>();
            services.AddScoped<ITransferValidationService, TransferValidationService>();

            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<IItemValidationService, ItemValidationService>();

            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderValidationService, OrderValidationService>();

            services.AddScoped<IItemLineService, ItemLineService>();
            services.AddScoped<IItemLineValidationService, ItemLineValidationService>();
        }
    }
}