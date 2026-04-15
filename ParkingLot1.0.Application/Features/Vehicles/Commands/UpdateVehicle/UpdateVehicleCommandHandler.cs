using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using ParkingLot1._0.Application.Interfaces;
using ParkingLot1._0.Domain.Entities;

namespace ParkingLot1._0.Application.Features.Vehicles.Commands.UpdateVehicle
{
    // Me encargo de manejar la actualizacion de un vehiculo
    public class UpdateVehicleCommandHandler : IRequestHandler<UpdateVehicleCommand, Unit>
    {
        private readonly IVehicleRepository _vehicleRepository;

        // Inyecto el repositorio de vehiculos
        public UpdateVehicleCommandHandler(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        // Busco el vehiculo, actualizo sus datos y lo guardo
        public async Task<Unit> Handle(UpdateVehicleCommand request, CancellationToken cancellationToken)
        {
            var vehicle = new Vehicle
            {
                Id = request.Id,
                LicensePlate = request.LicensePlate,
                Type = request.Type,
                Brand = request.Brand,
                Color = request.Color,
                CustomerId = request.CustomerId
            };

            // Actualizo el vehiculo en la base de datos
            await _vehicleRepository.UpdateAsync(vehicle);
            return Unit.Value;
        }
    }
}
