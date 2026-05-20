using ParkingLot1._0.Application.SimpleMediator;

namespace ParkingLot1._0.Application.Features.Vehicles.Commands.DeleteVehicle
{
    // Comando para eliminar un vehiculo por su Id
    public class DeleteVehicleCommand : IRequest
    {
        public int Id { get; set; }
    }
}
