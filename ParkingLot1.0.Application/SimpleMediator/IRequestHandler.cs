namespace ParkingLot1._0.Application.SimpleMediator
{
    // Interfaz para handlers que retornan un valor
    public interface IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        Task<TResponse> Handle(TRequest request);
    }

    // Interfaz para handlers que no retornan valor (void)
    public interface IRequestHandler<TRequest>
        where TRequest : IRequest
    {
        Task Handle(TRequest request);
    }
}
