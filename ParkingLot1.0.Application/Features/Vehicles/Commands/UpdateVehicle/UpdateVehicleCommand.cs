using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace ParkingLot1._0.Application.Features.Vehicles.Commands.UpdateVehicle
{
    // Comando para actualizar un vehiculo existente
    public class UpdateVehicleCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public string LicensePlate { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public int CustomerId { get; set; }
    }
}
