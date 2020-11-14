using System;
using System.Threading;
using MassTransit;

namespace PerfTest.Producer.Producers
{
    public class SingleQueueProducer
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public SingleQueueProducer(ISendEndpointProvider sendEndpointProvider)
        {
            _sendEndpointProvider = sendEndpointProvider;
        }

        public Guid ProducerId { get; set; }
        
        public void ExecuteProducer(CancellationToken stoppingToken)
        {
            
        }
    }
}
