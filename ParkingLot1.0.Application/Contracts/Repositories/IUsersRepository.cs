using ParkingLot1._0.Application.UseCases.Users.Queries.GetRoleOptions;
using ParkingLot1._0.Application.UseCases.Users.Queries.GetUserById;
using ParkingLot1._0.Application.UseCases.Users.Queries.GetUsersList;

namespace ParkingLot1._0.Application.Contracts.Repositories;

public interface IUsersRepository
{
    Task<List<UserListItemDTO>> GetUsersListAsync();
    Task<UserDetailDTO> GetUserByIdAsync(string id);
    Task<List<RoleOptionDTO>> GetRoleOptionsAsync();
    Task CreateAsync(string firstName, string lastName, string email, string phoneNumber, string roleName);
    Task UpdateAsync(string id, string firstName, string lastName, string email, string phoneNumber, string roleName);
    Task DeleteAsync(string id);
}