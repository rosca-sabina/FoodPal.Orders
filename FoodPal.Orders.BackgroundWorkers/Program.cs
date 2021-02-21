using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FoodPal.Orders.BackgroundWorkers
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices(HostServices.Configure)
            .ConfigureLogging((hostingContext, loggingBuilder) => {
                loggingBuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
            });
    }
}
