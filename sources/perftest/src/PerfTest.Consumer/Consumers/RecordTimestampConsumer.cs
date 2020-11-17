using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using PerfTest.Commands;
using PerfTest.Consumer.Stats;

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

        public async Task Consume(ConsumeContext<RecordTimestamp> context)
        {
            var receivedTime = DateTime.UtcNow;
            var message = context.Message;
            
            _logger.LogInformation("Consumed message {MessageId}", context.MessageId);
            _logger.LogInformation("Task {TaskId}, Thread {ThreadId}, Producer {ProducerId}, Sent at {SentTime}",
                message.TaskId,
                message.ThreadId,
                message.ProducerId,
                message.SentTime);

            ConsumerStats.Add(message.ProducerId, (receivedTime - message.SentTime));
        }
    }
}
