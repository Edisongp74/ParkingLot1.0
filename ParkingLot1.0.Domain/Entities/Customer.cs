namespace ParkingLot1._0.Domain.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string DocumentNumber { get; set; } = string.Empty;
        public string DocumentType { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string CustomerType { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}";

        public bool HasActiveMonthlyPass()
        {
            return MonthlyPasses != null && MonthlyPasses.Any(p => p.EndDate >= DateTime.Now && p.Status == "Active");
        }
        public bool CanAddVehicle() => Vehicles.Count < 3;

        public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
        public virtual ICollection<MonthlyPass> MonthlyPasses { get; set; } = new List<MonthlyPass>();
    }
}
