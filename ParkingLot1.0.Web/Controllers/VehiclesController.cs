using ParkingLot1._0.Application.SimpleMediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ParkingLot1._0.Application.Features.Customers.Queries.GetAllCustomers;
using ParkingLot1._0.Application.Features.Vehicles.Commands.CreateVehicle;
using ParkingLot1._0.Application.Features.Vehicles.Commands.DeleteVehicle;
using ParkingLot1._0.Application.Features.Vehicles.Commands.UpdateVehicle;
using ParkingLot1._0.Application.Features.Vehicles.Queries.GetAllVehicles;
using ParkingLot1._0.Application.Features.Vehicles.Queries.GetVehicleById;
using ParkingLot1._0.Application.Exceptions;
using ParkingLot1._0.Domain.Exceptions;
using ParkingLot1._0.Web.DTOs.Vehicles;
using AspNetCoreHero.ToastNotification.Abstractions;
using ParkingLot1._0.Persistence.Contexts;
using ParkingLot1._0.Domain.Entities;
using ParkingLot1._0.Application.Common;

namespace ParkingLot1._0.Web.Controllers
{
    [Authorize]
    public class VehiclesController : Controller
    {
        private readonly IMediator _mediator;
        private readonly INotyfService _notyf;
        private readonly ApplicationDbContext _context;

        public VehiclesController(IMediator mediator, INotyfService notyf, ApplicationDbContext context)
        {
            _mediator = mediator;
            _notyf = notyf;
            _context = context;
        }

        // GET: Vehicles
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 5)
        {
            // Traemos los vehículos desde el Query Handler
            var vehicles = await _mediator.Send(new GetAllVehiclesQuery());

            // Convertimos la colección a un formato genérico consultable
            var vehiclesQuery = vehicles.Cast<object>().AsQueryable();

            // Creamos una única lista pagiada limpia de tipo object
            var pagedVehicles = PagedList<object>.Create(vehiclesQuery, pageNumber, pageSize);

            return View(pagedVehicles);
        }

        public async Task<IActionResult> Create()
        {
            var customers = await _mediator.Send(new GetAllCustomersQuery());
            ViewBag.Customers = new SelectList(customers, "Id", "FirstName");
            return View(new CreateVehicleDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateVehicleDto dto)
        {
            if (!ModelState.IsValid)
            {
                _notyf.Error("Hay errores de validacion en el formulario");
                var customers = await _mediator.Send(new GetAllCustomersQuery());
                ViewBag.Customers = new SelectList(customers, "Id", "FirstName");
                return View(dto);
            }

            try
            {
                var command = new CreateVehicleCommand
                {
                    LicensePlate = dto.LicensePlate,
                    Type = dto.Type,
                    Brand = dto.Brand,
                    Color = dto.Color,
                    CustomerId = dto.CustomerId
                };

                await _mediator.Send(command);

                // --- LOG DE AUDITORÍA ---
                _context.AuditLogs.Add(new AuditLog
                {
                    Usuario = User.Identity?.Name ?? "Anónimo",
                    Accion = "Crear",
                    Detalle = $"Se registró el vehículo con Placa: {dto.LicensePlate}, Marca: {dto.Brand}, Color: {dto.Color}",
                    ControllerName = "Vehicles",
                    ActionName = "Create",
                    IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1",
                    FechaRegistro = DateTime.Now
                });
                await _context.SaveChangesAsync();
                // -------------------------

                _notyf.Success("Vehiculo creado exitosamente");
                return RedirectToAction(nameof(Index));
            }
            catch (CustomValidationException ex)
            {
                _notyf.Error(string.Join(", ", ex.Errors));
                var customers = await _mediator.Send(new GetAllCustomersQuery());
                ViewBag.Customers = new SelectList(customers, "Id", "FirstName");
                return View(dto);
            }
            catch (BusinessException ex)
            {
                _notyf.Error(ex.Message);
                var customers = await _mediator.Send(new GetAllCustomersQuery());
                ViewBag.Customers = new SelectList(customers, "Id", "FirstName");
                return View(dto);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var vehicle = await _mediator.Send(new GetVehicleByIdQuery { Id = id });

            if (vehicle == null)
            {
                return NotFound();
            }

            var dto = new UpdateVehicleDto
            {
                Id = vehicle.Id,
                LicensePlate = vehicle.LicensePlate,
                Type = vehicle.Type,
                Brand = vehicle.Brand,
                Color = vehicle.Color,
                CustomerId = vehicle.CustomerId
            };

            var customers = await _mediator.Send(new GetAllCustomersQuery());
            ViewBag.Customers = new SelectList(customers, "Id", "FirstName", vehicle.CustomerId);

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateVehicleDto dto)
        {
            if (id != dto.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                _notyf.Error("Hay errores de validacion en el formulario");
                var customers = await _mediator.Send(new GetAllCustomersQuery());
                ViewBag.Customers = new SelectList(customers, "Id", "FirstName", dto.CustomerId);
                return View(dto);
            }

            try
            {
                var command = new UpdateVehicleCommand
                {
                    Id = dto.Id,
                    LicensePlate = dto.LicensePlate,
                    Type = dto.Type,
                    Brand = dto.Brand,
                    Color = dto.Color,
                    CustomerId = dto.CustomerId
                };

                await _mediator.Send(command);

                // --- LOG DE AUDITORÍA ---
                _context.AuditLogs.Add(new AuditLog
                {
                    Usuario = User.Identity?.Name ?? "Anónimo",
                    Accion = "Modificar",
                    Detalle = $"Se actualizaron los datos del vehículo ID {dto.Id}. Nueva Placa: {dto.LicensePlate}",
                    ControllerName = "Vehicles",
                    ActionName = "Edit",
                    IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1",
                    FechaRegistro = DateTime.Now
                });
                await _context.SaveChangesAsync();
                // -------------------------

                _notyf.Success("Vehiculo actualizado exitosamente");
                return RedirectToAction(nameof(Index));
            }
            catch (CustomValidationException ex)
            {
                _notyf.Error(string.Join(", ", ex.Errors));
                var customers = await _mediator.Send(new GetAllCustomersQuery());
                ViewBag.Customers = new SelectList(customers, "Id", "FirstName", dto.CustomerId);
                return View(dto);
            }
            catch (BusinessException ex)
            {
                _notyf.Error(ex.Message);
                var customers = await _mediator.Send(new GetAllCustomersQuery());
                ViewBag.Customers = new SelectList(customers, "Id", "FirstName", dto.CustomerId);
                return View(dto);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var vehicle = await _mediator.Send(new GetVehicleByIdQuery { Id = id });

            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var vehicle = await _mediator.Send(new GetVehicleByIdQuery { Id = id });
                string plate = vehicle != null ? vehicle.LicensePlate : id.ToString();

                await _mediator.Send(new DeleteVehicleCommand { Id = id });

                // --- LOG DE AUDITORÍA ---
                _context.AuditLogs.Add(new AuditLog
                {
                    Usuario = User.Identity?.Name ?? "Anónimo",
                    Accion = "Eliminar",
                    Detalle = $"Se eliminó el vehículo con Placa: {plate} (ID: {id})",
                    ControllerName = "Vehicles",
                    ActionName = "Delete",
                    IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1",
                    FechaRegistro = DateTime.Now
                });
                await _context.SaveChangesAsync();
                // -------------------------

                _notyf.Success("Vehiculo eliminado exitosamente");
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
        }
    }
}
