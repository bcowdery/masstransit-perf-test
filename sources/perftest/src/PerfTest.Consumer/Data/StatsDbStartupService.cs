using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace PerfTest.Consumer.Data
{
    /// <summary>
    /// Creates the stats DB on startup.
    /// </summary>
    public class StatsDbStartupService
        : IHostedService
    {
        private readonly IStatsDbContext _statsDbContext;

        public StatsDbStartupService(IStatsDbContext statsDbContext)
        {
            _statsDbContext = statsDbContext;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _statsDbContext.EnsureCreated();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
