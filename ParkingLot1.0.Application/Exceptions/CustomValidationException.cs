using FluentValidation.Results;

namespace ParkingLot1._0.Application.Exceptions
{
    // Excepcion personalizada para errores de validacion
    public class CustomValidationException : Exception
    {
        public List<string> Errors { get; }

        // Constructor que recibe un ValidationResult de FluentValidation
        public CustomValidationException(ValidationResult validationResult)
            : base("Se han producido errores de validacion")
        {
            Errors = validationResult.Errors
                .Select(e => e.ErrorMessage)
                .ToList();
        }

        // Constructor que recibe un mensaje de error simple
        public CustomValidationException(string message)
            : base(message)
        {
            Errors = new List<string> { message };
        }
    }
}
