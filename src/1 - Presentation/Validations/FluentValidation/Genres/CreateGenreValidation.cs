using FluentValidation;
using MinhaBiblioteca.Core.Requests.Genres;

namespace MinhaBiblioteca.Api.Validations.FluentValidation;

public class CreateGenreValidation : AbstractValidator<CreateGenreRequest>
{
    public CreateGenreValidation()
    {
        RuleFor(g => g.Name)
            .NotEmpty()
            .WithMessage("O campo {PropertyName} é de preenchimento obrigatório")
            .Length(2, 100)
            .WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

        RuleFor(g => g.Description)
            .Length(2, 500)
            .WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres")
            .When(g => !string.IsNullOrWhiteSpace(g.Description));
    }
}