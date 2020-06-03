using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace StowayNet.Extensions.Quartz.Sample
{
    class Program
    {
        private static IConfiguration Configuration { get; set; }

        public static async Task Main(String[] args)
        {
            await new HostBuilder()
              .UseEnvironment("Development")
              .ConfigureServices((hostContext, services) =>
              {
                  var builder = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{AppContext.BaseDirectory}.json", optional: true)
                    .AddEnvironmentVariables();
                  Configuration = builder.Build();
                  services.AddSingleton(Configuration);
                  services.AddLogging(loggingBuilder =>
                  {
                      loggingBuilder.AddFilter("Microsoft", LogLevel.Warning)
                                    .AddFilter("System", LogLevel.Warning)
                                    .AddFilter("Quartz", LogLevel.Information)
                                    .SetMinimumLevel(LogLevel.Debug)
                                    .AddNLog(Configuration);
                  });
                  // configure StowayNet
                  services.AddStowayNet();
              })
              .RunConsoleAsync();

            Console.WriteLine("start...");

        }
    }
}
