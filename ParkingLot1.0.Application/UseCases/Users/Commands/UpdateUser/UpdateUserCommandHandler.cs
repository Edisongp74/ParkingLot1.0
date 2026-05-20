using ParkingLot1._0.Application.Contracts.Repositories;

namespace ParkingLot1._0.Application.UseCases.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler
{
    private readonly IUsersRepository _usersRepository;

    public UpdateUserCommandHandler(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    public async Task HandleAsync(UpdateUserCommand command)
    {
        await _usersRepository.UpdateAsync(
            command.Id,
            command.FirstName,
            command.LastName,
            command.Email,
            command.PhoneNumber,
            command.RoleName);
    }
}