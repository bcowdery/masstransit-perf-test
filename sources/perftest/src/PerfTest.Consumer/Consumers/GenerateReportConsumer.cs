using System;
using System.IO;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PerfTest.Commands;
using PerfTest.Consumer.Settings;
using PerfTest.Consumer.Stats;

namespace PerfTest.Consumer.Consumers
{
    public class GenerateReportConsumer
        : IConsumer<GenerateReport>
    {
        private readonly IOptions<ConsumerOptions> _options;
        private readonly ILogger<GenerateReportConsumer> _logger;

        public GenerateReportConsumer(IOptions<ConsumerOptions> options, ILogger<GenerateReportConsumer> logger)
        {
            _options = options;
            _logger = logger;
        }

        protected ConsumerOptions Options => _options.Value;
        
        public async Task Consume(ConsumeContext<GenerateReport> context)
        {
            _logger.LogInformation("Generating report ...");
            
            // Wait for queues to drain and all activity to complete
            while (ConsumerStats.LastUpdated < Options.InactivityTimeout)
            {
                await Task.Delay(TimeSpan.FromSeconds(10));
            }
            
            // Generate the report
            var report = ConsumerStats.ToJson();
            
            _logger.LogInformation(report);
            
            // Writer report to disk
            var date = DateTime.Now;
            var reportPath = Path.Join(Options.ReportPath, $"{date:yyyymmdd}.json");
            await File.WriteAllTextAsync(reportPath, report);
        }
    }
}
