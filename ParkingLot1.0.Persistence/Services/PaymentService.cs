using Microsoft.EntityFrameworkCore;
using ParkingLot1._0.Application.DTOs.Payments;
using ParkingLot1._0.Application.Interfaces.Services;
using ParkingLot1._0.Domain.Common.Enums;
using ParkingLot1._0.Domain.Entities;
using ParkingLot1._0.Persistence.Contexts;

namespace ParkingLot1._0.Persistence.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly ApplicationDbContext _context;

        public PaymentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreatePaymentAsync(CreatePaymentDto dto)
        {
            if (dto.PaymentMethodId <= 0)
                throw new Exception("PaymentMethodId inválido.");

            if (dto.MonthlyMembershipId == null || dto.MonthlyMembershipId <= 0)
                throw new Exception("MonthlyMembershipId inválido.");

            var paymentMethodExists = await _context.PaymentMethods
                .AnyAsync(pm => pm.Id == dto.PaymentMethodId);

            if (!paymentMethodExists)
                throw new Exception($"No existe un método de pago con Id = {dto.PaymentMethodId}");

            var monthlyPass = await _context.MonthlyPasses
                .FirstOrDefaultAsync(m => m.Id == dto.MonthlyMembershipId.Value);

            if (monthlyPass == null)
                throw new Exception($"No existe la mensualidad con Id = {dto.MonthlyMembershipId}");

            var payment = new Payment
            {
                CustomerId = monthlyPass.CustomerId,
                PaymentMethodId = dto.PaymentMethodId,
                Amount = dto.Amount,
                PaymentType = dto.PaymentType,
                MonthlyMembershipId = monthlyPass.Id,
                Reference = dto.Reference,
                Status = PaymentStatus.Pending,
                CreatedAt = DateTime.Now,
                IsMonthlyPayment = dto.PaymentType == PaymentType.MonthlyMembership
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return payment.Id;
        }
        public async Task<bool> ConfirmPaymentAsync(int paymentId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var payment = await _context.Payments.FirstOrDefaultAsync(p => p.Id == paymentId);

                if (payment == null)
                    return false;

                if (payment.Status == PaymentStatus.Paid)
                    return true;

                payment.Status = PaymentStatus.Paid;
                payment.PaidAt = DateTime.Now;

                if (payment.PaymentType == PaymentType.MonthlyMembership)
                {
                    var monthly = await _context.MonthlyPasses
                        .FirstOrDefaultAsync(m => m.Id == payment.MonthlyMembershipId);

                    if (monthly != null)
                    {
                        //monthly.IsActive = true;
                        monthly.StartDate = DateTime.Now;
                        monthly.EndDate = DateTime.Now.AddMonths(1);
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> FailPaymentAsync(int paymentId)
        {
            var payment = await _context.Payments.FirstOrDefaultAsync(p => p.Id == paymentId);

            if (payment == null)
                return false;

            payment.Status = PaymentStatus.Failed;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}