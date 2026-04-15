using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using ParkingLot1._0.Application.Interfaces;
using ParkingLot1._0.Domain.Entities;
using ParkingLot1._0.Domain.Exceptions;

namespace ParkingLot1._0.Application.Features.Customers.Queries.GetCustomerById
{
    // Me encargo de manejar la consulta de un cliente por su Id
    public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, Customer>
    {
        private readonly ICustomerRepository _customerRepository;

        // Inyecto el repositorio de clientes
        public GetCustomerByIdQueryHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        // Busco el cliente por su Id y lo retorno
        public async Task<Customer> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(request.Id);

            if (customer == null)
            {
                throw new NotFoundException($"El cliente con ID {request.Id} no fue encontrado");
            }

            return customer;
        }
    }
}