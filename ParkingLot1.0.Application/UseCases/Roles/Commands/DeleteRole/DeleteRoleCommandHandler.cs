using ParkingLot1._0.Application.Contracts.Repositories;

namespace ParkingLot1._0.Application.UseCases.Roles.Commands.DeleteRole;

public class DeleteRoleCommandHandler
{
    private readonly IRolesRepository _rolesRepository;

    public DeleteRoleCommandHandler(IRolesRepository rolesRepository)
    {
        _rolesRepository = rolesRepository;
    }

    public async Task HandleAsync(DeleteRoleCommand command)
    {
        await _rolesRepository.DeleteAsync(command.Id);
    }
}