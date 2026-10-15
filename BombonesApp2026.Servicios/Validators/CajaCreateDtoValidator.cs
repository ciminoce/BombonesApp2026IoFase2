using BombonesApp2026.Servicios.DTOs.Caja;
using FluentValidation;

namespace BombonesApp2026.Servicios.Validators
{
    public class CajaCreateDtoValidator : AbstractValidator<CajaCreateDto>
    {
        public CajaCreateDtoValidator()
        {
            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres.");

            RuleFor(x => x.Descripcion)
                .MaximumLength(250).WithMessage("La descripción no puede superar los 200 caracteres.");

            RuleFor(x => x.Stock)
                .NotEmpty().WithMessage("El stock es obligatorio.")
                .GreaterThanOrEqualTo(0).WithMessage("El stock debe ser un valor no negativo.");

        }
    }
}
