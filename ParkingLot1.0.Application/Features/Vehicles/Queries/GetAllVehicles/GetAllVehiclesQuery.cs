using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using ParkingLot1._0.Domain.Entities;

namespace ParkingLot1._0.Application.Features.Vehicles.Queries.GetAllVehicles
{
    // Query para obtener todos los vehiculos
    public class GetAllVehiclesQuery : IRequest<List<Vehicle>>
    {
    }
}
