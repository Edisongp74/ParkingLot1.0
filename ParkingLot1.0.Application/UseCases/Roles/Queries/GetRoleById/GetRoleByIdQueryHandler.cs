using ParkingLot1._0.Application.Contracts.Repositories;

namespace ParkingLot1._0.Application.UseCases.Roles.Queries.GetRoleById;

public class GetRoleByIdQueryHandler
{
    private readonly IRolesRepository _rolesRepository;

    public GetRoleByIdQueryHandler(IRolesRepository rolesRepository)
    {
        _rolesRepository = rolesRepository;
    }

    public async Task<RoleDetailDTO> HandleAsync(GetRoleByIdQuery query)
    {
        return await _rolesRepository.GetRoleByIdAsync(query.Id);
    }
}