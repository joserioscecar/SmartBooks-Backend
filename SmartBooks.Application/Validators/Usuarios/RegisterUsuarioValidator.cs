using FluentValidation;
using SmartBooks.Application.DTOs.Usuarios;

namespace SmartBooks.Application.Validators.Usuarios;

public class RegisterUsuarioValidator : AbstractValidator<RegisterUsuarioDto>
{
    public RegisterUsuarioValidator()
    {
        RuleFor(x => x.Identificacion).NotEmpty();
        RuleFor(x => x.Nombres).NotEmpty();

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .WithMessage("La contraseña debe tener al menos 8 caracteres.");
    }
}
