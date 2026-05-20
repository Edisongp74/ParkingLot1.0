using ParkingLot1._0.Application.Contracts.Repositories;

namespace ParkingLot1._0.Application.UseCases.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler
{
    private readonly IUsersRepository _usersRepository;

    public GetUserByIdQueryHandler(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    public async Task<UserDetailDTO> HandleAsync(GetUserByIdQuery query)
    {
        return await _usersRepository.GetUserByIdAsync(query.Id);
    }
}