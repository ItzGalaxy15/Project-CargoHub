using V1;
using V2;

namespace CargoHub
{
    /// <summary>
    /// The main entry point for the CargoHub application.
    public class Program
    {
        /// <summary>
        /// The main entry point for the CargoHub application.
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ServiceConfigurationV1.ConfigureServices(builder.Services);
            ServiceConfigurationV2.ConfigureServices(builder.Services);

            var app = builder.Build();
            app.Urls.Add("http://localhost:3000");

            app.MapControllers();

            app.UseApiKeyAuthorization();

            app.Run();
        }
    }
}