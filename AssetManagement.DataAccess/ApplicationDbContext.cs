using AssetManagement.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace AssetManagement.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<AssetAssignmentHistory> AssetAssignmentHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure unique constraint for SerialNumber on Asset
            modelBuilder.Entity<Asset>()
                .HasIndex(a => a.SerialNumber)
                .IsUnique();
        }
    }
}