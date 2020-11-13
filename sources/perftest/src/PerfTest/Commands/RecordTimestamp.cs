using System;

namespace PerfTest.Commands
{
    public interface RecordTimestamp
    {
        int TaskId { get; }
        int ThreadId { get; }
        int ExecutionCount { get; set; }
        DateTimeOffset SentTime { get; } 
    }
}