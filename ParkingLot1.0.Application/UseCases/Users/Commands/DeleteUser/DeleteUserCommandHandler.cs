using ParkingLot1._0.Application.Contracts.Repositories;

namespace ParkingLot1._0.Application.UseCases.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler
{
    private readonly IUsersRepository _usersRepository;

    public DeleteUserCommandHandler(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    public async Task HandleAsync(DeleteUserCommand command)
    {
        await _usersRepository.DeleteAsync(command.Id);
    }
}