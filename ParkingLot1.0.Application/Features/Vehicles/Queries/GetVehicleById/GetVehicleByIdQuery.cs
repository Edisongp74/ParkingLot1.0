using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using ParkingLot1._0.Domain.Entities;

namespace ParkingLot1._0.Application.Features.Vehicles.Queries.GetVehicleById
{
    // Query para obtener un vehiculo por su Id
    public class GetVehicleByIdQuery : IRequest<Vehicle?>
    {
        public int Id { get; set; }
    }
}
