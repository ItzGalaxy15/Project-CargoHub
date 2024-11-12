using apiV2.Interface;
using apiV2.Services;

namespace V2
{
    public static class ServiceConfigurationV2
    {
        public static void ConfigureServices(IServiceCollection Services)
        {
            Services.AddControllersWithViews();

            Services.AddScoped<IClientService, ClientService>();

            Services.AddScoped<ILocationService, LocationService>();
        }
    }
}