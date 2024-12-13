using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using RateCharts.Controller;
using RateCharts.Repository;
using RateCharts.Services;
using System.Threading.Tasks;

namespace RateCharts
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Set EPPlus LicenseContext
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; 

            var url = "http://localhost:5000/";

            // Manually create dependencies
            var configuration = BuildConfiguration();
            var dbContext = new DBContext(configuration);
            var rateChartRepository = new RateChartRepository(dbContext);
            var excelService = new ExcelService();

            // Create and configure the WebServer
            using var server = new WebServer(o => o
                .WithUrlPrefix(url)
                .WithMode(HttpListenerMode.Microsoft)
            );

            // Configure Web API
            ConfigureWebApi(server, rateChartRepository, excelService);

            // Start the server
            await server.RunAsync();
        }

        // Configure Web API with RateChartController
        public static void ConfigureWebApi(WebServer server, RateChartRepository rateChartRepository, ExcelService excelService)
        {
            // Register RateChartController with Web API
            server.WithWebApi("/api", api =>
            {
                api.WithController(() => new RateChartController(rateChartRepository, excelService));
            });
        }

        // Build configuration from appsettings.json file
        private static IConfiguration BuildConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
        }
    }
}
