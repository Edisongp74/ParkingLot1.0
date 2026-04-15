using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using ParkingLot1._0.Application.Interfaces;
using ParkingLot1._0.Domain.Entities;
using ParkingLot1._0.Domain.Exceptions;

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
            
            var existingCustomer = await _customerRepository.GetByIdAsync(request.Id);

            if (existingCustomer == null)
            {
                throw new NotFoundException($"El cliente con ID {request.Id} no existe");
            }

            
            if (string.IsNullOrWhiteSpace(request.FirstName))
            {
                throw new BusinessException("El nombre es obligatorio");
            }

            if (string.IsNullOrWhiteSpace(request.LastName))
            {
                throw new BusinessException("El apellido es obligatorio");
            }

            if (string.IsNullOrWhiteSpace(request.DocumentNumber))
            {
                throw new BusinessException("El documento es obligatorio");
            }

            
            existingCustomer.FirstName = request.FirstName;
            existingCustomer.LastName = request.LastName;
            existingCustomer.DocumentNumber = request.DocumentNumber;
            existingCustomer.DocumentType = request.DocumentType;
            existingCustomer.Phone = request.Phone;
            existingCustomer.CustomerType = request.CustomerType;

            // Guardar cambios
            await _customerRepository.UpdateAsync(existingCustomer);

            return Unit.Value;
        }
    }
}