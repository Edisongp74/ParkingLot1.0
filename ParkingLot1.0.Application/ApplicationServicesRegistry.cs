using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ParkingLot1._0.Application.SimpleMediator;

namespace ParkingLot1._0.Application
{
    // Extension method para registrar todos los servicios de la capa Application
    public static class ApplicationServicesRegistry
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Registro el mediador como Transient (como el profesor)
            services.AddTransient<IMediator, SimpleMediator.SimpleMediator>();

            // Uso Scrutor para auto-escanear y registrar todos los handlers y validators
            var assembly = typeof(ApplicationServicesRegistry).Assembly;

            // Registro handlers con respuesta: IRequestHandler<TRequest, TResponse>
            services.Scan(scan => scan
                .FromAssemblies(assembly)
                .AddClasses(classes => classes.AssignableTo(typeof(IRequestHandler<,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            );

            // Registro handlers sin respuesta: IRequestHandler<TRequest>
            services.Scan(scan => scan
                .FromAssemblies(assembly)
                .AddClasses(classes => classes.AssignableTo(typeof(IRequestHandler<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            );

            // Registro validators de FluentValidation: IValidator<T>
            services.Scan(scan => scan
                .FromAssemblies(assembly)
                .AddClasses(classes => classes.AssignableTo(typeof(IValidator<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            );

            return services;
        }
    }
}
