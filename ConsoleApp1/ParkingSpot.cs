using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    public class ParkingSpot
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Status { get; set; } = "Available"; // Available, Occupied, UnderMaintenance

        // Navigation Properties
        public virtual ICollection<ParkingRecord> ParkingRecords { get; set; } = new List<ParkingRecord>();
    }
}
