using apiV2.Interfaces;
using apiV2.Services;
//using apiV2.Validations;
//using apiV2.ValidationInterfaces;

namespace V2
{
    public static class ServiceConfigurationV2
    {
        public static void ConfigureServices(IServiceCollection Services)
        {
            Services.AddScoped<IClientService, ClientService>();

            Services.AddScoped<ILocationService, LocationService>();

            Services.AddScoped<IItemTypeService, ItemTypeService>();

            Services.AddScoped<IShipmentService, ShipmentService>();
        }
    }
}