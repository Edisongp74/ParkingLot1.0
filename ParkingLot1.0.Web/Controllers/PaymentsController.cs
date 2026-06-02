using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkingLot1._0.Application.DTOs.Payments;
using ParkingLot1._0.Application.Interfaces.Services;
using ParkingLot1._0.Domain.Common.Enums;
using ParkingLot1._0.Domain.Entities;
using ParkingLot1._0.Persistence.Contexts;
using ParkingLot1._0.Persistence.Identity;
using ParkingLot1._0.Web.Models;

namespace ParkingLot1._0.Web.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly IPaymentService _paymentService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PaymentsController(IPaymentService paymentService,ApplicationDbContext context,UserManager<ApplicationUser> userManager)
        {
            _paymentService = paymentService;
            _context = context;
            _userManager = userManager;
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
        [HttpGet]
        [Authorize(Roles = "Administrador,Cliente,Operario")]
        public async Task<IActionResult> History()
        {
            IQueryable<Payment> query = _context.Payments
                .Include(p => p.Customer)
                .Include(p => p.PaymentMethod);

            if (User.IsInRole("Cliente"))
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null) return Unauthorized();

                var customer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.ApplicationUserId == user.Id);

                if (customer == null) return NotFound("No se encontró el cliente asociado al usuario.");

                query = query.Where(p => p.CustomerId == customer.Id);
            }

            var payments = await query
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new PaymentHistoryViewModel
                {
                    Id = p.Id,
                    CustomerName = p.Customer.FirstName + " " + p.Customer.LastName,
                    Amount = p.Amount,
                    PaymentMethod = p.PaymentMethod.Name,
                    PaymentType = p.PaymentType,
                    Status = p.Status,
                    Reference = p.Reference,
                    CreatedAt = p.CreatedAt,
                    PaidAt = p.PaidAt
                })
                .ToListAsync();

            return View(payments);
        }
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> CashReport()
        {
            var payments = await _context.Payments
                .Include(p => p.Customer)
                .Include(p => p.PaymentMethod)
                .Where(p => p.Status == PaymentStatus.Paid)
                .OrderByDescending(p => p.PaidAt)
                .Select(p => new PaymentHistoryViewModel
                {
                    Id = p.Id,
                    CustomerName = p.Customer.FirstName + " " + p.Customer.LastName,
                    Amount = p.Amount,
                    PaymentMethod = p.PaymentMethod.Name,
                    PaymentType = p.PaymentType,
                    Status = p.Status,
                    Reference = p.Reference,
                    CreatedAt = p.CreatedAt,
                    PaidAt = p.PaidAt
                })
                .ToListAsync();

            var model = new CashReportViewModel
            {
                TotalIncome = payments.Sum(p => p.Amount),
                TotalPayments = payments.Count,
                TotalMonthlyPayments = payments.Count(p => p.PaymentType.ToString() == "MonthlyMembership"),
                Payments = payments
            };

            return View(model);
        }
    }
}