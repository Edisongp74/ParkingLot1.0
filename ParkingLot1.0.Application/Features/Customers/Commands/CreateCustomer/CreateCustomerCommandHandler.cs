using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using ParkingLot1._0.Application.Interfaces;
using ParkingLot1._0.Domain.Entities;

namespace ParkingLot1._0.Application.Features.Customers.Commands.CreateCustomer
{
    // Me encargo de manejar la creacion de un cliente
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, int>
    {
        private readonly ICustomerRepository _customerRepository;

        // Inyecto el repositorio de clientes
        public CreateCustomerCommandHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        // Creo el cliente con los datos del comando y lo guardo
        public async Task<int> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = new Customer
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                DocumentNumber = request.DocumentNumber,
                DocumentType = request.DocumentType,
                Phone = request.Phone,
                CustomerType = request.CustomerType
            };

            // Guardo el cliente y retorno su Id
            var id = await _customerRepository.AddAsync(customer);
            return id;
        }
    }
}
