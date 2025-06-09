using FluentValidation;
using MinhaBiblioteca.Core.Requests.Authors;

namespace MinhaBiblioteca.Api.Validations.FluentValidation.Authors;

public class CreateAuthorValidation : AbstractValidator<CreateAuthorRequest>
{
    public CreateAuthorValidation()
    {
        RuleFor(a => a.Name)
            .NotEmpty()
            .WithMessage("O campo {PropertyName} é de preenchimento obrigatório")
            .Length(2, 100)
            .WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

        RuleFor(a => a.Email)
            .Length(2, 100)
            .WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres")
            .EmailAddress()
            .WithMessage("O campo {PropertyName} precisa ser um endereço de e-mail válido")
            .When(a => !string.IsNullOrWhiteSpace(a.Email));

        RuleFor(a => a.Phone)
            .Length(2, 15)
            .WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres")
            .When(a => !string.IsNullOrWhiteSpace(a.Phone));
    }
}