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
    public class GenerateConsumerReportConsumer
        : IConsumer<GenerateConsumerReport>
    {
        private readonly IOptions<ConsumerOptions> _options;
        private readonly ILogger<GenerateConsumerReportConsumer> _logger;

        public GenerateConsumerReportConsumer(IOptions<ConsumerOptions> options, ILogger<GenerateConsumerReportConsumer> logger)
        {
            _options = options;
            _logger = logger;
        }

        protected ConsumerOptions Options => _options.Value;
        
        public async Task Consume(ConsumeContext<GenerateConsumerReport> context)
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
            var reportPath = Path.Join(Options.ReportPath, $"consumer-{Environment.MachineName}-{date:yyyymmdd}.json");
            await File.WriteAllTextAsync(reportPath, report);
        }
    }
}
