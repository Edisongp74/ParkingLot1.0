using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkingLot1._0.Persistence.Contexts;
using ParkingLot1._0.Web.Middleware;
using ParkingLot1._0.Web.Models;

namespace ParkingLot1._0.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.TotalClientes = _context.Customers.Count();
            ViewBag.TotalVehiculos = _context.Vehicles.Count();

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // Leo el mensaje de error de Session (como el profesor)
            var message = HttpContext.Session.GetString(
                ExceptionHandlerMiddleware.ERROR_MESSAGE_SESSION_KEY);

            // Limpio la sesion
            HttpContext.Session.Remove(ExceptionHandlerMiddleware.ERROR_MESSAGE_SESSION_KEY);

            return View(new ErrorViewModel
            {
                Message = message ?? "Ha ocurrido un error inesperado"
            });
        }
    }
}
