using FluentValidation;
using MinhaBiblioteca.Core.Requests.Authors;

namespace MinhaBiblioteca.Api.Validations.FluentValidation.Authors;

public class UpdateAuthorValidation : AbstractValidator<UpdateAuthorRequest>
{
    public UpdateAuthorValidation()
    {
        RuleFor(f => f.Id)
            .NotEmpty()
            .WithMessage("O campo {PropertyName} é de preenchimento obrigatório");

        RuleFor(f => f.Name)
            .NotEmpty()
            .WithMessage("O campo {PropertyName} é de preenchimento obrigatório")
            .Length(2, 100)
            .WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

        RuleFor(f => f.Email)
            .Length(2, 100)
            .WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres")
            .EmailAddress()
            .When(a => !string.IsNullOrWhiteSpace(a.Email));

        RuleFor(f => f.Phone)
            .Length(2, 100)
            .WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres")
            .When(a => !string.IsNullOrWhiteSpace(a.Phone));
    }
}