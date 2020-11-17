using System;
using System.Collections.Generic;
using System.Diagnostics;
using MassTransit.Internals.Extensions;
using MassTransit.Logging;
using PerfTest.Consumer.Stats;
using PerfTest.Producer.Stats;

namespace PerfTest.Producer.Stats
{
    public class ConsumerDiagnosticListener
        : IObserver<DiagnosticListener>
    {
        IDisposable _handle;

        public void OnCompleted() { }

        public void OnError(Exception error) { }

        public void OnNext(DiagnosticListener value)
        {
            if (value.Name == LogCategoryName.MassTransit)
            {
                _handle = value.Subscribe(new ConsumerStatsListenerObserver());
            }
        }
    }
    
    internal class ConsumerStatsListenerObserver 
        : IObserver<KeyValuePair<string, object>>
    {
        public void OnCompleted() { }

        public void OnError(Exception error) { }

        public void OnNext(KeyValuePair<string, object> value)
        {
            if (value.Key.EndsWith(".Stop", StringComparison.OrdinalIgnoreCase))
            {
                var activity = Activity.Current;
                ConsumerStats.AddEvent(activity.OperationName, activity.Duration);
            }
        }
    }    
}
