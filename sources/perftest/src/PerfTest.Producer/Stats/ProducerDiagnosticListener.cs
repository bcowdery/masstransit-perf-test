using System;
using System.Collections.Generic;
using System.Diagnostics;
using MassTransit.Internals.Extensions;
using MassTransit.Logging;
using PerfTest.Producer.Stats;

namespace PerfTest.Producer.Stats
{
    public class ProducerDiagnosticListener
        : IObserver<DiagnosticListener>
    {
        IDisposable _handle;

        public void OnCompleted() { }

        public void OnError(Exception error) { }

        public void OnNext(DiagnosticListener value)
        {
            if (value.Name == LogCategoryName.MassTransit)
            {
                _handle = value.Subscribe(new ProducerStatsListenerObserver());
            }
        }
    }
    
    internal class ProducerStatsListenerObserver 
        : IObserver<KeyValuePair<string, object>>
    {
        public void OnCompleted() { }

        public void OnError(Exception error) { }

        public void OnNext(KeyValuePair<string, object> value)
        {
            if (value.Key.EndsWith(".Stop", StringComparison.OrdinalIgnoreCase))
            {
                var activity = Activity.Current;
                ProducerStats.AddEvent(activity.OperationName, activity.Duration);
            }
        }
    }    
}
