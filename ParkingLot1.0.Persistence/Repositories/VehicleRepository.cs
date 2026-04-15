using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ParkingLot1._0.Application.Interfaces;
using ParkingLot1._0.Domain.Entities;
using ParkingLot1._0.Persistence.Contexts;

namespace ParkingLot1._0.Persistence.Repositories
{
    // Implemento el repositorio de vehiculos usando Entity Framework
    public class VehicleRepository : IVehicleRepository
    {
        private readonly ApplicationDbContext _context;

        // Inyecto el contexto de base de datos
        public VehicleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Obtengo todos los vehiculos incluyendo su cliente
        public async Task<List<Vehicle>> GetAllAsync()
        {
            return await _context.Vehicles.Include(v => v.Customer).ToListAsync();
        }

        // Busco un vehiculo por su Id incluyendo su cliente
        public async Task<Vehicle?> GetByIdAsync(int id)
        {
            return await _context.Vehicles.Include(v => v.Customer)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        // Agrego un nuevo vehiculo y retorno su Id
        public async Task<int> AddAsync(Vehicle vehicle)
        {
            await _context.Vehicles.AddAsync(vehicle);
            await _context.SaveChangesAsync();
            return vehicle.Id;
        }

        // Actualizo un vehiculo existente en la base de datos
        public async Task UpdateAsync(Vehicle vehicle)
        {
            _context.Vehicles.Update(vehicle);
            await _context.SaveChangesAsync();
        }

        // Elimino un vehiculo por su Id
        public async Task DeleteAsync(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle != null)
            {
                _context.Vehicles.Remove(vehicle);
                await _context.SaveChangesAsync();
            }
        }
    }
}
