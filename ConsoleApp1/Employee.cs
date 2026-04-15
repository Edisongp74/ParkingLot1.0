using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string DocumentNumber { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty; // Admin, Operator

        // Navigation Properties
        public virtual ICollection<ParkingRecord> ParkingRecords { get; set; } = new List<ParkingRecord>();
    }
}
