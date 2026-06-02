namespace ParkingLot1._0.Web.Models
{
    public class MonthlyPassStatusViewModel
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string VehiclePlate { get; set; } = string.Empty;
        public string VehicleBrand { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string RateInfo { get; set; } = string.Empty;
    }
}