using FluentValidation;
using MinhaBiblioteca.Core.Validations.FluentValidation;

namespace MinhaBiblioteca.Api.Configuration;

public static class FluentValidationConfig
{
    public static WebApplicationBuilder AddFluentValidationConfig(this WebApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssemblyContaining<LivroValidation>();

        return builder;
    }
}
