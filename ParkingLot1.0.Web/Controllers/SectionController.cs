using ParkingLot1._0.Application.SimpleMediator;
using Microsoft.AspNetCore.Mvc;
using ParkingLot1._0.Application.Features.Sections.Commands.CreateSection;
using ParkingLot1._0.Application.Features.Sections.Queries.GetSectionsList;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace ParkingLot1._0.Web.Controllers
{
    public class SectionsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly INotyfService _notyf;

        public SectionsController(IMediator mediator, INotyfService notyf)
        {
            _mediator = mediator;
            _notyf = notyf;
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
                _notyf.Error("Hay errores de validacion en el formulario");
                return View(command);
            }

            await _mediator.Send(command);

            _notyf.Success("Seccion creada exitosamente");
            return RedirectToAction(nameof(Index));
        }
    }
}
