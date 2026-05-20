using ParkingLot1._0.Application.SimpleMediator;

namespace ParkingLot1._0.Application.Features.Vehicles.Commands.UpdateVehicle
{
    // Comando para actualizar un vehiculo existente
    public class UpdateVehicleCommand : IRequest
    {
        public int Id { get; set; }
        public string LicensePlate { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public int CustomerId { get; set; }
    }
}
