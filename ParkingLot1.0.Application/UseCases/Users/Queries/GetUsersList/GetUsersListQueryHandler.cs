using ParkingLot1._0.Application.Contracts.Repositories;

namespace ParkingLot1._0.Application.UseCases.Users.Queries.GetUsersList;

public class GetUsersListQueryHandler
{
    private readonly IUsersRepository _usersRepository;

    public GetUsersListQueryHandler(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    public async Task<List<UserListItemDTO>> HandleAsync(GetUsersListQuery query)
    {
        return await _usersRepository.GetUsersListAsync();
    }
}