using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MediatR;
using ParkingLot1._0.Application.Features.Vehicles.Commands.CreateVehicle;
using ParkingLot1._0.Application.Features.Vehicles.Commands.UpdateVehicle;
using ParkingLot1._0.Application.Features.Vehicles.Commands.DeleteVehicle;
using ParkingLot1._0.Application.Features.Vehicles.Queries.GetAllVehicles;
using ParkingLot1._0.Application.Features.Vehicles.Queries.GetVehicleById;
using ParkingLot1._0.Application.Features.Customers.Queries.GetAllCustomers;

namespace ParkingLot1._0.Web.Controllers
{
    // Controlador para manejar las operaciones CRUD de vehiculos
    public class VehiclesController : Controller
    {
        private readonly IMediator _mediator;

        // Inyecto MediatR para enviar los commands y queries
        public VehiclesController(IMediator mediator)
        {
            _mediator = mediator;
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
            return View();
        }

        // Recibo los datos del formulario y creo el vehiculo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateVehicleCommand command)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(command);
                return RedirectToAction(nameof(Index));
            }

            // Si hay error, recargo la lista de clientes
            var customers = await _mediator.Send(new GetAllCustomersQuery());
            ViewBag.Customers = new SelectList(customers, "Id", "FirstName");
            return View(command);
        }

        // Muestro el formulario para editar un vehiculo
        public async Task<IActionResult> Edit(int id)
        {
            var vehicle = await _mediator.Send(new GetVehicleByIdQuery { Id = id });
            if (vehicle == null)
            {
                return NotFound();
            }

            // Mapeo los datos del vehiculo al comando de actualizacion
            var command = new UpdateVehicleCommand
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
            ViewBag.Customers = new SelectList(customers, "Id", "FirstName", vehicle.CustomerId);
            return View(command);
        }

        // Recibo los datos del formulario y actualizo el vehiculo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateVehicleCommand command)
        {
            if (id != command.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _mediator.Send(command);
                return RedirectToAction(nameof(Index));
            }

            // Si hay error, recargo la lista de clientes
            var customers = await _mediator.Send(new GetAllCustomersQuery());
            ViewBag.Customers = new SelectList(customers, "Id", "FirstName", command.CustomerId);
            return View(command);
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
            await _mediator.Send(new DeleteVehicleCommand { Id = id });
            return RedirectToAction(nameof(Index));
        }
    }
}
