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
            .NotEmpty()
            .WithMessage("O campo {PropertyName} é de preenchimento obrigatório")
            .Length(2, 100)
            .WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres")
            .EmailAddress();

        RuleFor(f => f.Phone)
            .NotEmpty()
            .WithMessage("O campo {PropertyName} é de preenchimento obrigatório")
            .Length(2, 100)
            .WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");
    }
}