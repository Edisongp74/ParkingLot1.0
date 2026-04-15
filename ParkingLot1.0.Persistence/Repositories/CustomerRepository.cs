using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ParkingLot1._0.Application.Interfaces;
using ParkingLot1._0.Domain.Entities;
using ParkingLot1._0.Persistence.Contexts;

namespace ParkingLot1._0.Persistence.Repositories
{
    // Implemento el repositorio de clientes usando Entity Framework
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _context;

        // Inyecto el contexto de base de datos
        public CustomerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Obtengo todos los clientes de la base de datos
        public async Task<List<Customer>> GetAllAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        // Busco un cliente por su Id
        public async Task<Customer?> GetByIdAsync(int id)
        {
            return await _context.Customers.FindAsync(id);
        }

        // Agrego un nuevo cliente y retorno su Id
        public async Task<int> AddAsync(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
            return customer.Id;
        }

        // Actualizo un cliente existente en la base de datos
        public async Task UpdateAsync(Customer customer)
        {
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
        }

        // Elimino un cliente por su Id, junto con sus vehiculos asociados
        public async Task DeleteAsync(int id)
        {
            var customer = await _context.Customers
                .Include(c => c.Vehicles)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (customer != null)
            {
                // Primero elimino los vehiculos que tenga asociados
                _context.Vehicles.RemoveRange(customer.Vehicles);
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
            }
        }
    }
}
