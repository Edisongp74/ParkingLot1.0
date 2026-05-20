namespace ParkingLot1._0.Application.Exceptions
{
    // Excepcion para cuando el mediator no encuentra un handler registrado
    public class MediatorException : Exception
    {
        public MediatorException(string message) : base(message) { }
    }
}
