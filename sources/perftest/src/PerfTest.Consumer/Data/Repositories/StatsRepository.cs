using System.Threading.Tasks;
using Dapper;
using PerfTest.Consumer.Data.Entities;

namespace PerfTest.Consumer.Data.Repositories
{
    public class StatsRepository
        : IStatsRepository
    {
        private readonly IStatsDbContext _dbContext;

        public StatsRepository(IStatsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<int> InsertOperation(Operation operation)
        {
            using var connection = _dbContext.CreateConnection();
            return connection.ExecuteAsync(
                @"INSERT (TaskId, ThreadId, StartTime, EndTime, Durration) VALUES (@taskId, @threadId, @start, @end, @duration)",
                new[]
                {
                    new
                    {
                        taskId = operation.TaskId,
                        threadId = operation.ThreadId,
                        start = operation.StartTime,
                        end = operation.EndTime,
                        duration = operation.Duration
                    }
                });
        }
    }

    public interface IStatsRepository
    {
        Task<int> InsertOperation(Operation operation);
    }
}
