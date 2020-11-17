using System;
using System.Diagnostics;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PerfTest.Commands;
using PerfTest.Producer.Consumers;
using PerfTest.Producer.Producers;
using PerfTest.Producer.Workers;
using PerfTest.Producer.Settings;
using PerfTest.Producer.Stats;

namespace PerfTest.Producer
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            using var host = CreateHostBuilder(args).Build();
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;
                    config.SetBasePath(env.ContentRootPath);
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                    config.AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true);
                    config.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
                    config.AddEnvironmentVariables();
                })
                .ConfigureLogging((hostContext, logging) =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })                
                .ConfigureServices((hostContext, services) =>
                {
                    var config = hostContext.Configuration;

                    DiagnosticListener.AllListeners.Subscribe(new ProducerDiagnosticListener());

                    // Configuration options
                    services.AddOptions();
                    services.Configure<ProducerOptions>(config.GetSection(ProducerOptions.Producer));
                    
                    // Test message producers
                    services.AddScoped<IProducer, SingleQueueProducer>();
                    services.AddHostedService<ProducerBackgroundService>();
                    
                    // Mass Transit
                    services.AddMassTransit(x =>
                    {
                        x.AddConsumer<GenerateProducerReportConsumer>();
                        
                        x.SetKebabCaseEndpointNameFormatter();
                        
                        x.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.Host(config.GetConnectionString("Rabbit"));
                            cfg.ConfigureEndpoints(context);
                        });
                    });
                    
                    services.AddMassTransitHostedService();
                })
                .UseConsoleLifetime();                
    }
}
