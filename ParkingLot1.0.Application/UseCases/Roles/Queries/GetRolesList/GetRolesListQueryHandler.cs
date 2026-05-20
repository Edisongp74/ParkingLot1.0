using ParkingLot1._0.Application.Contracts.Repositories;

namespace ParkingLot1._0.Application.UseCases.Roles.Queries.GetRolesList;

public class GetRolesListQueryHandler
{
    private readonly IRolesRepository _rolesRepository;

    public GetRolesListQueryHandler(IRolesRepository rolesRepository)
    {
        _rolesRepository = rolesRepository;
    }

    public async Task<List<RoleListItemDTO>> HandleAsync(GetRolesListQuery query)
    {
        return await _rolesRepository.GetRolesListAsync();
    }
}