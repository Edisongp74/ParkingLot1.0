using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using ParkingLot1._0.Application.Interfaces;
using ParkingLot1._0.Domain.Entities;

namespace ParkingLot1._0.Application.Features.Vehicles.Queries.GetVehicleById
{
    // Me encargo de manejar la consulta de un vehiculo por su Id
    public class GetVehicleByIdQueryHandler : IRequestHandler<GetVehicleByIdQuery, Vehicle?>
    {
        private readonly IVehicleRepository _vehicleRepository;

        // Inyecto el repositorio de vehiculos
        public GetVehicleByIdQueryHandler(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        // Busco el vehiculo por su Id y lo retorno
        public async Task<Vehicle?> Handle(GetVehicleByIdQuery request, CancellationToken cancellationToken)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(request.Id);
            return vehicle;
        }
    }
}
