using FluentValidation;
using MinhaBiblioteca.Core.Requests.Authors;

namespace MinhaBiblioteca.Api.Validations.FluentValidation.Authors;

public class CreateAuthorValidation : AbstractValidator<CreateAuthorRequest>
{
    public CreateAuthorValidation()
    {
        RuleFor(f => f.Name)
            .NotEmpty()
            .WithMessage("O campo {PropertyName} é de preenchimento obrigatório")
            .Length(2, 100)
            .WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

        RuleFor(f => f.Email)
            .Length(2, 100)
            .WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres")
            .EmailAddress();

        RuleFor(f => f.Phone)
            .Length(2, 100)
            .WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");
    }
}