using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkingLot1._0.Application.DTOs.Payments;
using ParkingLot1._0.Application.Interfaces.Services;
using ParkingLot1._0.Domain.Common.Enums;
using ParkingLot1._0.Persistence.Contexts;

namespace ParkingLot1._0.Web.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly IPaymentService _paymentService;
        private readonly ApplicationDbContext _context;

        public PaymentsController(IPaymentService paymentService, ApplicationDbContext context)
        {
            _paymentService = paymentService;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            if (TempData.Peek("CustomerId") == null || TempData.Peek("MonthlyMembershipId") == null)
            {
                return BadRequest("No se recibió la información de la mensualidad.");
            }

            int customerId = Convert.ToInt32(TempData.Peek("CustomerId"));
            int monthlyMembershipId = Convert.ToInt32(TempData.Peek("MonthlyMembershipId"));

            ViewBag.PaymentMethods = await _context.PaymentMethods
                .Where(pm => pm.IsActive)
                .ToListAsync();

            var model = new CreatePaymentDto
            {
                CustomerId = customerId,
                MonthlyMembershipId = monthlyMembershipId,
                PaymentType = PaymentType.MonthlyMembership
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePaymentDto dto)
        {
            if (dto.CustomerId <= 0)
            {
                ModelState.AddModelError(nameof(dto.CustomerId), "El cliente no es válido.");
            }

            if (dto.MonthlyMembershipId == null || dto.MonthlyMembershipId <= 0)
            {
                ModelState.AddModelError(nameof(dto.MonthlyMembershipId), "La mensualidad no es válida.");
            }

            if (dto.PaymentMethodId <= 0)
            {
                ModelState.AddModelError(nameof(dto.PaymentMethodId), "Seleccione un método de pago.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.PaymentMethods = await _context.PaymentMethods
                    .Where(pm => pm.IsActive)
                    .ToListAsync();

                return View(dto);
            }

            var paymentId = await _paymentService.CreatePaymentAsync(dto);

            return RedirectToAction(nameof(Confirm), new { id = paymentId });
        }

        [HttpGet]
        public async Task<IActionResult> Confirm(int id)
        {
            var result = await _paymentService.ConfirmPaymentAsync(id);

            if (!result)
                return NotFound();

            TempData["Message"] = "Pago confirmado y mensualidad activada correctamente.";
            return RedirectToAction("Index", "Home");
        }
    }
}