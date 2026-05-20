using System.ComponentModel.DataAnnotations;

namespace ParkingLot1._0.Web.DTOs.Customers
{
    // DTO para crear un cliente con Data Annotations para validacion del lado del cliente
    public class CreateCustomerDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MinLength(2, ErrorMessage = "El nombre debe tener al menos 2 caracteres")]
        [StringLength(50, ErrorMessage = "El nombre no puede exceder 50 caracteres")]
        [Display(Name = "Nombre")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [MinLength(2, ErrorMessage = "El apellido debe tener al menos 2 caracteres")]
        [StringLength(50, ErrorMessage = "El apellido no puede exceder 50 caracteres")]
        [Display(Name = "Apellido")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "El numero de documento es obligatorio")]
        [MinLength(5, ErrorMessage = "El documento debe tener al menos 5 caracteres")]
        [StringLength(20, ErrorMessage = "El documento no puede exceder 20 caracteres")]
        [Display(Name = "Numero de Documento")]
        public string DocumentNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "El tipo de documento es obligatorio")]
        [Display(Name = "Tipo de Documento")]
        public string DocumentType { get; set; } = string.Empty;

        [Required(ErrorMessage = "El telefono es obligatorio")]
        [StringLength(15, ErrorMessage = "El telefono no puede exceder 15 caracteres")]
        [Display(Name = "Telefono")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "El tipo de cliente es obligatorio")]
        [Display(Name = "Tipo de Cliente")]
        public string CustomerType { get; set; } = string.Empty;
    }
}
