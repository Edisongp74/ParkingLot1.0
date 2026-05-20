using ParkingLot1._0.Application.UseCases.Roles.Queries.GetRoleById;
using ParkingLot1._0.Application.UseCases.Roles.Queries.GetRolesList;

namespace ParkingLot1._0.Application.Contracts.Repositories;

public interface IRolesRepository
{
    Task<List<RoleListItemDTO>> GetRolesListAsync();
    Task<RoleDetailDTO> GetRoleByIdAsync(string id);
    Task CreateAsync(string name);
    Task UpdateAsync(string id, string name);
    Task DeleteAsync(string id);
}