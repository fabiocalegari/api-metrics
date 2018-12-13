using Microsoft.EntityFrameworkCore;
using APIMetrics22.Models;

namespace APIMetrics22
{
    public class APIMetricsContext : DbContext
    {

        public APIMetricsContext(DbContextOptions<APIMetricsContext> options) : 
            base(options)
            { 
            }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        public DbSet<Issue> Issue { get; set; }
        public DbSet<Transition> Transition { get; set; }
    }
}
