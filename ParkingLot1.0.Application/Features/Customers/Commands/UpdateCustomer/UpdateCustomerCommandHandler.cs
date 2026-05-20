using ParkingLot1._0.Application.SimpleMediator;
using ParkingLot1._0.Application.Interfaces;
using ParkingLot1._0.Domain.Exceptions;

namespace ParkingLot1._0.Application.Features.Customers.Commands.UpdateCustomer
{
    // Me encargo de manejar la actualizacion de un cliente
    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand>
    {
        private readonly ICustomerRepository _customerRepository;

        // Inyecto el repositorio de clientes
        public UpdateCustomerCommandHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        // Busco el cliente, actualizo sus datos y lo guardo
        public async Task Handle(UpdateCustomerCommand request)
        {
            var existingCustomer = await _customerRepository.GetByIdAsync(request.Id);

            if (existingCustomer == null)
            {
                throw new NotFoundException($"El cliente con ID {request.Id} no existe");
            }

            existingCustomer.FirstName = request.FirstName;
            existingCustomer.LastName = request.LastName;
            existingCustomer.DocumentNumber = request.DocumentNumber;
            existingCustomer.DocumentType = request.DocumentType;
            existingCustomer.Phone = request.Phone;
            existingCustomer.CustomerType = request.CustomerType;

            // Guardar cambios
            await _customerRepository.UpdateAsync(existingCustomer);
        }
    }
}
