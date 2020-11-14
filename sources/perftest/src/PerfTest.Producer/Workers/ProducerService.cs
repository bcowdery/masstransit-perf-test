using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PerfTest.Commands;
using PerfTest.Producer.Settings;

namespace PerfTest.Producer.Workers
{
    public class ProducerService
        : BackgroundService
    {
        private static long _executionCount = 0;

        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly ILogger<ProducerService> _logger;
        private readonly IOptions<ProducerOptions> _producerOptions;

        public ProducerService(ISendEndpointProvider sendEndpointProvider,
            IOptions<ProducerOptions> producerOptions,
            ILogger<ProducerService> logger)
        {
            _sendEndpointProvider = sendEndpointProvider;
            _producerOptions = producerOptions;
            _logger = logger;
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var uuid = NewId.NextGuid();
            var options = _producerOptions.Value;
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:record-timestamp"));
            
            while (!stoppingToken.IsCancellationRequested && _executionCount < options.MaxSendCount)
            {
                Task[] tasks = new Task[options.MaxSendConcurrency];
                
                for (int i = 0; i < tasks.Length; i++)
                {
                    _logger.LogInformation("Sending {Index}/{Max}, total {Count} ...", i, options.MaxSendConcurrency, _executionCount);
                    
                    tasks[i] = endpoint.Send<RecordTimestamp>(new
                    {
                        TaskId = i,
                        ThreadId = Thread.CurrentThread.ManagedThreadId,
                        ExecutionCount = _executionCount,
                        SentTime = DateTime.UtcNow
                    });

                    _executionCount++;
                }

                Task.WaitAll(tasks);

                if (options.Interval > 0)
                {
                    await Task.Delay(options.Interval, stoppingToken);
                }
            }
            
            
        }
    }
}
