using System;
using System.Collections.Generic;
using System.Text;
using ParkingLot1._0.Domain.Entities;

namespace ParkingLot1._0.Application.Interfaces
{
    // Defino el contrato del repositorio de vehiculos
    public interface IVehicleRepository
    {
        // Obtengo todos los vehiculos
        Task<List<Vehicle>> GetAllAsync();

        // Busco un vehiculo por su Id
        Task<Vehicle?> GetByIdAsync(int id);

        // Agrego un nuevo vehiculo
        Task<int> AddAsync(Vehicle vehicle);

        // Actualizo un vehiculo existente
        Task UpdateAsync(Vehicle vehicle);

        // Elimino un vehiculo por su Id
        Task DeleteAsync(int id);
    }
}
