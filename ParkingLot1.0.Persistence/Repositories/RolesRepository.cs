using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ParkingLot1._0.Application.Contracts.Repositories;
using ParkingLot1._0.Application.UseCases.Roles.Queries.GetRoleById;
using ParkingLot1._0.Application.UseCases.Roles.Queries.GetRolesList;
using ParkingLot1._0.Persistence.Identity;

namespace ParkingLot1._0.Persistence.Repositories;

public class RolesRepository : IRolesRepository
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public RolesRepository(
        RoleManager<ApplicationRole> roleManager,
        UserManager<ApplicationUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task<List<RoleListItemDTO>> GetRolesListAsync()
    {
        return await _roleManager.Roles
            .Select(r => new RoleListItemDTO
            {
                Id = r.Id,
                Name = r.Name ?? string.Empty
            })
            .ToListAsync();
    }

    public async Task<RoleDetailDTO> GetRoleByIdAsync(string id)
    {
        ApplicationRole role = await _roleManager.FindByIdAsync(id)
            ?? throw new Exception("Rol no encontrado.");

        return new RoleDetailDTO
        {
            Id = role.Id,
            Name = role.Name ?? string.Empty
        };
    }

    public async Task CreateAsync(string name)
    {
        bool exists = await _roleManager.RoleExistsAsync(name);
        if (exists)
            throw new Exception("Ya existe un rol con ese nombre.");

        IdentityResult result = await _roleManager.CreateAsync(new ApplicationRole(name));

        if (!result.Succeeded)
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
    }

    public async Task UpdateAsync(string id, string name)
    {
        ApplicationRole role = await _roleManager.FindByIdAsync(id)
            ?? throw new Exception("Rol no encontrado.");

        role.Name = name;

        IdentityResult result = await _roleManager.UpdateAsync(role);

        if (!result.Succeeded)
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
    }

    public async Task DeleteAsync(string id)
    {
        ApplicationRole role = await _roleManager.FindByIdAsync(id)
            ?? throw new Exception("Rol no encontrado.");

        if (role.Name == "Administrador")
            throw new Exception("No se puede eliminar el rol Administrador.");

        List<ApplicationUser> usersInRole = [.. await _userManager.GetUsersInRoleAsync(role.Name!)];

        foreach (ApplicationUser user in usersInRole)
        {
            IdentityResult removeResult = await _userManager.RemoveFromRoleAsync(user, role.Name!);

            if (!removeResult.Succeeded)
                throw new Exception(string.Join(", ", removeResult.Errors.Select(e => e.Description)));
        }

        IdentityResult result = await _roleManager.DeleteAsync(role);

        if (!result.Succeeded)
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
    }
}