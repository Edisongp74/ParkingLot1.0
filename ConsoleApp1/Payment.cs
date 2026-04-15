using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{    public class Payment
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty; // Cash, CreditCard, Transfer
        public DateTime PaymentDate { get; set; }

        // Links to ParkingRecord OR MonthlyPass
        public int? ParkingRecordId { get; set; }
        public virtual ParkingRecord? ParkingRecord { get; set; }

        public int? MonthlyPassId { get; set; }
        public virtual MonthlyPass? MonthlyPass { get; set; }
    }
}
