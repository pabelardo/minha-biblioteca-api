using FluentValidation;
using MinhaBiblioteca.Core.Requests.Books;

namespace MinhaBiblioteca.Api.Validations.FluentValidation.Books;

public class CreateBookValidation : AbstractValidator<CreateBookRequest>
{
    public CreateBookValidation()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .WithMessage("O campo {PropertyName} é de preenchimento obrigatório")
            .Length(2, 100)
            .WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

        RuleFor(c => c.AuthorId)
            .NotEmpty()
            .WithMessage("O campo {PropertyName} é de preenchimento obrigatório");

        RuleFor(c => c.GenreId)
            .NotEmpty()
            .WithMessage("O campo {PropertyName} é de preenchimento obrigatório");
    }
}