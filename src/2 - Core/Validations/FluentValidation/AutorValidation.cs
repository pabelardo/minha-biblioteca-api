using FluentValidation;
using MinhaBiblioteca.Core.DTOs;

namespace MinhaBiblioteca.Core.Validations.FluentValidation;

public class AutorValidation : AbstractValidator<AutorDto>
{
    public AutorValidation()
    {
        RuleFor(f => f.Nome)
            .NotEmpty()
            .WithMessage("O campo {PropertyName} é de preenchimento obrigatório")
            .Length(2, 100)
            .WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

        RuleFor(f => f.Email)
            .NotEmpty()
            .WithMessage("O campo {PropertyName} é de preenchimento obrigatório")
            .Length(2, 100)
            .WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres")
            .EmailAddress();

        RuleFor(f => f.Telefone)
            .NotEmpty()
            .WithMessage("O campo {PropertyName} é de preenchimento obrigatório")
            .Length(2, 100)
            .WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");
    }
}