using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ParkingLot1._0.Persistence.Contexts;
using ParkingLot1._0.Web.Models;

namespace ParkingLot1._0.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        // Asume que tu contexto se llama ApplicationDbContext (cámbialo si es necesario)
        private readonly ApplicationDbContext _context; 

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // 1. Contadores para las tarjetas superiores del Dashboard
            ViewBag.TotalClientes = await _context.Customers.CountAsync();
            ViewBag.TotalVehiculos = await _context.Vehicles.CountAsync();

            // 2. Ingresos acumulados del mes actual
            var inicioMes = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            ViewBag.TotalIngresosMes = await _context.Payments
                .Where(p => p.PaidAt >= inicioMes)
                .SumAsync(p => p.Amount);

            // 3. Eje X: Generar las últimas 7 horas dinámicas a partir de la hora actual
            var horaActual = DateTime.Now.Hour;
            var horasGrafica = Enumerable.Range(horaActual - 6, 7)
                .Select(h => (h + 24) % 24)
                .ToList();

            var hoy = DateTime.Today;

            // Traemos los pagos de hoy a memoria para evitar errores de traducción LINQ-to-SQL
            var pagosHoy = await _context.Payments
                .Where(p => p.PaidAt >= hoy)
                .ToListAsync();

            var listaIngresos = new List<decimal>();
            var listaVentas = new List<decimal>();
            var listaClientes = new List<decimal>();

            foreach (var h in horasGrafica)
            {
                var pagosDeLaHora = pagosHoy.Where(p => p.PaidAt.Value.Hour == h).ToList();

                // A. Total dinero de la hora
                listaIngresos.Add(pagosDeLaHora.Sum(p => p.Amount));

                // B. Cantidad de ventas de la hora
                listaVentas.Add((decimal)pagosDeLaHora.Count);

                // C. Cantidad de clientes de la hora (Igualado a transacciones para asegurar consistencia)
                listaClientes.Add((decimal)pagosDeLaHora.Count);
            }

            // Enviamos las listas listas a la vista
            ViewBag.GraficaLabels = horasGrafica.Select(h => $"{h:D2}:00").ToList();
            ViewBag.GraficaVentas = listaVentas;
            ViewBag.GraficaIngresos = listaIngresos;
            ViewBag.GraficaClientes = listaClientes;

            return View();
        }
    }
}