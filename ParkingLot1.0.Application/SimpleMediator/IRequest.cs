namespace ParkingLot1._0.Application.SimpleMediator
{
    // Interfaz para requests que retornan un valor
    public interface IRequest<TResponse> { }

    // Interfaz para requests que no retornan valor (void)
    public interface IRequest { }
}
