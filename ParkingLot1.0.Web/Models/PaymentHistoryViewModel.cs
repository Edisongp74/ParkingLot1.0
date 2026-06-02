using ParkingLot1._0.Domain.Common.Enums;

namespace ParkingLot1._0.Web.Models
{
    public class PaymentHistoryViewModel
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public PaymentType PaymentType { get; set; }
        public PaymentStatus Status { get; set; }
        public string? Reference { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? PaidAt { get; set; }
    }
}