using Microsoft.EntityFrameworkCore;
using ParkingLot1._0.Domain.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ParkingLot1._0.Persistence.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<ParkingSpot> ParkingSpots { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<ParkingRecord> ParkingRecords { get; set; }
        public DbSet<Rate> Rates { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<MonthlyPass> MonthlyPasses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Esto rompe el ciclo de borrado que causa el error rojo
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;
            }

            // Aprovechamos para quitar los mensajes amarillos de los decimales
            modelBuilder.Entity<Payment>().Property(p => p.Amount).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Rate>().Property(r => r.Value).HasColumnType("decimal(18,2)");
        }
    }
}