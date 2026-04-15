using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using ParkingLot1._0.Application.Interfaces;
using ParkingLot1._0.Domain.Entities;

namespace ParkingLot1._0.Application.Features.Vehicles.Commands.CreateVehicle
{
    // Me encargo de manejar la creacion de un vehiculo
    public class CreateVehicleCommandHandler : IRequestHandler<CreateVehicleCommand, int>
    {
        private readonly IVehicleRepository _vehicleRepository;

        // Inyecto el repositorio de vehiculos
        public CreateVehicleCommandHandler(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        // Creo el vehiculo con los datos del comando y lo guardo
        public async Task<int> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
        {
            var vehicle = new Vehicle
            {
                LicensePlate = request.LicensePlate,
                Type = request.Type,
                Brand = request.Brand,
                Color = request.Color,
                CustomerId = request.CustomerId
            };

            // Guardo el vehiculo y retorno su Id
            var id = await _vehicleRepository.AddAsync(vehicle);
            return id;
        }
    }
}
