using ParkingLot1._0.Domain.Entities;

namespace ParkingLot1._0.Application.Interfaces
{
    public interface ICustomerRepository
    {
        Task<List<Customer>> GetAllAsync();


        Task<Customer?> GetByIdAsync(int id);

        Task<int> AddAsync(Customer customer);

        Task UpdateAsync(Customer customer);


        Task DeleteAsync(int id);

        Task<Customer?> GetByApplicationUserIdAsync(string applicationUserId);

        Task<Customer?> GetFirstCustomerWithoutUserAsync();

       

    }
}
