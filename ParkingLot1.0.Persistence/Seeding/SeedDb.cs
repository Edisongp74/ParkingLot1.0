using Microsoft.AspNetCore.Identity;
using ParkingLot1._0.Persistence.Identity;

namespace ParkingLot1._0.Persistence.Seeding;

public class SeedDb
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public SeedDb(
        RoleManager<ApplicationRole> roleManager,
        UserManager<ApplicationUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task SeedAsync()
    {
        await SeedRolesAsync();
        await SeedAdminUserAsync();
    }

    private async Task SeedRolesAsync()
    {
        string[] roles = ["Administrador", "Operador"];

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
}