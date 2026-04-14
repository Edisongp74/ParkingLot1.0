using System;
using System.Collections.Generic;
using System.Text;

namespace ParkingLot1._0.Domain.Entities
{    public class Payment
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty; 
        public DateTime PaymentDate { get; set; }

  
        public int? ParkingRecordId { get; set; }
        public virtual ParkingRecord? ParkingRecord { get; set; }

        public int? MonthlyPassId { get; set; }
        public virtual MonthlyPass? MonthlyPass { get; set; }
    }
}
