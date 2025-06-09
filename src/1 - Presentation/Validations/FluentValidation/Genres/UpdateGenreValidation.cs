using FluentValidation;
using MinhaBiblioteca.Core.Requests.Genres;

namespace MinhaBiblioteca.Api.Validations.FluentValidation.Genres;

public class UpdateGenreValidation : AbstractValidator<UpdateGenreRequest>
{
    public UpdateGenreValidation()
    {
        RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("O campo {PropertyName} é de preenchimento obrigatório");

        RuleFor(c => c.Name)
            .NotEmpty()
            .WithMessage("O campo {PropertyName} é de preenchimento obrigatório")
            .Length(2, 100)
            .WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

        RuleFor(c => c.Description)
            .Length(2, 500)
            .WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres")
            .When(g => !string.IsNullOrWhiteSpace(g.Description));
    }
}