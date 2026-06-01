using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkingLot1._0.Api.DTOs.Customer;
using ParkingLot1._0.Application.Features.Customers.Commands.CreateCustomer;
using ParkingLot1._0.Application.Features.Customers.Commands.DeleteCustomer;
using ParkingLot1._0.Application.Features.Customers.Commands.UpdateCustomer;
using ParkingLot1._0.Application.Features.Customers.Queries.GetAllCustomers;
using ParkingLot1._0.Application.Features.Customers.Queries.GetCustomerById;
using ParkingLot1._0.Application.SimpleMediator;

namespace ParkingLot1._0.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                GetAllCustomersQuery query = new GetAllCustomersQuery();

                var customers = await _mediator.Send(query);

                // Mapeo manual de Customer a CustomerListItemDTO
                List<CustomerListItemDTO> list = customers.Select(c => new CustomerListItemDTO
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    DocumentNumber = c.DocumentNumber,
                    DocumentType = c.DocumentType,
                    Phone = c.Phone,
                    CustomerType = c.CustomerType,
                    FullName = c.FullName
                }).ToList();

                return StatusCode(StatusCodes.Status200OK, list);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                GetCustomerByIdQuery query = new GetCustomerByIdQuery { Id = id };

                var customer = await _mediator.Send(query);

                if (customer == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "Cliente no encontrado");
                }

                CustomerListItemDTO dto = new CustomerListItemDTO
                {
                    Id = customer.Id,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    DocumentNumber = customer.DocumentNumber,
                    DocumentType = customer.DocumentType,
                    Phone = customer.Phone,
                    CustomerType = customer.CustomerType,
                    FullName = customer.FullName
                };

                return StatusCode(StatusCodes.Status200OK, dto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState);
                }

                CreateCustomerCommand command = new CreateCustomerCommand
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    DocumentNumber = dto.DocumentNumber,
                    DocumentType = dto.DocumentType,
                    Phone = dto.Phone,
                    CustomerType = dto.CustomerType
                };

                int newCustomerId = await _mediator.Send(command);

                return StatusCode(StatusCodes.Status201Created, newCustomerId);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] EditCustomerDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, ModelState);
                }

                UpdateCustomerCommand command = new UpdateCustomerCommand
                {
                    Id = id,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    DocumentNumber = dto.DocumentNumber,
                    DocumentType = dto.DocumentType,
                    Phone = dto.Phone,
                    CustomerType = dto.CustomerType
                };

                await _mediator.Send(command);

                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _mediator.Send(new DeleteCustomerCommand { Id = id });
            return StatusCode(StatusCodes.Status204NoContent);
        }

    }
}
