using ParkingLot1._0.Domain.Entities;
using ParkingLot1._0.Persistence.Contexts;

namespace ParkingLot1._0.Persistence.Seeding
{
    public static class PaymentMethodSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext _context)
        {
            if (!_context.PaymentMethods.Any())
            {
                _context.PaymentMethods.AddRange(
                    new PaymentMethod { Name = "Efectivo", Description = "Pago en efectivo", IsActive = true },
                    new PaymentMethod { Name = "Transferencia", Description = "Pago por transferencia bancaria", IsActive = true },
                    new PaymentMethod { Name = "Tarjeta", Description = "Pago con tarjeta", IsActive = true }
                );

                await _context.SaveChangesAsync();
            }
        }
    }
}