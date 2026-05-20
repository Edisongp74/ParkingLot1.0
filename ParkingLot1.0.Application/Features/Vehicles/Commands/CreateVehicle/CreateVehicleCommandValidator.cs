using FluentValidation;

namespace ParkingLot1._0.Application.Features.Vehicles.Commands.CreateVehicle
{
    // Validador para el comando de crear vehiculo
    public class CreateVehicleCommandValidator : AbstractValidator<CreateVehicleCommand>
    {
        public CreateVehicleCommandValidator()
        {
            RuleFor(x => x.LicensePlate)
                .NotEmpty().WithMessage("La placa es obligatoria")
                .MinimumLength(5).WithMessage("La placa debe tener al menos 5 caracteres")
                .MaximumLength(10).WithMessage("La placa no puede exceder 10 caracteres");

            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("El tipo de vehiculo es obligatorio");

            RuleFor(x => x.Brand)
                .MaximumLength(30).WithMessage("La marca no puede exceder 30 caracteres");

            RuleFor(x => x.Color)
                .MaximumLength(20).WithMessage("El color no puede exceder 20 caracteres");

            RuleFor(x => x.CustomerId)
                .GreaterThan(0).WithMessage("Debe seleccionar un cliente valido");
        }
    }
}
