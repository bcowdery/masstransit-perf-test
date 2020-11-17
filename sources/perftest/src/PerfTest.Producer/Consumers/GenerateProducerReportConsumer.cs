using System;
using System.IO;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PerfTest.Commands;
using PerfTest.Producer.Settings;
using PerfTest.Producer.Stats;

namespace PerfTest.Producer.Consumers
{
    public class GenerateProducerReportConsumer
        : IConsumer<GenerateProducerReport>
    {
        private readonly IOptions<ProducerOptions> _options;
        private readonly ILogger<GenerateProducerReportConsumer> _logger;

        public GenerateProducerReportConsumer(IOptions<ProducerOptions> options, ILogger<GenerateProducerReportConsumer> logger)
        {
            _options = options;
            _logger = logger;
        }
        
        public async Task Consume(ConsumeContext<GenerateProducerReport> context)
        {
            _logger.LogInformation("Generating report ...");

            // Generate the report
            var report = ProducerStats.ToJson();
            
            _logger.LogInformation(report);
            
            // Writer report to disk
            var date = DateTime.Now;
            var options = _options.Value;
            var reportPath = Path.Join(options.ReportPath, $"producer-{Environment.MachineName}-{date:yyyymmdd}.json");
            await File.WriteAllTextAsync(reportPath, report);
        }
    }
}
