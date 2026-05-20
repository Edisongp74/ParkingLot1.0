using Microsoft.AspNetCore.Identity;

using ParkingLot1._0.Persistence.Identity;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}