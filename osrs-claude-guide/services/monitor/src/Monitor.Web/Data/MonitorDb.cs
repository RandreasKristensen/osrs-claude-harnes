using Microsoft.EntityFrameworkCore;

namespace Monitor.Web.Data
{
    public class MonitorDb : DbContext
    {
        public DbSet<Check> Checks { get; set; }

        public MonitorDb(DbContextOptions<MonitorDb> options) : base(options)
        {
        }
    }

    public class Check
    {
        public int Id { get; set; }
        public string? ServiceName { get; set; }
        public string? Url { get; set; }
        public DateTime CheckedAt { get; set; }
        public int StatusCode { get; set; }
        public long ResponseMs { get; set; }
        public bool IsHealthy { get; set; }
    }
}
