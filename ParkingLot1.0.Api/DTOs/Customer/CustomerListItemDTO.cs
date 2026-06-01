namespace ParkingLot1._0.Api.DTOs.Customer
{
    public class CustomerListItemDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string DocumentNumber { get; set; } = string.Empty;
        public string DocumentType { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string CustomerType { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
    }
}
