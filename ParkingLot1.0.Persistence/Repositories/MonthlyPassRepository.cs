using ParkingLot1._0.Application.Interfaces;
using ParkingLot1._0.Domain.Entities;
using ParkingLot1._0.Persistence.Contexts;

namespace ParkingLot1._0.Persistence.Repositories
{
    public class MonthlyPassRepository : IMonthlyPassRepository
    {
        private readonly ApplicationDbContext _context;

        public MonthlyPassRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(MonthlyPass monthlyPass)
        {
            _context.MonthlyPasses.Add(monthlyPass);
            await _context.SaveChangesAsync();
            return monthlyPass.Id;
        }
    }
}
