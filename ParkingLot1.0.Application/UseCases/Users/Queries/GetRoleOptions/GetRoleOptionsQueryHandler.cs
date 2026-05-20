
using ParkingLot1._0.Application.Contracts.Repositories;

namespace ParkingLot1._0.Application.UseCases.Users.Queries.GetRoleOptions;

public class GetRoleOptionsQueryHandler
{
    private readonly IUsersRepository _usersRepository;

    public GetRoleOptionsQueryHandler(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    public async Task<List<RoleOptionDTO>> HandleAsync(GetRoleOptionsQuery query)
    {
        return await _usersRepository.GetRoleOptionsAsync();
    }
}