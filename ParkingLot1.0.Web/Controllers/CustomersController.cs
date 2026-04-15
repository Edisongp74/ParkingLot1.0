using Microsoft.AspNetCore.Mvc;
using MediatR;
using ParkingLot1._0.Application.Features.Customers.Commands.CreateCustomer;
using ParkingLot1._0.Application.Features.Customers.Commands.UpdateCustomer;
using ParkingLot1._0.Application.Features.Customers.Commands.DeleteCustomer;
using ParkingLot1._0.Application.Features.Customers.Queries.GetAllCustomers;
using ParkingLot1._0.Application.Features.Customers.Queries.GetCustomerById;

namespace ParkingLot1._0.Web.Controllers
{
    // Controlador para manejar las operaciones CRUD de clientes
    public class CustomersController : Controller
    {
        private readonly IMediator _mediator;

        // Inyecto MediatR para enviar los commands y queries
        public CustomersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Listo todos los clientes
        public async Task<IActionResult> Index()
        {
            var customers = await _mediator.Send(new GetAllCustomersQuery());
            return View(customers);
        }

        // Muestro el formulario para crear un cliente
        public IActionResult Create()
        {
            return View();
        }

        // Recibo los datos del formulario y creo el cliente
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCustomerCommand command)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(command);
                return RedirectToAction(nameof(Index));
            }
            return View(command);
        }

        // Muestro el formulario para editar un cliente
        public async Task<IActionResult> Edit(int id)
        {
            var customer = await _mediator.Send(new GetCustomerByIdQuery { Id = id });
            if (customer == null)
            {
                return NotFound();
            }

            // Mapeo los datos del cliente al comando de actualizacion
            var command = new UpdateCustomerCommand
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                DocumentNumber = customer.DocumentNumber,
                DocumentType = customer.DocumentType,
                Phone = customer.Phone,
                CustomerType = customer.CustomerType
            };

            return View(command);
        }

        // Recibo los datos del formulario y actualizo el cliente
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateCustomerCommand command)
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
            return View(command);
        }

        // Muestro la vista de confirmacion para eliminar un cliente
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _mediator.Send(new GetCustomerByIdQuery { Id = id });
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // Confirmo la eliminacion del cliente
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _mediator.Send(new DeleteCustomerCommand { Id = id });
            return RedirectToAction(nameof(Index));
        }
    }
}
