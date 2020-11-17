using System;

namespace PerfTest.Consumer.Settings
{
    public class ConsumerOptions
    {
        /// <summary>
        /// Config section key
        /// </summary>
        public const string Consumer = "Consumer";
        
        /// <summary>
        /// Report path to write JSON results to
        /// </summary>
        public string ReportPath { get; set; }
        
        /// <summary>
        /// Period of time to wait before determining all activity has stopped.
        /// </summary>
        public TimeSpan InactivityTimeout { get; set; }
    }
}
