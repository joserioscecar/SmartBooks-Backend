using FluentValidation;
using SmartBooks.Application.DTOs.Libros;

namespace SmartBooks.Application.Validators;

public class CreateLibroValidator : AbstractValidator<CreateLibroDto>
{
    public CreateLibroValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre del libro es obligatorio.");

        RuleFor(x => x.Nivel)
            .NotEmpty().WithMessage("El nivel es obligatorio.");

        RuleFor(x => (int)x.Tipo)
            .InclusiveBetween(1, 2).WithMessage("El tipo debe ser 1 (Student's Book) o 2 (Workbook).");


        RuleFor(x => x.Edicion)
            .NotEmpty().WithMessage("La edición es obligatoria.");
    }
}
