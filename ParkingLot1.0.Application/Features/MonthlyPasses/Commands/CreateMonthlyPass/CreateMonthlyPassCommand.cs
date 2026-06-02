using ParkingLot1._0.Application.SimpleMediator;

namespace ParkingLot1._0.Application.Features.MonthlyPasses.Commands.CreateMonthlyPass
{
    public class CreateMonthlyPassCommand : IRequest<int>
    {
        public int CustomerId { get; set; }
        public int VehicleId { get; set; }
        public DateTime StartDate { get; set; }
        public int RateId { get; set; }
    }
}