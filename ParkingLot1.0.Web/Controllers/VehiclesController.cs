using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using AspNetCoreHero.ToastNotification.Abstractions;
using ParkingLot1._0.Application.Common;
using ParkingLot1._0.Application.Exceptions;
using ParkingLot1._0.Application.Features.Customers.Queries.GetAllCustomers;
using ParkingLot1._0.Application.Features.Vehicles.Commands.CreateVehicle;
using ParkingLot1._0.Application.Features.Vehicles.Commands.DeleteVehicle;
using ParkingLot1._0.Application.Features.Vehicles.Commands.UpdateVehicle;
using ParkingLot1._0.Application.Features.Vehicles.Queries.GetAllVehicles;
using ParkingLot1._0.Application.Features.Vehicles.Queries.GetVehicleById;
using ParkingLot1._0.Application.Interfaces;
using ParkingLot1._0.Application.SimpleMediator;
using ParkingLot1._0.Domain.Entities;
using ParkingLot1._0.Domain.Exceptions;
using ParkingLot1._0.Persistence.Contexts;
using ParkingLot1._0.Web.DTOs.Vehicles;

namespace ParkingLot1._0.Web.Controllers
{
    [Authorize]
    public class VehiclesController : Controller
    {
        private readonly IMediator _mediator;
        private readonly INotyfService _notyf;
        private readonly ApplicationDbContext _context;
        private readonly ICustomerRepository _customerRepository;
        private readonly IVehicleRepository _vehicleRepository;

        public VehiclesController(
            IMediator mediator,
            INotyfService notyf,
            ApplicationDbContext context,
            ICustomerRepository customerRepository,
            IVehicleRepository vehicleRepository)
        {
            _mediator = mediator;
            _notyf = notyf;
            _context = context;
            _customerRepository = customerRepository;
            _vehicleRepository = vehicleRepository;
        }


        [Authorize]
        public async Task<IActionResult> MyVehicles()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var customer = await _customerRepository.GetByApplicationUserIdAsync(userId);

            if (customer == null)
            {
                _notyf.Warning("Your account is not linked to a customer profile.");
                return RedirectToAction("Index", "Home");
            }

            var vehicles = await _vehicleRepository.GetByCustomerIdAsync(customer.Id);

            return View(vehicles);
        }

        [Authorize]
        public IActionResult CreateMyVehicle()
        {
            return View(new CreateVehicleDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> CreateMyVehicle(CreateVehicleDto dto)
        {
            if (!ModelState.IsValid)
            {
                _notyf.Error("There are validation errors in the form.");
                return View(dto);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var customer = await _customerRepository.GetByApplicationUserIdAsync(userId);

            if (customer == null)
            {
                _notyf.Warning("Your account is not linked to a customer profile.");
                return RedirectToAction("Index", "Home");
            }

            if (!customer.CanAddVehicle())
            {
                _notyf.Warning("You cannot register more than 3 vehicles.");
                return RedirectToAction(nameof(MyVehicles));
            }

            try
            {
                var command = new CreateVehicleCommand
                {
                    LicensePlate = dto.LicensePlate,
                    Type = dto.Type,
                    Brand = dto.Brand,
                    Color = dto.Color,
                    CustomerId = customer.Id
                };

                await _mediator.Send(command);

                _context.AuditLogs.Add(new AuditLog
                {
                    Usuario = User.Identity?.Name ?? "Anonymous",
                    Accion = "Create",
                    Detalle = $"Customer created vehicle with plate: {dto.LicensePlate}",
                    ControllerName = "Vehicles",
                    ActionName = "CreateMyVehicle",
                    IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1",
                    FechaRegistro = DateTime.Now
                });

                await _context.SaveChangesAsync();

                _notyf.Success("Vehicle created successfully.");
                return RedirectToAction(nameof(MyVehicles));
            }
            catch (CustomValidationException ex)
            {
                _notyf.Error(string.Join(", ", ex.Errors));
                return View(dto);
            }
            catch (BusinessException ex)
            {
                _notyf.Error(ex.Message);
                return View(dto);
            }
        }

        [Authorize]
        [Authorize]
        public async Task<IActionResult> LinkMyCustomer()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = User.Identity?.Name ?? "Cliente";

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var existingCustomer = await _customerRepository.GetByApplicationUserIdAsync(userId);

            if (existingCustomer != null)
            {
                _notyf.Success("Your account is already linked to a customer profile.");
                return RedirectToAction(nameof(MyVehicles));
            }

            var customer = await _customerRepository.GetFirstCustomerWithoutUserAsync();

            if (customer != null)
            {
                customer.ApplicationUserId = userId;
                await _customerRepository.UpdateAsync(customer);

                _notyf.Success("Your account was linked successfully.");
                return RedirectToAction(nameof(MyVehicles));
            }

            var newCustomer = new Customer
            {
                FirstName = userName,
                LastName = "Auto",
                DocumentType = "CC",
                DocumentNumber = $"AUTO-{Guid.NewGuid().ToString("N").Substring(0, 8)}",
                Phone = "0000000000",
                CustomerType = "Regular",
                ApplicationUserId = userId
            };

            await _customerRepository.AddAsync(newCustomer);

            _notyf.Success("A new customer profile was created and linked successfully.");
            return RedirectToAction(nameof(MyVehicles));
        }

        [Authorize(Roles = "Administrador,Operador")]
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 5)
        {
            var vehicles = await _mediator.Send(new GetAllVehiclesQuery());

            var vehiclesQuery = vehicles.Cast<object>().AsQueryable();

            var pagedVehicles = PagedList<object>.Create(vehiclesQuery, pageNumber, pageSize);

            return View(pagedVehicles);
        }
    }
}