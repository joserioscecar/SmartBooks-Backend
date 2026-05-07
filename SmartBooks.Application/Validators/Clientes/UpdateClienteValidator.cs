using FluentValidation;
using SmartBooks.Application.DTOs.Clientes;

namespace SmartBooks.Application.Validators.Clientes
{
    public class UpdateClienteValidator : AbstractValidator<UpdateClienteDto>
    {
        public UpdateClienteValidator()
        {
            RuleFor(x => x.Nombres)
                .NotEmpty().WithMessage("El nombre es obligatorio.");

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress().WithMessage("Correo electrónico no válido.");

            RuleFor(x => x.Celular)
                .Matches(@"^\d{10}$").WithMessage("El celular debe tener exactamente 10 dígitos.");

        }
    }
}
