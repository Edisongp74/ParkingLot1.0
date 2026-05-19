using MediatR;
using Microsoft.AspNetCore.Mvc;
using ParkingLot1._0.Application.Features.Sections.Commands.CreateSection;
using ParkingLot1._0.Application.Features.Sections.Queries.GetSectionsList;
using System.Threading.Tasks;

namespace ParkingLot1._0.Web.Controllers
{
    public class SectionsController : Controller
    {
        private readonly IMediator _mediator;

        public SectionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Index([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5)
        {
            var query = new GetSectionsListQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await _mediator.Send(query);

            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateSectionCommand command)
        {
            if (!ModelState.IsValid)
            {
                return View(command);
            }
            await _mediator.Send(command);

            return RedirectToAction(nameof(Index));
        }
    }
}