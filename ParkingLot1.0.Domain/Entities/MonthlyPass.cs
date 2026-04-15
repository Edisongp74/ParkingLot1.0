using System;
using System.Collections.Generic;
using System.Text;

namespace ParkingLot1._0.Domain.Entities
{
    public class MonthlyPass
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = "Active";

        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; } = null!;

        public int VehicleId { get; set; }
        public virtual Vehicle Vehicle { get; set; } = null!;

        public int RateId { get; set; }
        public virtual Rate Rate { get; set; } = null!;
    }
}
