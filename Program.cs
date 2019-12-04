using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace repro
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration((hostingContext, config) =>
                    {
                        config.AddAzureAppConfiguration(options =>
                        {
                            options.Connect(Environment.GetEnvironmentVariable("AppConfiguration__ConnectionString"))
                            .Select(KeyFilter.Any, LabelFilter.Null)
                            .Select(KeyFilter.Any, "dev")
                            .UseFeatureFlags(fo =>
                            {
                                fo.CacheExpirationTime = TimeSpan.FromSeconds(1);
                                //options.Label = env; //commented out as this seems to break updates in 1.0.0-preview-009000001-1251
                            });
                        });
                    });
                    webBuilder.ConfigureServices(services =>
                    {
                        Startup.ConfigureServices(services);
                    });
                    webBuilder.Configure(app =>
                    {
                        Startup.Configure(app, app.ApplicationServices.GetRequiredService<Microsoft.AspNetCore.Hosting.IWebHostEnvironment>());
                    });
                });
    }
}
