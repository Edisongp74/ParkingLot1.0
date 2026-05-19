using MediatR;

namespace ParkingLot1._0.Application.Features.Vehicles.Commands.DeleteVehicle
{
    // Comando para eliminar un vehiculo por su Id
    public class DeleteVehicleCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
