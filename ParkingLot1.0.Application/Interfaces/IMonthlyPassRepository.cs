using ParkingLot1._0.Domain.Entities;

namespace ParkingLot1._0.Application.Interfaces
{
    public interface IMonthlyPassRepository
    {
        Task<int> AddAsync(MonthlyPass monthlyPass);
    }
}
