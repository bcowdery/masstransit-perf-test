using System;
using Microsoft.Extensions.DependencyInjection;
using PerfTest.Consumer.Data;
using PerfTest.Consumer.Data.Repositories;
using PerfTest.Consumer.Data.Settings;

namespace PerfTest.Bootstrap
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds PerfTest services to the DI container.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configure"></param>
        public static void AddPerfTestServices(this IServiceCollection services, Action<PerfTestConfiguration> configure)
        {
            var config = new PerfTestConfiguration();
            configure(config);
            
            // SQLite database
            services.AddSingleton(new StatsDbOptions() { ConnectionString = config.ConnectionString });
            services.AddScoped<IStatsDbContext, StatsDbContext>();
            services.AddScoped<IStatsRepository, StatsRepository>();
            
            services.AddHostedService<StatsDbStartupService>();
        }
    }

    /// <summary>
    /// Configuration for the PerfTest service
    /// </summary>
    public class PerfTestConfiguration
    {
        public string ConnectionString { get; set; }
    }
}
