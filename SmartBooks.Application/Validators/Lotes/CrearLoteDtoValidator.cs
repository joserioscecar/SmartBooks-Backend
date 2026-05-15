using FluentValidation;
using SmartBooks.Application.DTOs.Lotes;

namespace SmartBooks.Application.Validators.Lotes;

public class CrearLoteDtoValidator : AbstractValidator<CrearLoteDto>
{
    public CrearLoteDtoValidator()
    {
        RuleFor(x => x.Lote)
            .Must(BeValidLote)
            .WithMessage("El lote debe tener el formato YYYY1 o YYYY2. Ejemplo: 20261 o 20262.");
    }

    private bool BeValidLote(int lote)
    {
        var loteStr = lote.ToString();

        // Debe tener exactamente 5 dígitos
        if (loteStr.Length != 5)
            return false;

        // Último dígito debe ser 1 o 2
        var semestre = loteStr[^1];
        if (semestre != '1' && semestre != '2')
            return false;

        // Extraer año
        var year = int.Parse(loteStr.Substring(0, 4));

        return year >= 2000 && year <= 2100;
    }
}