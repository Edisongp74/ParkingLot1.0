using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ParkingLot1._0.Application.Contracts.Repositories;
using ParkingLot1._0.Application.Interfaces;
using ParkingLot1._0.Persistence.Contexts;
using ParkingLot1._0.Persistence.Repositories;

namespace ParkingLot1._0.Persistence
{
    public static class PersistenceServicesRegistry
    {
        public static IServiceCollection AddPersistenceServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<IMonthlyPassRepository, MonthlyPassRepository>();
            services.AddScoped<ISectionRepository, SectionRepository>();
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IRolesRepository, RolesRepository>();

            return services;
        }
    }
}
