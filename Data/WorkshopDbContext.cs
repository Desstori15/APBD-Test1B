
using Microsoft.EntityFrameworkCore;
using WorkshopVisitApi.Models;

namespace WorkshopVisitApi.Data
{
    public class WorkshopDbContext : DbContext
    {
        public WorkshopDbContext(DbContextOptions<WorkshopDbContext> options) : base(options) { }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Mechanic> Mechanics { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Visit> Visits { get; set; }
        public DbSet<VisitService> VisitServices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VisitService>().HasKey(x => new { x.ServiceId, x.VisitId });
        }
    }
}
