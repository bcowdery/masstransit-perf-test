using System;

namespace PerfTest.Consumer.Data.Entities
{
    public class Operation
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public int ThreadId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double Duration { get; set; }

        /// <summary>
        /// Create a new operation
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="threadId"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static Operation CreateNew(int taskId, int threadId, DateTime start, DateTime end) => new Operation()
        {
            TaskId = taskId,
            ThreadId = threadId,
            StartTime = start,
            EndTime = end,
            Duration = (end - start).TotalMilliseconds
        };
    }
}
