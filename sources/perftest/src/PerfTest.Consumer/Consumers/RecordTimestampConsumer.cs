using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using PerfTest.Commands;
using PerfTest.Consumer.Data;
using PerfTest.Consumer.Data.Entities;
using PerfTest.Consumer.Data.Repositories;

namespace PerfTest.Consumer.Consumers
{
    public class RecordTimestampConsumer
        : IConsumer<RecordTimestamp>
    {
        private readonly ILogger<RecordTimestampConsumer> _logger;
        private readonly StatsRepository _statsRepository;

        public RecordTimestampConsumer(ILogger<RecordTimestampConsumer> logger, StatsRepository statsRepository)
        {
            _logger = logger;
            _statsRepository = statsRepository;
        }

        public async Task Consume(ConsumeContext<RecordTimestamp> context)
        {
            _logger.LogInformation("Consumed message {MessageId}", context.MessageId);

            var message = context.Message;
            var operation = Operation.CreateNew(message.TaskId, message.ThreadId, message.SentTime, DateTime.UtcNow);

            await _statsRepository.InsertOperation(operation);
        }
    }
}
