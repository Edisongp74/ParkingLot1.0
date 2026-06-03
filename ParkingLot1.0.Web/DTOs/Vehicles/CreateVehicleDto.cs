using System.ComponentModel.DataAnnotations;

namespace ParkingLot1._0.Web.DTOs.Vehicles
{
    public class CreateVehicleDto
    {
        [Required(ErrorMessage = "La placa es obligatoria")]
        [MinLength(5, ErrorMessage = "La placa debe tener al menos 5 caracteres")]
        [StringLength(10, ErrorMessage = "La placa no puede exceder 10 caracteres")]
        [Display(Name = "Placa")]
        public string LicensePlate { get; set; } = string.Empty;

        [Required(ErrorMessage = "El tipo de vehiculo es obligatorio")]
        [Display(Name = "Tipo de Vehiculo")]
        public string Type { get; set; } = string.Empty;

        [StringLength(30, ErrorMessage = "La marca no puede exceder 30 caracteres")]
        [Display(Name = "Marca")]
        public string Brand { get; set; } = string.Empty;

        [StringLength(20, ErrorMessage = "El color no puede exceder 20 caracteres")]
        [Display(Name = "Color")]
        public string Color { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe seleccionar un cliente")]
        public int CustomerId { get; set; }

    }
}