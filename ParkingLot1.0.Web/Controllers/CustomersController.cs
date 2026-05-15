using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkingLot1._0.Application.Features.Customers.Commands.CreateCustomer;
using ParkingLot1._0.Application.Features.Customers.Commands.DeleteCustomer;
using ParkingLot1._0.Application.Features.Customers.Commands.UpdateCustomer;
using ParkingLot1._0.Application.Features.Customers.Queries.GetAllCustomers;
using ParkingLot1._0.Application.Features.Customers.Queries.GetCustomerById;
using ParkingLot1._0.Application.Features.MonthlyPasses.Commands.CreateMonthlyPass;
using ParkingLot1._0.Web.Models;

namespace ParkingLot1._0.Web.Controllers
{

    public class CustomersController : Controller
    {
        private readonly IMediator _mediator;

        public CustomersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            var customers = await _mediator.Send(new GetAllCustomersQuery());

            // Dentro del Select del Index:
            var viewModel = customers.Select(c => new CustomerViewModel
            {
                Id = c.Id,
                FullName = $"{c.FirstName} {c.LastName}",
                DocumentNumber = c.DocumentNumber,
                Phone = c.Phone,
                TotalVehicles = c.Vehicles?.Count ?? 0,
                HasActivePass = c.HasActiveMonthlyPass()
            }).ToList();

            // 3. Envía la lista de ViewModels a la Vista
            return View(viewModel);
        }

        public IActionResult Create()
        {
            return View();
        }


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


        public async Task<IActionResult> Edit(int id)
        {
            var customer = await _mediator.Send(new GetCustomerByIdQuery { Id = id });
            if (customer == null)
            {
                return NotFound();
            }


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
            await _mediator.Send(new DeleteCustomerCommand { Id = id });
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> BuyPass(int id)
        {
            var customer = await _mediator.Send(new GetCustomerByIdQuery { Id = id });

            if (customer == null) return NotFound();

            var viewModel = new BuyMonthlyPassViewModel
            {
                CustomerId = customer.Id,
                CustomerName = customer.FullName,

                Vehicles = customer.Vehicles.Select(v => new VehicleViewModel
                {
                    Id = v.Id,
                    Plate = v.LicensePlate
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> BuyPass(BuyMonthlyPassViewModel model)
        {
            if (ModelState.IsValid)
            {
     
                await _mediator.Send(new CreateMonthlyPassCommand
                {
                    CustomerId = model.CustomerId,
                    VehicleId = model.VehicleId,
                    StartDate = model.StartDate,
                    RateId = 1 // ID de la tarifa mensual por defecto
                });

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
    }
}
