using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PerfTest.Commands;
using PerfTest.Producer.Producers;
using PerfTest.Producer.Settings;

namespace PerfTest.Producer.Workers
{
    /// <summary>
    /// Background service that executes message producers to test service bus transport throughput.
    /// </summary>
    public class ProducerBackgroundService
        : BackgroundService
    {
        private readonly IEnumerable<IProducer> _producers;
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly ILogger<ProducerBackgroundService> _logger;
        private readonly IOptions<ProducerOptions> _producerOptions;

        public ProducerBackgroundService(IEnumerable<IProducer> producers,
            ISendEndpointProvider sendEndpointProvider,
            IOptions<ProducerOptions> producerOptions,
            ILogger<ProducerBackgroundService> logger)
        {
            _producers = producers;
            _sendEndpointProvider = sendEndpointProvider;
            _producerOptions = producerOptions;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting test ...");
            
            foreach (var producer in _producers)
            {
                _logger.LogInformation("Executing message producer = {ProducerType}", producer.Type);
                
                await producer.ExecuteAsync(_sendEndpointProvider, _producerOptions.Value, stoppingToken);
            }

            _logger.LogInformation("Finished test.");
            
            // Generate consumer statistics reports
            var consumerReportEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:generate-consumer-report"));
            await consumerReportEndpoint.Send<GenerateConsumerReport>(new {}, stoppingToken);
            
            // Generate producer statistics reports
            var producerReportEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:generate-producer-report"));
            await producerReportEndpoint.Send<GenerateProducerReport>(new {}, stoppingToken);            
        }
    }
}
