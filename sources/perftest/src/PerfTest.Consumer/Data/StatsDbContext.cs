using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using PerfTest.Consumer.Data.Settings;

namespace PerfTest.Consumer.Data
{
    /// <summary>
    /// Stats database DB context for SQLite
    /// </summary>
    public class StatsDbContext
        : IStatsDbContext
    {
        private readonly StatsDbOptions _dbOptions;
        private readonly ILogger<StatsDbContext> _logger;

        public StatsDbContext(StatsDbOptions dbOptions, ILogger<StatsDbContext> logger)
        {
            _dbOptions = dbOptions;
            _logger = logger;
        }

        /// <summary>
        /// Create a new Sqlite DbConnection.
        /// </summary>
        /// <returns></returns>
        public DbConnection CreateConnection()
            => new SqliteConnection(_dbOptions.ConnectionString);
    
        /// <summary>
        /// Creates the application database schema.
        /// </summary>
        /// <returns></returns>
        public async Task EnsureCreated()
        {
            _logger.LogInformation("Creating SQLite database ...");
            
            await using var connection = CreateConnection();
            await connection.OpenAsync();
            
            var command = connection.CreateCommand();
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Operations
                (
                    ID integer primary key AUTOINCREMENT,
                    TaskId integer not null,
                    ThreadId integer not null,
                    StartTime datetime not null,
                    EndTime datetime not null,
                    Duration integer not null
                );
            ";
            
            await command.ExecuteNonQueryAsync();
        }
    }
}
