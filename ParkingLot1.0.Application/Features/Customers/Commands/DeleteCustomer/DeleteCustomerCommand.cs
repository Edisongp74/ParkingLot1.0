using ParkingLot1._0.Application.SimpleMediator;

namespace ParkingLot1._0.Application.Features.Customers.Commands.DeleteCustomer
{
    // Comando para eliminar un cliente por su Id
    public class DeleteCustomerCommand : IRequest
    {
        public int Id { get; set; }
    }
}
