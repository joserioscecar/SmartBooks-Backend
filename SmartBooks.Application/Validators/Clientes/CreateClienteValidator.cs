using FluentValidation;
using SmartBooks.Application.Extensions;
using SmartBooks.Application.DTOs.Clientes;

namespace SmartBooks.Application.Validators.Clientes
{
    public class CreateClienteValidator : AbstractValidator<CreateClienteDto>
    {
        public CreateClienteValidator()
        {
            RuleFor(x => x.Identificacion)
                .NotEmpty().WithMessage("La identificación es obligatoria.");

            RuleFor(x => x.Nombres)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .Must(n => n == StringSanitizer.WithoutDiacritics(n))
                .WithMessage("El nombre no debe tener acentos.");

            RuleFor(x => x.Email)
                .NotEmpty().EmailAddress()
                .WithMessage("Correo electrónico no válido.");

            RuleFor(x => x.Celular)
                .Matches(@"^\d{10}$")
                .WithMessage("El celular debe tener exactamente 10 dígitos.");

        }
    }
}
