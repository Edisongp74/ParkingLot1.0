using System.ComponentModel.DataAnnotations;

namespace ParkingLot1._0.Web.DTOs.Account;

public class LoginDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; }
}
