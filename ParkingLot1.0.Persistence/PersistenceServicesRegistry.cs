using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ParkingLot1._0.Application.Interfaces;
using ParkingLot1._0.Persistence.Contexts;
using ParkingLot1._0.Persistence.Repositories;

namespace ParkingLot1._0.Persistence
{
    // Extension method para registrar todos los servicios de la capa Persistence
    public static class PersistenceServicesRegistry
    {
        public static IServiceCollection AddPersistenceServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Registro el DbContext con SQL Server
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")));

            // Registro los repositorios
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<IMonthlyPassRepository, MonthlyPassRepository>();
            services.AddScoped<ISectionRepository, SectionRepository>();

            return services;
        }
    }
}
