using System.ComponentModel.DataAnnotations;

namespace ParkingLot1._0.Api.DTOs.Customer
{
    public class CreateCustomerDTO
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public required string FirstName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public required string LastName { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 5)]
        public required string DocumentNumber { get; set; }

        [Required]
        [StringLength(20)]
        public required string DocumentType { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 7)]
        public required string Phone { get; set; }

        [Required]
        [StringLength(20)]
        public required string CustomerType { get; set; }
    }
}
