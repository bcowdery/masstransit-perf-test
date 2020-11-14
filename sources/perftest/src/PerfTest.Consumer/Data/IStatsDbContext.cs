using System.Data.Common;
using System.Threading.Tasks;

namespace PerfTest.Consumer.Data
{
    public interface IStatsDbContext
    {
        DbConnection CreateConnection();
        Task EnsureCreated();
    }
}
