using FluentValidation;
using MinhaBiblioteca.Api.Common.Extensions;
using MinhaBiblioteca.Api.Validations.FluentValidation.Books;
using MinhaBiblioteca.Core.Requests.Books;

namespace MinhaBiblioteca.Tests.Api;

public class ValidationExtensionsTests
{
    [Fact(DisplayName = "Deve retornar um ValidationResult")]
    [Trait("Category", "Extensions")]
    public async Task ValidationExtensions_GetValidationResult_ShouldReturnValidationResult()
    {
        // Arrange
        CreateBookRequest book = new() { Name = "Test1", GenreId = Guid.NewGuid(), AuthorId = Guid.NewGuid() };

        IValidator<CreateBookRequest> validator = new CreateBookValidation();

        // Act
        var validationResult = await validator.GetValidationResult(book);

        // Assert
        Assert.NotNull(validationResult);
    }

    [Fact(DisplayName = "Deve lançar uma exceção se o validator não for encontrado")]
    [Trait("Category", "Extensions")]
    public async Task ValidationExtensions_GetValidationResult_ShouldThrowExceptionIfValidatorNotFound()
    {
        // Arrange
        var book = new DeleteBookRequest(); // Example object

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(async () => await book.GetValidationResult());
    }
}
