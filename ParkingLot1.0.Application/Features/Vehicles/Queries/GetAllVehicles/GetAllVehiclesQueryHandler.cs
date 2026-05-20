using ParkingLot1._0.Application.SimpleMediator;
using ParkingLot1._0.Application.Interfaces;
using ParkingLot1._0.Domain.Entities;

namespace ParkingLot1._0.Application.Features.Vehicles.Queries.GetAllVehicles
{
    // Me encargo de manejar la consulta de todos los vehiculos
    public class GetAllVehiclesQueryHandler : IRequestHandler<GetAllVehiclesQuery, List<Vehicle>>
    {
        private readonly IVehicleRepository _vehicleRepository;

        // Inyecto el repositorio de vehiculos
        public GetAllVehiclesQueryHandler(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        // Obtengo todos los vehiculos desde el repositorio
        public async Task<List<Vehicle>> Handle(GetAllVehiclesQuery request)
        {
            var vehicles = await _vehicleRepository.GetAllAsync();
            return vehicles;
        }
    }
}
