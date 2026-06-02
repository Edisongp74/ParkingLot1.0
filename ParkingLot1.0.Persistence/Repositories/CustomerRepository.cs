using Microsoft.EntityFrameworkCore;
using ParkingLot1._0.Application.Interfaces;
using ParkingLot1._0.Domain.Entities;
using ParkingLot1._0.Persistence.Contexts;

namespace ParkingLot1._0.Persistence.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            return await _context.Customers
                .Include(c => c.Vehicles)
                .Include(c => c.MonthlyPasses)
                .ToListAsync();
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            return await _context.Customers
                .Include(c => c.Vehicles)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<int> AddAsync(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
            return customer.Id;
        }

        public async Task UpdateAsync(Customer customer)
        {
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var customer = await _context.Customers
                .Include(c => c.Vehicles)
                .Include(c => c.MonthlyPasses)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (customer == null)
                return;

            var monthlyPassIds = await _context.MonthlyPasses
                .Where(m => m.CustomerId == id)
                .Select(m => m.Id)
                .ToListAsync();

            var payments = await _context.Payments
                .Where(p => p.CustomerId == id || (p.MonthlyMembershipId.HasValue && monthlyPassIds.Contains(p.MonthlyMembershipId.Value)))
                .ToListAsync();

            var monthlyPasses = await _context.MonthlyPasses
                .Where(m => m.CustomerId == id)
                .ToListAsync();

            if (payments.Any())
                _context.Payments.RemoveRange(payments);

            if (monthlyPasses.Any())
                _context.MonthlyPasses.RemoveRange(monthlyPasses);

            if (customer.Vehicles.Any())
                _context.Vehicles.RemoveRange(customer.Vehicles);

            _context.Customers.Remove(customer);

            await _context.SaveChangesAsync();
        }
        public async Task<Customer?> GetByApplicationUserIdAsync(string applicationUserId)
        {
            return await _context.Customers
                .Include(c => c.Vehicles)
                .FirstOrDefaultAsync(c => c.ApplicationUserId == applicationUserId);
        }
        public async Task<Customer?> GetFirstCustomerWithoutUserAsync()
        {
            return await _context.Customers
                .FirstOrDefaultAsync(c => c.ApplicationUserId == null);
        }

        
    }
}