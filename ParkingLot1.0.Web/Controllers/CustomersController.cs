using ParkingLot1._0.Application.SimpleMediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkingLot1._0.Application.Features.Customers.Commands.CreateCustomer;
using ParkingLot1._0.Application.Features.Customers.Commands.DeleteCustomer;
using ParkingLot1._0.Application.Features.Customers.Commands.UpdateCustomer;
using ParkingLot1._0.Application.Features.Customers.Queries.GetAllCustomers;
using ParkingLot1._0.Application.Features.Customers.Queries.GetCustomerById;
using ParkingLot1._0.Application.Features.MonthlyPasses.Commands.CreateMonthlyPass;
using ParkingLot1._0.Application.Exceptions;
using ParkingLot1._0.Domain.Exceptions;
using ParkingLot1._0.Web.DTOs.Customers;
using ParkingLot1._0.Web.Models;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace ParkingLot1._0.Web.Controllers
{
    [Authorize]
    public class CustomersController : Controller
    {
        private readonly IMediator _mediator;
        private readonly INotyfService _notyf;

        public CustomersController(IMediator mediator, INotyfService notyf)
        {
            _mediator = mediator;
            _notyf = notyf;
        }

        public async Task<IActionResult> Index()
        {
            var customers = await _mediator.Send(new GetAllCustomersQuery());

            var viewModel = customers.Select(c => new CustomerViewModel
            {
                Id = c.Id,
                FullName = $"{c.FirstName} {c.LastName}",
                DocumentNumber = c.DocumentNumber,
                Phone = c.Phone,
                TotalVehicles = c.Vehicles?.Count ?? 0,
                HasActivePass = c.HasActiveMonthlyPass()
            }).ToList();

            return View(viewModel);
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
                await _mediator.Send(new DeleteCustomerCommand { Id = id });

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
        }

        [HttpGet]
        public async Task<IActionResult> BuyPass(int id)
        {
            var customer = await _mediator.Send(new GetCustomerByIdQuery { Id = id });

            if (customer == null)
            {
                return NotFound();
            }

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
                try
                {
                    await _mediator.Send(new CreateMonthlyPassCommand
                    {
                        CustomerId = model.CustomerId,
                        VehicleId = model.VehicleId,
                        StartDate = model.StartDate,
                        RateId = 1
                    });

                    _notyf.Success("Mensualidad comprada exitosamente");
                    return RedirectToAction(nameof(Index));
                }
                catch (BusinessException ex)
                {
                    _notyf.Error(ex.Message);
                    return View(model);
                }
            }

            return View(model);
        }
    }
}
