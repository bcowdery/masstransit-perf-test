using System;
using System.Collections.Concurrent;
using Newtonsoft.Json;

namespace PerfTest.Consumer.Stats
{
    public static class ConsumerStats
    {
        private static ConcurrentDictionary<Guid, Trend> _trends = new ConcurrentDictionary<Guid, Trend>();
        private static DateTime _lastUpdated = DateTime.Now;
        
        /// <summary>
        /// Adds a message duration to the consumer stats
        /// </summary>
        /// <param name="messageProducerId"></param>
        /// <param name="duration"></param>
        public static void Add(Guid messageProducerId, TimeSpan duration)
        {
            if (!_trends.ContainsKey(messageProducerId))
            {
                _trends[messageProducerId] = new Trend(messageProducerId);
            }
            
            _trends[messageProducerId].Add(duration);
            _lastUpdated = DateTime.Now;
        }

        /// <summary>
        /// Time since the last update
        /// </summary>
        public static TimeSpan LastUpdated => (DateTime.Now - _lastUpdated);

        /// <summary>
        /// Print statistics for a specific message provider as a JSON string.
        /// </summary>
        /// <param name="messageProviderId"></param>
        /// <returns></returns>
        public static string ToJson(Guid messageProviderId) => JsonConvert.SerializeObject(new
        {
            Trend = _trends[messageProviderId]
        });
        
        
        /// <summary>
        /// Print all tracked statistics as a JSON string
        /// </summary>
        /// <returns></returns>
        public static string ToJson() => JsonConvert.SerializeObject(new
        {
            Trends = _trends
        });
    }
}
