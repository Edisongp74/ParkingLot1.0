using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using ParkingLot1._0.Application.Interfaces;

namespace ParkingLot1._0.Application.Features.Vehicles.Commands.DeleteVehicle
{
    // Me encargo de manejar la eliminacion de un vehiculo
    public class DeleteVehicleCommandHandler : IRequestHandler<DeleteVehicleCommand, Unit>
    {
        private readonly IVehicleRepository _vehicleRepository;

        // Inyecto el repositorio de vehiculos
        public DeleteVehicleCommandHandler(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        // Elimino el vehiculo por su Id
        public async Task<Unit> Handle(DeleteVehicleCommand request, CancellationToken cancellationToken)
        {
            await _vehicleRepository.DeleteAsync(request.Id);
            return Unit.Value;
        }
    }
}
