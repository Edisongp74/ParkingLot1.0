using ParkingLot1._0.Application.Contracts.Repositories;

namespace ParkingLot1._0.Application.UseCases.Roles.Commands.UpdateRole;

public class UpdateRoleCommandHandler
{
    private readonly IRolesRepository _rolesRepository;

    public UpdateRoleCommandHandler(IRolesRepository rolesRepository)
    {
        _rolesRepository = rolesRepository;
    }

    public async Task HandleAsync(UpdateRoleCommand command)
    {
        await _rolesRepository.UpdateAsync(command.Id, command.Name);
    }
}
