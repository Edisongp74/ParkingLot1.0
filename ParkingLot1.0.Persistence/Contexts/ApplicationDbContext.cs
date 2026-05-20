using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ParkingLot1._0.Domain.Entities;
using System.Linq;

namespace ParkingLot1._0.Persistence.Contexts
{
    public class ApplicationDbContext
        : IdentityDbContext<ParkingLot1._0.Persistence.Identity.ApplicationUser, ParkingLot1._0.Persistence.Identity.ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<ParkingSpot> ParkingSpots { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<ParkingRecord> ParkingRecords { get; set; }
        public DbSet<Rate> Rates { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<MonthlyPass> MonthlyPasses { get; set; }

        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;
            }

            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Rate>()
                .Property(r => r.Value)
                .HasColumnType("decimal(18,2)");
        }
    }
}