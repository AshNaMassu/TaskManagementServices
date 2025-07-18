using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Database.Mapping;

namespace Persistence.Database
{
    public class DataBaseContext : DbContext
    {
        public DbSet<ActivityLog> ActivityLogs { get; set; }

        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ActivityLogMapping());
        }
    }
}
