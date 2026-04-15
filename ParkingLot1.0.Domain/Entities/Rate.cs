using System;
using System.Collections.Generic;
using System.Text;

namespace ParkingLot1._0.Domain.Entities
{
    public class Rate
    {
        public int Id { get; set; }
        public string VehicleType { get; set; } = string.Empty; 
        public string Modality { get; set; } = string.Empty;    
        public decimal Value { get; set; }
    }
}
