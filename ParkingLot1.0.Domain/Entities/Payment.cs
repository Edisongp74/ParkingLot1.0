using ParkingLot1._0.Domain.Common.Enums;

namespace ParkingLot1._0.Domain.Entities
{
    public class Payment
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;

        public int PaymentMethodId { get; set; }
        public PaymentMethod PaymentMethod { get; set; } = null!;

        public PaymentType PaymentType { get; set; }
        public PaymentStatus Status { get; set; }

        public decimal Amount { get; set; }
        public string? Reference { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? PaidAt { get; set; }

        public bool IsMonthlyPayment { get; set; }

        public int? MonthlyMembershipId { get; set; }

        
    }
}