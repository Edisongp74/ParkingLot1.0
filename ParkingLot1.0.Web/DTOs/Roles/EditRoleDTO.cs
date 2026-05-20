using System.ComponentModel.DataAnnotations;

namespace ParkingLot1._0.Web.DTOs.Roles;

public class EditRoleDTO
{
    public string Id { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre del rol es requerido.")]
    public string Name { get; set; } = string.Empty;
}