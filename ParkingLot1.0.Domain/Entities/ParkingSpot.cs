using System;
using System.Collections.Generic;
using System.Text;

namespace ParkingLot1._0.Domain.Entities
{    public class ParkingSpot
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Status { get; set; } = "Available"; 

     
        public virtual ICollection<ParkingRecord> ParkingRecords { get; set; } = new List<ParkingRecord>();
    }
}
