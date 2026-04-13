using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string LicensePlate { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;

        // Foreign Key
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; } = null!;

        // Navigation Properties
        public virtual ICollection<ParkingRecord> ParkingRecords { get; set; } = new List<ParkingRecord>();
    }
}
