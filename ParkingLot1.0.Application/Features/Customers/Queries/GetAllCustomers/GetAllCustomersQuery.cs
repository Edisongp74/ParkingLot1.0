using ParkingLot1._0.Application.SimpleMediator;
using ParkingLot1._0.Domain.Entities;

namespace ParkingLot1._0.Application.Features.Customers.Queries.GetAllCustomers
{
    // Query para obtener todos los clientes
    public class GetAllCustomersQuery : IRequest<List<Customer>>
    {
    }
}
