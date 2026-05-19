using System.ComponentModel.DataAnnotations;

namespace ParkingLot1._0.Web.Models
{
    public class BuyMonthlyPassViewModel
    {
        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un vehículo")]
        public int VehicleId { get; set; }

        [Required]
        public DateTime StartDate { get; set; } = DateTime.Now;

        public List<VehicleViewModel> Vehicles { get; set; } = new();
    }
}