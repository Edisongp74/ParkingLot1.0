using System.ComponentModel.DataAnnotations;
using ParkingLot1._0.Domain.Common.Enums;

namespace ParkingLot1._0.Application.DTOs.Payments
{
    public class CreatePaymentDto
    {
        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int PaymentMethodId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public PaymentType PaymentType { get; set; }

        public int? MonthlyMembershipId { get; set; }

        public string? Reference { get; set; }

        
    }
}