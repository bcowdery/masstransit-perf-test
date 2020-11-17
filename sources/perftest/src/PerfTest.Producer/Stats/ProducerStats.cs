using System;
using System.Collections.Concurrent;
using Newtonsoft.Json;
using PerfTest.Metrics;

namespace PerfTest.Producer.Stats
{
    public static class ProducerStats
    {
        private static readonly ConcurrentDictionary<string, Trend>
            _diagnosticEvents = new ConcurrentDictionary<string, Trend>();
        
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
        /// Print all tracked statistics as a JSON string
        /// </summary>
        /// <returns></returns>
        public static string ToJson() => JsonConvert.SerializeObject(new
        {
            Diagnostics = _diagnosticEvents
        });        
    }
}
