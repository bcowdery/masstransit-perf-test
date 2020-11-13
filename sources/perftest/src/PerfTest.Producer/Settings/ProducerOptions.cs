namespace PerfTest.Producer.Settings
{
    public class ProducerOptions
    {
        /// <summary>
        /// Config section key
        /// </summary>
        public const string Producer = "Producer";
        
        /// <summary>
        /// Messages to send with every iteration.
        /// </summary>
        public int MaxSendConcurrency { get; set; }
        
        /// <summary>
        /// Total number of messages to send before halting.
        /// </summary>
        public int MaxSendCount { get; set; }
        
        /// <summary>
        /// Interval of time between each iteration in milliseconds.
        /// If zero, service will send as fast as possible only waiting for task completion of the current batch.
        /// </summary>
        public int Interval { get; set; }
    }
}
