using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ParkingLot1._0.Application.Common;
using ParkingLot1._0.Application.Exceptions;
using ParkingLot1._0.Application.Features.Customers.Commands.CreateCustomer;
using ParkingLot1._0.Application.Features.Customers.Commands.DeleteCustomer;
using ParkingLot1._0.Application.Features.Customers.Commands.UpdateCustomer;
using ParkingLot1._0.Application.Features.Customers.Queries.GetAllCustomers;
using ParkingLot1._0.Application.Features.Customers.Queries.GetCustomerById;
using ParkingLot1._0.Application.Features.MonthlyPasses.Commands.CreateMonthlyPass;
using ParkingLot1._0.Application.Interfaces;
using ParkingLot1._0.Application.SimpleMediator;
using ParkingLot1._0.Domain.Entities;
using ParkingLot1._0.Domain.Exceptions;
using ParkingLot1._0.Persistence.Contexts;
using ParkingLot1._0.Web.DTOs.Customers;
using ParkingLot1._0.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ParkingLot1._0.Persistence.Identity;

namespace ParkingLot1._0.Web.Controllers
{
    [Authorize]
    public class CustomersController : Controller
    {
        private readonly IMediator _mediator;
        private readonly INotyfService _notyf;
        private readonly ApplicationDbContext _context;
        private readonly ICustomerRepository _customerRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        public CustomersController(
            IMediator mediator,
            INotyfService notyf,
            ApplicationDbContext context,
            ICustomerRepository customerRepository,
            IVehicleRepository vehicleRepository,
            UserManager<ApplicationUser> userManager)

        {
            _mediator = mediator;
            _notyf = notyf;
            _context = context;
            _customerRepository = customerRepository;
            _vehicleRepository = vehicleRepository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 5)
        {
            var customers = await _mediator.Send(new GetAllCustomersQuery());

            var viewModelQuery = customers.Select(c => new CustomerViewModel
            {
                Id = c.Id,
                FullName = $"{c.FirstName} {c.LastName}",
                DocumentNumber = c.DocumentNumber,
                Phone = c.Phone,
                TotalVehicles = c.Vehicles?.Count ?? 0,
                HasActivePass = c.HasActiveMonthlyPass()
            }).AsQueryable();

            var pagedViewModel = PagedList<CustomerViewModel>.Create(viewModelQuery, pageNumber, pageSize);

            return View(pagedViewModel);
        }

        public IActionResult Create()
        {
            return View(new CreateCustomerDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCustomerDto dto)
        {
            if (!ModelState.IsValid)
            {
                _notyf.Error("Hay errores de validacion en el formulario");
                return View(dto);
            }

            try
            {
                var command = new CreateCustomerCommand
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    DocumentNumber = dto.DocumentNumber,
                    DocumentType = dto.DocumentType,
                    Phone = dto.Phone,
                    CustomerType = dto.CustomerType
                };

                await _mediator.Send(command);

                _context.AuditLogs.Add(new AuditLog
                {
                    Usuario = User.Identity?.Name ?? "Anónimo",
                    Accion = "Crear",
                    Detalle = $"Se registró al cliente {dto.FirstName} {dto.LastName} con Documento: {dto.DocumentNumber}",
                    ControllerName = "Customers",
                    ActionName = "Create",
                    IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1",
                    FechaRegistro = DateTime.Now
                });

                await _context.SaveChangesAsync();

                _notyf.Success("Cliente creado exitosamente");
                return RedirectToAction(nameof(Index));
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

        public async Task<IActionResult> Edit(int id)
        {
            var customer = await _mediator.Send(new GetCustomerByIdQuery { Id = id });

            if (customer == null)
            {
                return NotFound();
            }

            var dto = new UpdateCustomerDto
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                DocumentNumber = customer.DocumentNumber,
                DocumentType = customer.DocumentType,
                Phone = customer.Phone,
                CustomerType = customer.CustomerType
            };

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateCustomerDto dto)
        {
            if (id != dto.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                _notyf.Error("Hay errores de validacion en el formulario");
                return View(dto);
            }

            try
            {
                var command = new UpdateCustomerCommand
                {
                    Id = dto.Id,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    DocumentNumber = dto.DocumentNumber,
                    DocumentType = dto.DocumentType,
                    Phone = dto.Phone,
                    CustomerType = dto.CustomerType
                };

                await _mediator.Send(command);

                _context.AuditLogs.Add(new AuditLog
                {
                    Usuario = User.Identity?.Name ?? "Anónimo",
                    Accion = "Modificar",
                    Detalle = $"Se actualizaron los datos del cliente ID {dto.Id}. Nuevo nombre: {dto.FirstName} {dto.LastName}",
                    ControllerName = "Customers",
                    ActionName = "Edit",
                    IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1",
                    FechaRegistro = DateTime.Now
                });

                await _context.SaveChangesAsync();

                _notyf.Success("Cliente actualizado exitosamente");
                return RedirectToAction(nameof(Index));
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

        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _mediator.Send(new GetCustomerByIdQuery { Id = id });

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var customer = await _mediator.Send(new GetCustomerByIdQuery { Id = id });
                string name = customer != null ? $"{customer.FirstName} {customer.LastName}" : id.ToString();

                await _mediator.Send(new DeleteCustomerCommand { Id = id });

                _context.AuditLogs.Add(new AuditLog
                {
                    Usuario = User.Identity?.Name ?? "Anónimo",
                    Accion = "Eliminar",
                    Detalle = $"Se eliminó al cliente: {name} (ID: {id})",
                    ControllerName = "Customers",
                    ActionName = "Delete",
                    IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1",
                    FechaRegistro = DateTime.Now
                });

                await _context.SaveChangesAsync();

                _notyf.Success("Cliente eliminado exitosamente");
                return RedirectToAction(nameof(Index));
            }
            catch (NotFoundException ex)
            {
                _notyf.Error(ex.Message);
                return RedirectToAction(nameof(Index));
            }
            catch (BusinessException ex)
            {
                _notyf.Error(ex.Message);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _notyf.Error(ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> BuyPass(int id, int? vehicleId)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null)
                return NotFound();

            var vehicles = await _vehicleRepository.GetByCustomerIdAsync(id);

            var model = new BuyMonthlyPassViewModel
            {
                CustomerId = id,
                CustomerName = $"{customer.FirstName} {customer.LastName}",
                VehicleId = vehicleId ?? 0,
                StartDate = DateTime.Today,
                Vehicles = vehicles.Select(v => new VehicleViewModel
                {
                    Id = v.Id,
                    Plate = $"{v.LicensePlate} - {v.Brand}"
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BuyPass(BuyMonthlyPassViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var vehicles = await _vehicleRepository.GetByCustomerIdAsync(model.CustomerId);
                model.Vehicles = vehicles.Select(v => new VehicleViewModel
                {
                    Id = v.Id,
                    Plate = $"{v.LicensePlate} - {v.Brand}"
                }).ToList();

                return View(model);
            }

            try
            {
                var monthlyMembershipId = await _mediator.Send(new CreateMonthlyPassCommand
                {
                    CustomerId = model.CustomerId,
                    VehicleId = model.VehicleId,
                    StartDate = model.StartDate,
                    RateId = 1
                });

                TempData["CustomerId"] = model.CustomerId;
                TempData["MonthlyMembershipId"] = monthlyMembershipId;

                return RedirectToAction("Create", "Payments");
            }
            catch (BusinessException ex)
            {
                var vehicles = await _vehicleRepository.GetByCustomerIdAsync(model.CustomerId);
                model.Vehicles = vehicles.Select(v => new VehicleViewModel
                {
                    Id = v.Id,
                    Plate = $"{v.LicensePlate} - {v.Brand}"
                }).ToList();

                _notyf.Error(ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Administrador,Cliente,Operario")]
        public async Task<IActionResult> MonthlyPassStatus(int? customerId)
        {
            IQueryable<MonthlyPass> query = _context.MonthlyPasses
                .Include(m => m.Customer)
                .Include(m => m.Vehicle)
                .Include(m => m.Rate);

            if (User.IsInRole("Cliente"))
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null) return Unauthorized();

                var customer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.ApplicationUserId == user.Id);

                if (customer == null) return NotFound("No se encontró el cliente asociado al usuario.");

                query = query.Where(m => m.CustomerId == customer.Id);
            }
            else if (customerId.HasValue)
            {
                query = query.Where(m => m.CustomerId == customerId.Value);
            }

            var data = await query
                .OrderByDescending(m => m.StartDate)
                .Select(m => new MonthlyPassStatusViewModel
                {
                    Id = m.Id,
                    CustomerName = m.Customer.FirstName + " " + m.Customer.LastName,
                    VehiclePlate = m.Vehicle.LicensePlate,
                    VehicleBrand = m.Vehicle.Brand,
                    Status = m.Status,
                    StartDate = m.StartDate,
                    EndDate = m.EndDate,
                    RateInfo = m.Rate.VehicleType + " - " + m.Rate.Modality + " - $" + m.Rate.Value
                })
                .ToListAsync();

            return View(data);
        }
    }
}