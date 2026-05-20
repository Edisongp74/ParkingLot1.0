using FluentValidation;

namespace ParkingLot1._0.Application.Features.Customers.Commands.CreateCustomer
{
    // Validador para el comando de crear cliente
    public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("El nombre es obligatorio")
                .MinimumLength(2).WithMessage("El nombre debe tener al menos 2 caracteres")
                .MaximumLength(50).WithMessage("El nombre no puede exceder 50 caracteres");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("El apellido es obligatorio")
                .MinimumLength(2).WithMessage("El apellido debe tener al menos 2 caracteres")
                .MaximumLength(50).WithMessage("El apellido no puede exceder 50 caracteres");

            RuleFor(x => x.DocumentNumber)
                .NotEmpty().WithMessage("El numero de documento es obligatorio")
                .MinimumLength(5).WithMessage("El documento debe tener al menos 5 caracteres")
                .MaximumLength(20).WithMessage("El documento no puede exceder 20 caracteres");

            RuleFor(x => x.DocumentType)
                .NotEmpty().WithMessage("El tipo de documento es obligatorio");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("El telefono es obligatorio")
                .MaximumLength(15).WithMessage("El telefono no puede exceder 15 caracteres");

            RuleFor(x => x.CustomerType)
                .NotEmpty().WithMessage("El tipo de cliente es obligatorio");
        }
    }
}
