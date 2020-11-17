using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using PerfTest.Commands;
using PerfTest.Producer.Settings;

namespace PerfTest.Producer.Producers
{
    /// <summary>
    /// Sends messages to a single queue endpoint.
    /// </summary>
    public class SingleQueueProducer
        : IProducer
    {
        private readonly ILogger<SingleQueueProducer> _logger;
        private int _executionCount;

        public SingleQueueProducer(ILogger<SingleQueueProducer> logger)
        {
            _logger = logger;
        }
        
        public Guid Id { get; } = NewId.NextGuid();
        public string Type { get; } = @"single-queue";
        
        public async Task ExecuteAsync(ISendEndpointProvider endpointProvider, ProducerOptions options, CancellationToken stoppingToken)
        {
            var endpoint = await endpointProvider.GetSendEndpoint(new Uri(@"queue:record-timestamp"));

            while (!stoppingToken.IsCancellationRequested && _executionCount < options.MaxSendCount)
            {
                Task[] tasks = new Task[options.MaxSendConcurrency];
                
                for (int i = 0; i < tasks.Length; i++)
                {
                    _logger.LogInformation("Sending message {Index}/{Total}, total = {ExecutionCount} ...", i, options.MaxSendConcurrency, _executionCount);
                    
                    tasks[i] = endpoint.Send<RecordTimestamp>(new
                    {
                        TaskId = i,
                        ThreadId = Thread.CurrentThread.ManagedThreadId,
                        ExecutionCount = _executionCount,
                        ProducerId = Id,
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
