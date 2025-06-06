using FluentValidation;
using MinhaBiblioteca.Core.DTOs;

namespace MinhaBiblioteca.Core.Validations.FluentValidation;

public class LivroValidation : AbstractValidator<LivroDto>
{
    public LivroValidation()
    {
        RuleFor(c => c.Nome)
            .NotEmpty()
            .WithMessage("O campo {PropertyName} é de preenchimento obrigatório")
            .Length(2, 100)
            .WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

        RuleFor(c => c.DataCadastro)
            .NotEmpty()
            .WithMessage("O campo {PropertyName} é de preenchimento obrigatório");

        RuleFor(c => c.AutorId)
            .NotEmpty()
            .WithMessage("O campo {PropertyName} é de preenchimento obrigatório");

        RuleFor(c => c.GeneroId)
            .NotEmpty()
            .WithMessage("O campo {PropertyName} é de preenchimento obrigatório");
    }
}