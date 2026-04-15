using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace ParkingLot1._0.Application.Features.ParkingRecords.Commands.CreateParkingRecord
{
    public class CreateParkingRecordCommand : IRequest<int>
    {
        public string LicensePlate { get; set; } = string.Empty;
        public int ParkingSpotId { get; set; }
        public int EmployeeId { get; set; }
    }
}
