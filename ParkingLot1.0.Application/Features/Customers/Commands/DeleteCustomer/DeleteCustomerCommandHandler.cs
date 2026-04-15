using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using ParkingLot1._0.Application.Interfaces;
using ParkingLot1._0.Domain.Exceptions;

namespace ParkingLot1._0.Application.Features.Customers.Commands.DeleteCustomer
{
    // Me encargo de manejar la eliminacion de un cliente
    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, Unit>
    {
        private readonly ICustomerRepository _customerRepository;

        // Inyecto el repositorio de clientes
        public DeleteCustomerCommandHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        // Elimino el cliente por su Id
        public async Task<Unit> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(request.Id);

            if (customer == null)
            {
                throw new NotFoundException($"No se puede eliminar, el cliente con ID {request.Id} no existe");
            }

            await _customerRepository.DeleteAsync(request.Id);

            return Unit.Value;
        }
    }
}