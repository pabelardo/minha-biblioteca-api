using FluentValidation;
using FluentValidation.Results;

namespace MinhaBiblioteca.Api.Extension;

public static class ValidationExtensions
{
    public static async Task<ValidationResult> ObterValidationResult<T>(this IValidator<T> validator, T instance) =>
        await validator.ValidateAsync(instance);
}
