using ParkingLot1._0.Application.DTOs.Payments;

namespace ParkingLot1._0.Application.Interfaces.Services
{
    public interface IPaymentService
    {
        Task<int> CreatePaymentAsync(CreatePaymentDto dto);
        Task<bool> ConfirmPaymentAsync(int paymentId);
        Task<bool> FailPaymentAsync(int paymentId);
    }
}