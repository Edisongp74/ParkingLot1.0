using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    public class Rate
    {
        public int Id { get; set; }
        public string VehicleType { get; set; } = string.Empty; // Car, Motorcycle
        public string Modality { get; set; } = string.Empty;    // Hour, Day, Month
        public decimal Value { get; set; }
    }
}
