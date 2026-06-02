using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ParkingLot1._0.Domain.Entities;
using ParkingLot1._0.Persistence.Contexts;
using ParkingLot1._0.Persistence.Identity;

namespace ParkingLot1._0.Persistence.Seeding
{
    public class SeedDb
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public SeedDb(
            RoleManager<ApplicationRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }

        public async Task SeedAsync()
        {
            await SeedRolesAsync();
            await SeedAdminUserAsync();
            await SeedPaymentMethodsAsync();
        }

        private async Task SeedRolesAsync()
        {
            string[] roles = ["Administrador", "Operador", "Cliente"];

            foreach (string role in roles)
            {
                bool exists = await _roleManager.RoleExistsAsync(role);
                if (!exists)
                {
                    await _roleManager.CreateAsync(new ApplicationRole(role));
                }
            }
        }

        private async Task SeedAdminUserAsync()
        {
            const string email = "admin@parkinglot.com";
            const string password = "Admin123!";

            ApplicationUser? user = await _userManager.FindByEmailAsync(email);

            if (user is null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    FirstName = "Admin",
                    LastName = "Sistema",
                    EmailConfirmed = true
                };

                IdentityResult result = await _userManager.CreateAsync(user, password);

                if (!result.Succeeded)
                {
                    throw new Exception("No se pudo crear el usuario admin: " +
                        string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }

            bool isInRole = await _userManager.IsInRoleAsync(user, "Administrador");
            if (!isInRole)
            {
                await _userManager.AddToRoleAsync(user, "Administrador");
            }
        }

        private async Task SeedPaymentMethodsAsync()
        {
            if (!await _context.PaymentMethods.AnyAsync())
            {
                _context.PaymentMethods.AddRange(
                    new PaymentMethod
                    {
                        Name = "Efectivo",
                        Description = "Pago en efectivo",
                        IsActive = true
                    },
                    new PaymentMethod
                    {
                        Name = "Transferencia",
                        Description = "Pago por transferencia bancaria",
                        IsActive = true
                    },
                    new PaymentMethod
                    {
                        Name = "Tarjeta",
                        Description = "Pago con tarjeta",
                        IsActive = true
                    }
                );

                await _context.SaveChangesAsync();
            }
        }
    }
}