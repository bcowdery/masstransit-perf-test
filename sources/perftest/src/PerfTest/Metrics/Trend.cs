using System;
using Newtonsoft.Json;

namespace PerfTest.Metrics
{
    public class Trend
    {
        private readonly object _syncLock = new object();
        private readonly string _trendId;
        
        private double _min = double.MaxValue;
        private double _max = 0.00d;
        private double _avg = 0.00d;
        private int _totalItems = 0;
        private double _totalDuration = 0.00d;

        private DateTime _lastUpdated = DateTime.UtcNow;

        public Trend(Guid guid)
        {
            _trendId = guid.ToString();
        }
        
        public Trend(string trendId)
        {
            _trendId = trendId;
        }
        
        [JsonProperty] public double Min => Math.Round(_min, 2);
        [JsonProperty] public double Max => Math.Round(_max, 2);
        [JsonProperty] public double Avg => Math.Round(_avg, 2);
        [JsonProperty] public int TotalItems => _totalItems;
        [JsonProperty] public double TotalDuration => Math.Round(_totalDuration, 2);
        [JsonProperty] public string Throughput => $"{Math.Round((_totalItems / _totalDuration) * 1000, 2)}/sec";
       
        /// <summary>
        /// Time since the last update
        /// </summary>
        [JsonIgnore] public TimeSpan LastUpdated => (DateTime.UtcNow - _lastUpdated);

        /// <summary>
        /// Add a new data point
        /// </summary>
        /// <param name="duration"></param>
        public void Add(TimeSpan duration)
        {
            Add(duration.TotalMilliseconds);
        }

        /// <summary>
        /// Add a new data point
        /// </summary>
        /// <param name="duration"></param>
        public void Add(double duration)
        {
            lock (_syncLock)
            {
                if (duration < _min) _min = duration;
                if (duration > _max) _max = duration;

                _totalItems++;
                _totalDuration += duration;
                _avg = (_totalDuration / _totalItems);
                _lastUpdated = DateTime.UtcNow;
            }
        }
    }
}
