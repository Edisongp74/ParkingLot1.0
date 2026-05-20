using System.ComponentModel.DataAnnotations;

namespace ParkingLot1._0.Web.DTOs.Roles;

public class CreateRoleDTO
{
    [Required(ErrorMessage = "El nombre del rol es requerido.")]
    public string Name { get; set; } = string.Empty;
}