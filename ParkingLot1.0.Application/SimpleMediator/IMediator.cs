namespace ParkingLot1._0.Application.SimpleMediator
{
    // Interfaz del mediador que envia requests a sus handlers
    public interface IMediator
    {
        // Envio un request que retorna un valor
        Task<TResponse> Send<TResponse>(IRequest<TResponse> request);

        // Envio un request que no retorna valor
        Task Send(IRequest request);
    }
}
