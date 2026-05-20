using FluentValidation;

namespace ParkingLot1._0.Application.Features.Sections.Commands.CreateSection
{
    // Validador para el comando de crear seccion
    public class CreateSectionCommandValidator : AbstractValidator<CreateSectionCommand>
    {
        public CreateSectionCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre de la seccion es obligatorio")
                .MaximumLength(50).WithMessage("El nombre no puede exceder 50 caracteres");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("La descripcion es obligatoria")
                .MaximumLength(200).WithMessage("La descripcion no puede exceder 200 caracteres");
        }
    }
}
