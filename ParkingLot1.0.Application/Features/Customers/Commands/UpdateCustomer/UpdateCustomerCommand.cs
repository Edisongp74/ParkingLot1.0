using ParkingLot1._0.Application.SimpleMediator;

namespace ParkingLot1._0.Application.Features.Customers.Commands.UpdateCustomer
{
    // Comando para actualizar un cliente existente
    public class UpdateCustomerCommand : IRequest
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string DocumentNumber { get; set; } = string.Empty;
        public string DocumentType { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string CustomerType { get; set; } = string.Empty;
    }
}
