using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using PerfTest.Commands;

namespace PerfTest.Consumer.Consumers
{
    public class RecordTimestampConsumer
        : IConsumer<RecordTimestamp>
    {
        private readonly ILogger<RecordTimestampConsumer> _logger;

        public RecordTimestampConsumer(ILogger<RecordTimestampConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<RecordTimestamp> context)
        {
            _logger.LogInformation("Consumed message {MessageId}", context.MessageId);

            return Task.CompletedTask;
        }
    }
}
