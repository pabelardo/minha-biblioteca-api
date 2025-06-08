using FluentValidation;
using FluentValidation.Results;

namespace MinhaBiblioteca.Api.Common.Extensions;

public static class ValidationExtensions
{
    public static async Task<ValidationResult> GetValidationResult<T>(this IValidator<T> validator, T instance) =>
        await validator.ValidateAsync(instance);

    public static async Task<ValidationResult> GetValidationResult<T>(this T obj) where T : class
    {
        Type genericType = typeof(IValidator<>).MakeGenericType(typeof(T));

        if (genericType.GetService() is not IValidator<T> validator)
            throw new ValidationException($"Não foi encontrado o validator para o tipo {typeof(T).Name}.");

        var validationResult = await validator.GetValidationResult(obj);

        return validationResult;
    }
}
