
using System;

namespace ParkingLot1._0.Domain.Entities
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public string Accion { get; set; } = string.Empty; 
        public string Detalle { get; set; } = string.Empty; 
        public string ControllerName { get; set; } = string.Empty;
        public string ActionName { get; set; } = string.Empty;
        public string IpAddress { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}
