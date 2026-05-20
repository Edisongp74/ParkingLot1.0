using FluentValidation;
using ParkingLot1._0.Application.Exceptions;

namespace ParkingLot1._0.Application.SimpleMediator
{
    // Implementacion del mediador que valida requests antes de ejecutar handlers
    public class SimpleMediator : IMediator
    {
        private readonly IServiceProvider _serviceProvider;

        public SimpleMediator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        // Envio un request con respuesta: valido y luego ejecuto el handler
        public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request)
        {
            // Valido el request antes de ejecutar el handler
            await ValidateRequestAsync(request);

            var requestType = request.GetType();
            var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(TResponse));

            var handler = _serviceProvider.GetService(handlerType);

            if (handler == null)
            {
                throw new MediatorException(
                    $"No se encontro un handler para el request de tipo {requestType.Name}");
            }

            var method = handlerType.GetMethod("Handle");
            var result = await (Task<TResponse>)method!.Invoke(handler, new object[] { request })!;

            return result;
        }

        // Envio un request sin respuesta: valido y luego ejecuto el handler
        public async Task Send(IRequest request)
        {
            // Valido el request antes de ejecutar el handler
            await ValidateRequestAsync(request);

            var requestType = request.GetType();
            var handlerType = typeof(IRequestHandler<>).MakeGenericType(requestType);

            var handler = _serviceProvider.GetService(handlerType);

            if (handler == null)
            {
                throw new MediatorException(
                    $"No se encontro un handler para el request de tipo {requestType.Name}");
            }

            var method = handlerType.GetMethod("Handle");
            await (Task)method!.Invoke(handler, new object[] { request })!;
        }

        // Busco un IValidator<T> registrado via reflexion y valido el request
        private async Task ValidateRequestAsync<T>(T request)
        {
            var validatorType = typeof(IValidator<>).MakeGenericType(request!.GetType());
            var validator = _serviceProvider.GetService(validatorType);

            if (validator != null)
            {
                var validateMethod = validatorType.GetMethod("ValidateAsync",
                    new[] { request.GetType(), typeof(CancellationToken) });

                var validationResult = await (Task<FluentValidation.Results.ValidationResult>)
                    validateMethod!.Invoke(validator, new object[] { request, CancellationToken.None })!;

                if (!validationResult.IsValid)
                {
                    throw new CustomValidationException(validationResult);
                }
            }
        }
    }
}
