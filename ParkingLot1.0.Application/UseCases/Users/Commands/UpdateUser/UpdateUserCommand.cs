
namespace ParkingLot1._0.Application.UseCases.Users.Commands.UpdateUser;

public class UpdateUserCommand
{
    public string Id { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
}
