using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FoodPal.Orders.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureAppConfiguration((hostingContext, configSources) =>
				{
					var env = hostingContext.HostingEnvironment;
					configSources.Sources.Clear();

					if (env.IsDevelopment())
					{
						configSources.AddJsonFile("appsettings.json", true, true);
						configSources.AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true);
					}
					configSources.AddEnvironmentVariables();
				})
				.ConfigureLogging((hostingContext, configLogging) =>
				{
					configLogging.ClearProviders();
					configLogging
					.AddConsole(x =>
					{
						x.IncludeScopes = true;
					}).AddDebug();
				})
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});
	}
}
