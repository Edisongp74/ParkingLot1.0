using ParkingLot1._0.Application.SimpleMediator;
using Microsoft.AspNetCore.Mvc;
using ParkingLot1._0.Application.Features.Sections.Commands.CreateSection;
using ParkingLot1._0.Application.SimpleMediator;
using Microsoft.AspNetCore.Mvc;
using ParkingLot1._0.Application.Features.Sections.Commands.CreateSection;
using ParkingLot1._0.Application.Features.Sections.Queries.GetSectionsList;
using AspNetCoreHero.ToastNotification.Abstractions;
using ParkingLot1._0.Persistence.Contexts;
using ParkingLot1._0.Domain.Entities;
using Microsoft.AspNetCore.Authorization;

namespace ParkingLot1._0.Web.Controllers
{
    [Authorize(Roles = "Administrador")]
    [Authorize(Roles = "Administrador, Operador")]
    public class SectionsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly INotyfService _notyf;
        private readonly ApplicationDbContext _context; // Añadido para los logs

        public SectionsController(IMediator mediator, INotyfService notyf, ApplicationDbContext context)
        {
            _mediator = mediator;
            _notyf = notyf;
            _context = context;
        }

        public async Task<IActionResult> Index([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 5)
        {
            var query = new GetSectionsListQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await _mediator.Send(query);

            // Pasamos el resultado directo porque tu backend ya se encarga de paginarlo
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

            try
            {
                await _mediator.Send(command);

                // --- LOG DE AUDITORÍA ---
                _context.AuditLogs.Add(new AuditLog
                {
                    Usuario = User.Identity?.Name ?? "Anónimo",
                    Accion = "Crear",
                    Detalle = $"Se creó una nueva sección: {command.Name}.",
                    ControllerName = "Sections",
                    ActionName = "Create",
                    IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1",
                    FechaRegistro = DateTime.Now
                });
                await _context.SaveChangesAsync();
                // -------------------------

                _notyf.Success("Seccion creada exitosamente");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _notyf.Error($"Error al crear la sección: {ex.Message}");
                return View(command);
            }
        }
    }
}