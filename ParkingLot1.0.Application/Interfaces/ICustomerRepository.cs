using System;
using System.Collections.Generic;
using System.Text;
using ParkingLot1._0.Domain.Entities;

namespace ParkingLot1._0.Application.Interfaces
{
    // Defino el contrato del repositorio de clientes
    public interface ICustomerRepository
    {
        // Obtengo todos los clientes
        Task<List<Customer>> GetAllAsync();

        // Busco un cliente por su Id
        Task<Customer?> GetByIdAsync(int id);

        // Agrego un nuevo cliente
        Task<int> AddAsync(Customer customer);

        // Actualizo un cliente existente
        Task UpdateAsync(Customer customer);

        // Elimino un cliente por su Id
        Task DeleteAsync(int id);
    }
}
