namespace ParkingLot1._0.Web.Models
{
    public class CustomerViewModel
    {
        public int Id { get; set; }
        public string DocumentNumber { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public int TotalVehicles { get; set; }
        public bool HasActivePass { get; set; }
    }
}