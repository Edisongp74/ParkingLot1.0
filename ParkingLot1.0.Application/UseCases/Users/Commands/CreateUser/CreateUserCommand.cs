
namespace ParkingLot1._0.Application.UseCases.Users.Commands.CreateUser;

public class CreateUserCommand
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
}