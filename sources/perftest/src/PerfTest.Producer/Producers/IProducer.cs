using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using PerfTest.Producer.Settings;

namespace PerfTest.Producer.Producers
{
    /// <summary>
    /// Defines a producer that sends messages to a MassTransit queue.
    /// </summary>
    public interface IProducer
    {
        Guid Id { get; }
        string Type { get; }
        Task ExecuteAsync(ISendEndpointProvider endpointProvider, ProducerOptions options, CancellationToken stoppingToken);
    }
}
