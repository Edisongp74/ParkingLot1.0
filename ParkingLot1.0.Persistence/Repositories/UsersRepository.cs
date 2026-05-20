using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ParkingLot1._0.Application.Contracts.Repositories;
using ParkingLot1._0.Application.UseCases.Users.Queries.GetRoleOptions;
using ParkingLot1._0.Application.UseCases.Users.Queries.GetUserById;
using ParkingLot1._0.Application.UseCases.Users.Queries.GetUsersList;
using ParkingLot1._0.Persistence.Identity;

namespace ParkingLot1._0.Persistence.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public UsersRepository(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<List<UserListItemDTO>> GetUsersListAsync()
    {
        List<ApplicationUser> users = await _userManager.Users.ToListAsync();
        List<UserListItemDTO> result = [];

        foreach (ApplicationUser user in users)
        {
            IList<string> roles = await _userManager.GetRolesAsync(user);

            result.Add(new UserListItemDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email ?? string.Empty,
                RoleName = roles.FirstOrDefault() ?? "Sin rol"
            });
        }

        return result;
    }

    public async Task<UserDetailDTO> GetUserByIdAsync(string id)
    {
        ApplicationUser user = await _userManager.FindByIdAsync(id)
            ?? throw new Exception("Usuario no encontrado.");

        IList<string> roles = await _userManager.GetRolesAsync(user);

        return new UserDetailDTO
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email ?? string.Empty,
            PhoneNumber = user.PhoneNumber ?? string.Empty,
            RoleName = roles.FirstOrDefault() ?? string.Empty
        };
    }

    public async Task<List<RoleOptionDTO>> GetRoleOptionsAsync()
    {
        return await _roleManager.Roles
            .Select(r => new RoleOptionDTO
            {
                Id = r.Id,
                Name = r.Name ?? string.Empty
            })
            .ToListAsync();
    }

    public async Task CreateAsync(string firstName, string lastName, string email, string phoneNumber, string roleName)
    {
        ApplicationUser user = new()
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            UserName = email,
            PhoneNumber = phoneNumber
        };

        IdentityResult result = await _userManager.CreateAsync(user, "Parking123!");

        if (!result.Succeeded)
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

        if (!string.IsNullOrWhiteSpace(roleName))
            await _userManager.AddToRoleAsync(user, roleName);
    }

    public async Task UpdateAsync(string id, string firstName, string lastName, string email, string phoneNumber, string roleName)
    {
        ApplicationUser user = await _userManager.FindByIdAsync(id)
            ?? throw new Exception("Usuario no encontrado.");

        user.FirstName = firstName;
        user.LastName = lastName;
        user.Email = email;
        user.UserName = email;
        user.PhoneNumber = phoneNumber;

        IdentityResult updateResult = await _userManager.UpdateAsync(user);

        if (!updateResult.Succeeded)
            throw new Exception(string.Join(", ", updateResult.Errors.Select(e => e.Description)));

        IList<string> currentRoles = await _userManager.GetRolesAsync(user);
        if (currentRoles.Any())
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

        if (!string.IsNullOrWhiteSpace(roleName))
            await _userManager.AddToRoleAsync(user, roleName);
    }

    public async Task DeleteAsync(string id)
    {
        ApplicationUser user = await _userManager.FindByIdAsync(id)
            ?? throw new Exception("Usuario no encontrado.");

        if (user.Email == "admin@parkinglot.com")
            throw new Exception("No se puede eliminar el usuario administrador principal.");

        IList<string> roles = await _userManager.GetRolesAsync(user);
        if (roles.Any())
        {
            IdentityResult removeRolesResult = await _userManager.RemoveFromRolesAsync(user, roles);

            if (!removeRolesResult.Succeeded)
                throw new Exception(string.Join(", ", removeRolesResult.Errors.Select(e => e.Description)));
        }

        IdentityResult result = await _userManager.DeleteAsync(user);

        if (!result.Succeeded)
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
    }
}
