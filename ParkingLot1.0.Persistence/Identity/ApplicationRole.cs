using Microsoft.AspNetCore.Identity;

namespace ParkingLot1._0.Persistence.Identity;

public class ApplicationRole : IdentityRole
{
    public ApplicationRole() { }

    public ApplicationRole(string roleName) : base(roleName) { }
}