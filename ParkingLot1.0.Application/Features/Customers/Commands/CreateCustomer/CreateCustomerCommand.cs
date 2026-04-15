using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace ParkingLot1._0.Application.Features.Customers.Commands.CreateCustomer
{
    // Comando para crear un nuevo cliente
    public class CreateCustomerCommand : IRequest<int>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string DocumentNumber { get; set; } = string.Empty;
        public string DocumentType { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string CustomerType { get; set; } = string.Empty;
    }
}
