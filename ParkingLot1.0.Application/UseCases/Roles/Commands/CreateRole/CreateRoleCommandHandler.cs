using ParkingLot1._0.Application.Contracts.Repositories;

namespace ParkingLot1._0.Application.UseCases.Roles.Commands.CreateRole;

public class CreateRoleCommandHandler
{
    private readonly IRolesRepository _rolesRepository;

    public CreateRoleCommandHandler(IRolesRepository rolesRepository)
    {
        _rolesRepository = rolesRepository;
    }

    public async Task HandleAsync(CreateRoleCommand command)
    {
        await _rolesRepository.CreateAsync(command.Name);
    }
}