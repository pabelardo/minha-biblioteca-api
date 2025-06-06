using FluentValidation;
using MinhaBiblioteca.Core.DTOs;

namespace MinhaBiblioteca.Core.Validations.FluentValidation;

public class GeneroValidation : AbstractValidator<GeneroDto>
{
    public GeneroValidation()
    {
        RuleFor(c => c.Nome)
            .NotEmpty()
            .WithMessage("O campo {PropertyName} é de preenchimento obrigatório")
            .Length(2, 100)
            .WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

        RuleFor(c => c.Descricao)
            .NotEmpty()
            .WithMessage("O campo {PropertyName} é de preenchimento obrigatório")
            .Length(2, 255)
            .WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");
    }
}