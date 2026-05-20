using ParkingLot1._0.Application.SimpleMediator;
using ParkingLot1._0.Application.Interfaces;
using ParkingLot1._0.Domain.Entities;

namespace ParkingLot1._0.Application.Features.MonthlyPasses.Commands.CreateMonthlyPass
{
    public class CreateMonthlyPassCommandHandler : IRequestHandler<CreateMonthlyPassCommand, int>
    {
        private readonly IMonthlyPassRepository _repository;

        public CreateMonthlyPassCommandHandler(IMonthlyPassRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> Handle(CreateMonthlyPassCommand request)
        {
            var entity = new MonthlyPass
            {
                CustomerId = request.CustomerId,
                VehicleId = request.VehicleId,
                RateId = request.RateId,
                StartDate = request.StartDate,
                EndDate = request.StartDate.AddDays(30),
                Status = "Active"
            };

            // Usamos el metodo del repositorio
            return await _repository.AddAsync(entity);
        }
    }
}
