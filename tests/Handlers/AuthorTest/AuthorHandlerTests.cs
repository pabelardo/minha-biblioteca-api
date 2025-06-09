using Microsoft.Extensions.Logging;
using MinhaBiblioteca.Api.Data.Repositories;
using MinhaBiblioteca.Api.Handlers;
using MinhaBiblioteca.Core.Enums;
using MinhaBiblioteca.Core.Models;
using MinhaBiblioteca.Core.Repositories;
using MinhaBiblioteca.Core.Requests.Authors;
using Moq;
using System.Linq.Expressions;

namespace MinhaBiblioteca.Tests.Handlers.AuthorTest;

[Collection(nameof(HandlerCollection))]
public class AuthorHandlerTests(HandlerTestsFixture fixture)
{
    private readonly HandlerTestsFixture _fixture = fixture;

    [Fact(DisplayName = "Deve criar um autor")]
    [Trait("Category", "AuthorHandler")]
    public async Task AuthorHandler_CreateAuthor_ShouldReturnCreatedAuthor()
    {
        // Arrange
        var authorHandler = _fixture.GetAuthorHandler();

        var request = new CreateAuthorRequest
        {
            Name = "Teste",
            Email = "teste@teste.com",
            Phone = "1234567890"
        };

        var authorCreated = new Author
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Email = request.Email,
            Phone = request.Phone,
            CreatedAt = DateTime.Now
        };

        _fixture.Mocker.GetMock<IAuthorRepository>()
            .Setup(r => r.AddAsync(It.IsAny<Author>()))
            .ReturnsAsync(authorCreated);

        _fixture.Mocker.GetMock<IAuthorRepository>()
            .Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Author, bool>>>()))
            .ReturnsAsync(false);

        // Act
        var response = await authorHandler.CreateAsync(request);

        // Assert
        Assert.Equal((int)StatusCodeEnum.Created, response.StatusCode);
        Assert.NotNull(response.Data);
        Assert.Equal(authorCreated.Id, response.Data?.Id);
        Assert.Equal("Teste", response.Data?.Name);
        Assert.Equal("teste@teste.com", response.Data?.Email);
        Assert.Equal("1234567890", response.Data?.Phone);

        _fixture.Mocker.GetMock<IAuthorRepository>()
            .Verify(r => r.AddAsync(It.IsAny<Author>()), Times.Once);

        _fixture.Mocker.GetMock<IAuthorRepository>()
            .Verify(r => r.ExistsAsync(It.IsAny<Expression<Func<Author, bool>>>()), Times.Once);
    }

    [Fact(DisplayName = "Deve obter todos os autores")]
    [Trait("Category", "AuthorHandler")]
    public async Task AuthorHandler_GetAllAuthors_ShouldReturnPagedAuthors()
    {
        // Arrange
        var context = _fixture.CreateContext();

        context.Author.AddRange(
            new Author
            {
                Id = Guid.NewGuid(),
                Name = "Autor 1",
                Email = "test@teste.com",
                Phone = "1234567890",
                CreatedAt = DateTime.Now
            },
            new Author
            {
                Id = Guid.NewGuid(),
                Name = "Autor 2",
                Email = "test2@teste.com",
                Phone = "1234667890",
                CreatedAt = DateTime.Now
            }
        );

        await context.SaveChangesAsync();

        Mock<ILogger<AuthorHandler>> loggerMock = new();
        Mock<AuthorRepository> authorRepo = new(context);

        AuthorHandler authorHandler = new(authorRepo.Object, loggerMock.Object);

        var request = new GetAllAuthorsRequest
        {
            PageNumber = 1,
            PageSize = 10
        };

        // Act
        var response = await authorHandler.GetAllAsync(request);

        // Assert
        Assert.Equal((int)StatusCodeEnum.OK, response.StatusCode);
        Assert.NotNull(response.Data);
        Assert.Equal(2, response.Data.Count());
    }

    [Fact(DisplayName = "Deve obter autor por ID")]
    [Trait("Category", "AuthorHandler")]
    public async Task AuthorHandler_GetAuthorById_ShouldReturnAuthorById()
    {
        // Arrange
        var authorHandler = _fixture.GetAuthorHandler();

        var request = new GetAuthorByIdRequest
        {
            Id = Guid.NewGuid()
        };

        var author = new Author
        {
            Id = Guid.NewGuid(),
            Name = "Autor Teste",
            Email = "test@teste.com",
            Phone = "1234567890",
            CreatedAt = DateTime.Now,
            Books = []
        };

        _fixture.Mocker.GetMock<IAuthorRepository>()
            .Setup(r => r.GetByIdAsync(request.Id))
            .ReturnsAsync(author);

        _fixture.Mocker.GetMock<IAuthorRepository>()
            .Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Author, bool>>>()))
            .ReturnsAsync(true);

        // Act
        var response = await authorHandler.GetByIdAsync(request);

        // Assert
        Assert.Equal((int)StatusCodeEnum.OK, response.StatusCode);
        Assert.NotNull(response.Data);
        Assert.Equal(author.Id, response.Data?.Id);
        Assert.Equal(author.Name, response.Data?.Name);
        Assert.Equal(author.Email, response.Data?.Email);
        Assert.Equal(author.Phone, response.Data?.Phone);
        Assert.Equal(author.CreatedAt, response.Data?.CreatedAt);
        Assert.Empty(response.Data?.Books!);
    }

    [Fact(DisplayName = "Deve atualizar um autor")]
    public async Task AuthorHandler_UpdateAuthor_ShouldReturnUpdatedAuthor()
    {
        // Arrange
        var authorHandler = _fixture.GetAuthorHandler();

        var request = new UpdateAuthorRequest
        {
            Id = Guid.NewGuid(),
            Name = "Autor Atualizado",
            Email = "test@teste.com",
            Phone = "1234567890",
        };

        var authorUpdated = new Author
        {
            Id = request.Id,
            Name = request.Name,
            Email = request.Email,
            Phone = request.Phone,
            CreatedAt = DateTime.Now
        };

        _fixture.Mocker.GetMock<IAuthorRepository>()
            .Setup(r => r.GetByIdAsync(request.Id))
            .ReturnsAsync(authorUpdated);

        _fixture.Mocker.GetMock<IAuthorRepository>()
            .Setup(r => r.UpdateAsync(It.IsAny<Author>()))
            .ReturnsAsync(authorUpdated);

        _fixture.Mocker.GetMock<IAuthorRepository>()
            .Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Author, bool>>>()))
            .ReturnsAsync(true);

        // Act
        var response = await authorHandler.UpdateAsync(request.Id, request);

        // Assert
        Assert.Equal((int)StatusCodeEnum.OK, response.StatusCode);
        Assert.NotNull(response.Data);
        Assert.Equal(authorUpdated.Id, response.Data?.Id);
        Assert.Equal(authorUpdated.Name, response.Data?.Name);
        Assert.Equal(authorUpdated.Email, response.Data?.Email);
        Assert.Equal(authorUpdated.Phone, response.Data?.Phone);
    }

    [Fact(DisplayName = "Deve excluir um autor")]
    [Trait("Category", "AuthorHandler")]
    public async Task AuthorHandler_DeleteAuthor_ShouldReturnNoContent()
    {
        // Arrange
        var authorHandler = _fixture.GetAuthorHandler();

        var request = new DeleteAuthorRequest
        {
            Id = Guid.NewGuid()
        };

        var author = new Author
        {
            Id = Guid.NewGuid(),
            Name = "Autor Atualizado",
            Email = "test@teste.com",
            Phone = "1234567890"
        };

        _fixture.Mocker.GetMock<IAuthorRepository>()
            .Setup(r => r.GetByIdAsync(request.Id))
            .ReturnsAsync(author);

        _fixture.Mocker.GetMock<IAuthorRepository>()
            .Setup(r => r.RemoveAsync(author))
            .Returns(Task.CompletedTask);

        _fixture.Mocker.GetMock<IAuthorRepository>()
            .Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Author, bool>>>()))
            .ReturnsAsync(false);

        // Act
        var response = await authorHandler.DeleteAsync(request);

        // Assert
        Assert.Equal((int)StatusCodeEnum.OK, response.StatusCode);
    }
}
