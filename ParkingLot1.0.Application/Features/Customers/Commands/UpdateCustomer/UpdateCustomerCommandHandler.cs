using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using ParkingLot1._0.Application.Interfaces;
using ParkingLot1._0.Domain.Entities;

namespace ParkingLot1._0.Application.Features.Customers.Commands.UpdateCustomer
{
    // Me encargo de manejar la actualizacion de un cliente
    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, Unit>
    {
        private readonly ICustomerRepository _customerRepository;

        // Inyecto el repositorio de clientes
        public UpdateCustomerCommandHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        // Busco el cliente, actualizo sus datos y lo guardo
        public async Task<Unit> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = new Customer
            {
                Id = request.Id,
                FirstName = request.FirstName,
                LastName = request.LastName,
                DocumentNumber = request.DocumentNumber,
                DocumentType = request.DocumentType,
                Phone = request.Phone,
                CustomerType = request.CustomerType
            };

            // Actualizo el cliente en la base de datos
            await _customerRepository.UpdateAsync(customer);
            return Unit.Value;
        }
    }
}
