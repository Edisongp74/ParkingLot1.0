using System;
using System.Collections.Generic;
using System.Text;

namespace ParkingLot1._0.Domain.Entities
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string LicensePlate { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;

     
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; } = null!;

        
        public virtual ICollection<ParkingRecord> ParkingRecords { get; set; } = new List<ParkingRecord>();
    }
}
