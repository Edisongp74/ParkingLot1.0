using System;
using System.Collections.Generic;
using System.Text;

namespace ParkingLot1._0.Domain.Entities
{
    public class ParkingRecord
    {
        public int Id { get; set; }
        public DateTime EntryTime { get; set; }
        public DateTime? ExitTime { get; set; }

        // Foreign Keys
        public int VehicleId { get; set; }
        public virtual Vehicle Vehicle { get; set; } = null!;

        public int ParkingSpotId { get; set; }
        public virtual ParkingSpot ParkingSpot { get; set; } = null!;

        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; } = null!;

        public int? RateId { get; set; }
        public virtual Rate? Rate { get; set; }

        
        public virtual Payment? Payment { get; set; }
    }
}
