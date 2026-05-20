using ParkingLot1._0.Application.Contracts.Repositories;

namespace ParkingLot1._0.Application.UseCases.Users.Commands.CreateUser;

public class CreateUserCommandHandler
{
    private readonly IUsersRepository _usersRepository;

    public CreateUserCommandHandler(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    public async Task HandleAsync(CreateUserCommand command)
    {
        await _usersRepository.CreateAsync(
            command.FirstName,
            command.LastName,
            command.Email,
            command.PhoneNumber,
            command.RoleName);
    }
}