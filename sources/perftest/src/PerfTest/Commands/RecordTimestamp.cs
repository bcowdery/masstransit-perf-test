using System;

namespace PerfTest.Commands
{
    public interface RecordTimestamp
    {
        int TaskId { get; }
        int ThreadId { get; }
        Guid ProducerId { get; }
        DateTime SentTime { get; } 
    }
}
