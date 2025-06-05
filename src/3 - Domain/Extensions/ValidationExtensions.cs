using FluentValidation;
using FluentValidation.Results;

namespace MinhaBiblioteca.Domain.Extensions;

public static class ValidationExtensions
{
    public static async Task<ValidationResult> ObterValidationResult<T>(this IValidator<T> validator, T instance) =>
        await validator.ValidateAsync(instance);
}
