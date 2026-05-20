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

namespace ParkingLot1._0.Web.Controllers
{
    // Controlador para manejar las operaciones CRUD de vehiculos
    [Authorize]
    public class VehiclesController : Controller
    {
        private readonly IMediator _mediator;
        private readonly INotyfService _notyf;

        // Inyecto el mediador y el servicio de notificaciones
        public VehiclesController(IMediator mediator, INotyfService notyf)
        {
            _mediator = mediator;
            _notyf = notyf;
        }

        // Listo todos los vehiculos
        public async Task<IActionResult> Index()
        {
            var vehicles = await _mediator.Send(new GetAllVehiclesQuery());
            return View(vehicles);
        }

        // Muestro el formulario para crear un vehiculo
        public async Task<IActionResult> Create()
        {
            // Cargo la lista de clientes para el dropdown
            var customers = await _mediator.Send(new GetAllCustomersQuery());
            ViewBag.Customers = new SelectList(customers, "Id", "FirstName");

            return View(new CreateVehicleDto());
        }

        // Recibo los datos del formulario y creo el vehiculo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateVehicleDto dto)
        {
            if (!ModelState.IsValid)
            {
                _notyf.Error("Hay errores de validacion en el formulario");

                // Si hay error, recargo la lista de clientes
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

        // Muestro el formulario para editar un vehiculo
        public async Task<IActionResult> Edit(int id)
        {
            var vehicle = await _mediator.Send(new GetVehicleByIdQuery { Id = id });

            if (vehicle == null)
            {
                return NotFound();
            }

            // Mapeo los datos del vehiculo al DTO de actualizacion
            var dto = new UpdateVehicleDto
            {
                Id = vehicle.Id,
                LicensePlate = vehicle.LicensePlate,
                Type = vehicle.Type,
                Brand = vehicle.Brand,
                Color = vehicle.Color,
                CustomerId = vehicle.CustomerId
            };

            // Cargo la lista de clientes para el dropdown
            var customers = await _mediator.Send(new GetAllCustomersQuery());

            ViewBag.Customers = new SelectList(
                customers,
                "Id",
                "FirstName",
                vehicle.CustomerId
            );

            return View(dto);
        }

        // Recibo los datos del formulario y actualizo el vehiculo
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

        // Muestro la vista de confirmacion para eliminar un vehiculo
        public async Task<IActionResult> Delete(int id)
        {
            var vehicle = await _mediator.Send(new GetVehicleByIdQuery { Id = id });

            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // Confirmo la eliminacion del vehiculo
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _mediator.Send(new DeleteVehicleCommand { Id = id });

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
