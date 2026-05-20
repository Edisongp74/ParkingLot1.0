using ParkingLot1._0.Application.Exceptions;
using ParkingLot1._0.Domain.Exceptions;

namespace ParkingLot1._0.Web.Middleware
{
    // Middleware global que atrapa excepciones no manejadas y redirige a /Home/Error
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        // Clave de sesion donde se guarda el mensaje de error (como el profesor)
        public const string ERROR_MESSAGE_SESSION_KEY = "ErrorMessage";

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Excepcion no manejada: {Message}", ex.Message);

                // Determino el mensaje segun el tipo de excepcion
                var message = ex switch
                {
                    CustomValidationException validationEx =>
                        $"Errores de validacion: {string.Join(", ", validationEx.Errors)}",

                    BusinessException businessEx =>
                        $"Error de negocio: {businessEx.Message}",

                    NotFoundException notFoundEx =>
                        $"No encontrado: {notFoundEx.Message}",

                    MediatorException mediatorEx =>
                        $"Error interno del mediador: {mediatorEx.Message}",

                    _ => $"Ha ocurrido un error inesperado: {ex.Message}"
                };

                // Intento guardar en Session, si no se puede, uso query string
                try
                {
                    context.Session.SetString(ERROR_MESSAGE_SESSION_KEY, message);
                    context.Response.Redirect("/Home/Error");
                }
                catch
                {
                    context.Response.Redirect("/Home/Error");
                }
            }
        }
    }

    // Extension method para registrar el middleware en el pipeline
    public static class ExceptionHandlerMiddlewareExtension
    {
        public static IApplicationBuilder UseExceptionHandlerMiddleware(
            this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}
