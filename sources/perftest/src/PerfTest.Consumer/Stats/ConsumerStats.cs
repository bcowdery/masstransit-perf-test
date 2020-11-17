using System;
using System.Collections.Concurrent;
using Newtonsoft.Json;
using PerfTest.Metrics;

namespace PerfTest.Consumer.Stats
{
    public static class ConsumerStats
    {
        private static readonly ConcurrentDictionary<string, Trend>
            _diagnosticEvents = new ConcurrentDictionary<string, Trend>();
        
        private static readonly ConcurrentDictionary<Guid, Trend> _producers = new ConcurrentDictionary<Guid, Trend>();
        private static readonly Trend _totals = new Trend(Guid.Empty);

        /// <summary>
        /// AddEvent diagnostic events.
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="duration"></param>
        public static void AddEvent(string eventName, TimeSpan duration)
        {
            if (!_diagnosticEvents.ContainsKey(eventName))
            {
                _diagnosticEvents[eventName] = new Trend(eventName);
            }
            
            _diagnosticEvents[eventName].Add(duration);
        }

        /// <summary>
        /// Adds a message duration to the consumer stats
        /// </summary>
        /// <param name="messageProducerId"></param>
        /// <param name="duration"></param>
        public static void Add(Guid messageProducerId, TimeSpan duration)
        {
            if (!_producers.ContainsKey(messageProducerId))
            {
                _producers[messageProducerId] = new Trend(messageProducerId);
            }
            
            _totals.Add(duration);
            _producers[messageProducerId].Add(duration);
        }

        /// <summary>
        /// Time since the last update
        /// </summary>
        public static TimeSpan LastUpdated => _totals.LastUpdated;

        /// <summary>
        /// Print statistics for a specific message provider as a JSON string.
        /// </summary>
        /// <param name="messageProviderId"></param>
        /// <returns></returns>
        public static string ToJson(Guid messageProviderId) => JsonConvert.SerializeObject(new
        {
            Producer = _producers[messageProviderId]
        });
        
        /// <summary>
        /// Print all tracked statistics as a JSON string
        /// </summary>
        /// <returns></returns>
        public static string ToJson() => JsonConvert.SerializeObject(new
        {
            Total = _totals,
            Producers = _producers,
            Diagnostics = _diagnosticEvents
        });
    }
}
