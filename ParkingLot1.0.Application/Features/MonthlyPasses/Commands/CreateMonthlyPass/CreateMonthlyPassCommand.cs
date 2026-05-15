using System;
using System.Collections.Generic;
using System.Text;

using MediatR;

namespace ParkingLot1._0.Application.Features.MonthlyPasses.Commands.CreateMonthlyPass
{
    public class CreateMonthlyPassCommand : IRequest<int>
    {
        public int CustomerId { get; set; }
        public int VehicleId { get; set; }
        public int RateId { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;
    }
}