using ParkingLot1._0.Application.SimpleMediator;
using ParkingLot1._0.Domain.Entities;

namespace ParkingLot1._0.Application.Features.Customers.Queries.GetCustomerById
{
    // Query para obtener un cliente por su Id
    public class GetCustomerByIdQuery : IRequest<Customer?>
    {
        public int Id { get; set; }
    }
}
