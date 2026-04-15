using System;
using System.Collections.Generic;
using System.Text;
using MediatR;

namespace ParkingLot1._0.Application.Features.Customers.Commands.DeleteCustomer
{
    // Comando para eliminar un cliente por su Id
    public class DeleteCustomerCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
