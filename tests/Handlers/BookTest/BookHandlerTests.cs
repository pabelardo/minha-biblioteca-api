using Microsoft.Extensions.Logging;
using MinhaBiblioteca.Api.Data.Repositories;
using MinhaBiblioteca.Api.Handlers;
using MinhaBiblioteca.Core.Enums;
using MinhaBiblioteca.Core.Models;
using MinhaBiblioteca.Core.Repositories;
using MinhaBiblioteca.Core.Requests.Books;
using Moq;
using System.Linq.Expressions;

namespace MinhaBiblioteca.Tests.Handlers.BookTest;

[Collection(nameof(HandlerCollection))]
public class BookHandlerTests(HandlerTestsFixture fixture)
{
    private readonly HandlerTestsFixture _fixture = fixture;

    [Fact(DisplayName = "Deve criar um livro")]
    [Trait("Category", "BookHandler")]
    public async Task BookHandler_CreateBook_ShouldReturnCreatedBook()
    {
        // Arrange
        var bookHandler = _fixture.GetBookHandler();

        var request = new CreateBookRequest
        {
            Name = "Teste Livro",
            AuthorId = Guid.NewGuid(),
            GenreId = Guid.NewGuid()
        };

        var bookCreated = new Book
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            AuthorId = request.AuthorId,
            GenreId = request.GenreId,
            CreatedAt = DateTime.Now
        };

        _fixture.Mocker.GetMock<IBookRepository>()
            .Setup(r => r.AddAsync(It.IsAny<Book>()))
            .ReturnsAsync(bookCreated);

        _fixture.Mocker.GetMock<IBookRepository>()
            .Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Book, bool>>>()))
            .ReturnsAsync(false);

        _fixture.Mocker.GetMock<IGenreRepository>()
            .Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Genre, bool>>>()))
            .ReturnsAsync(true);

        _fixture.Mocker.GetMock<IAuthorRepository>()
            .Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Author, bool>>>()))
            .ReturnsAsync(true);

        // Act
        var response = await bookHandler.CreateAsync(request);

        // Assert
        Assert.Equal((int)StatusCodeEnum.Created, response.StatusCode);
        Assert.NotNull(response.Data);
        Assert.Equal(bookCreated.Id, response.Data?.Id);
        Assert.Equal("Teste Livro", response.Data?.Name);
        Assert.Equal(bookCreated.AuthorId, response.Data?.AuthorId);
        Assert.Equal(bookCreated.GenreId, response.Data?.GenreId);
        Assert.Equal(bookCreated.CreatedAt, response.Data?.CreatedAt);
        _fixture.Mocker.GetMock<IBookRepository>().Verify(r => r.AddAsync(It.IsAny<Book>()), Times.Once);
    }

    [Fact(DisplayName = "Deve retornar erro ao criar livro com propriedades inválidas")]
    [Trait("Category", "BookHandler")]
    public async Task BookHandler_CreateBook_WithInvalidProperties_ShouldReturnBadRequest()
    {
        // Arrange
        var bookHandler = _fixture.GetBookHandler();

        var request = new CreateBookRequest
        {
            Name = "", // Nome inválido
            AuthorId = Guid.NewGuid(),
            GenreId = Guid.NewGuid()
        };

        // Act
        var response = await bookHandler.CreateAsync(request);

        // Assert
        Assert.Equal((int)StatusCodeEnum.BadRequest, response.StatusCode);
        Assert.Null(response.Data);
        Assert.Contains("Não foi possível criar o livro", response.Message);
    }

    [Fact(DisplayName = "Deve retornar erro ao criar livro com autor ou gênero inexistente")]
    [Trait("Category", "BookHandler")]
    public async Task BookHandler_CreateBook_WithNonExistentAuthorOrGenre_ShouldReturnBadRequest()
    {
        // Arrange
        var bookHandler = _fixture.GetBookHandler();
        var request = new CreateBookRequest
        {
            Name = "Teste Livro",
            AuthorId = Guid.NewGuid(),
            GenreId = Guid.NewGuid()
        };
        _fixture.Mocker.GetMock<IBookRepository>()
            .Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Book, bool>>>()))
            .ReturnsAsync(false);
        _fixture.Mocker.GetMock<IGenreRepository>()
            .Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Genre, bool>>>()))
            .ReturnsAsync(false);
        _fixture.Mocker.GetMock<IAuthorRepository>()
            .Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Author, bool>>>()))
            .ReturnsAsync(false);

        // Act
        var response = await bookHandler.CreateAsync(request);

        // Assert
        Assert.Equal((int)StatusCodeEnum.BadRequest, response.StatusCode);
        Assert.Null(response.Data);
        Assert.Contains("Autor e gênero não foram encontrados na base", response.Message);
    }

    [Fact(DisplayName = "Deve retornar erro ao criar livro com o mesmo autor e gênero")]
    [Trait("Category", "BookHandler")]
    public async Task BookHandler_CreateBook_WithExistingAuthorAndGenre_ShouldReturnBadRequest()
    {
        // Arrange
        var bookHandler = _fixture.GetBookHandler();
        var request = new CreateBookRequest
        {
            Name = "Teste Livro",
            AuthorId = Guid.NewGuid(),
            GenreId = Guid.NewGuid()
        };

        _fixture.Mocker.GetMock<IBookRepository>()
            .Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Book, bool>>>()))
            .ReturnsAsync(true);

        _fixture.Mocker.GetMock<IGenreRepository>()
            .Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Genre, bool>>>()))
            .ReturnsAsync(true);

        _fixture.Mocker.GetMock<IAuthorRepository>()
            .Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Author, bool>>>()))
            .ReturnsAsync(true);

        // Act
        var response = await bookHandler.CreateAsync(request);

        // Assert
        Assert.Equal((int)StatusCodeEnum.BadRequest, response.StatusCode);
        Assert.Null(response.Data);
        Assert.Contains("Já existe um livro com o mesmo autor e gênero", response.Message);
    }

    [Fact(DisplayName = "Deve obter todos os livros")]
    [Trait("Category", "BookHandler")]
    public async Task BookHandler_GetAllBooks_ShouldReturnAllBooks()
    {
        // Arrange
        var context = _fixture.CreateContext();

        context.Books.AddRange(
            new Book
            {
                Id = Guid.NewGuid(),
                Name = "Livro 1",
                AuthorId = Guid.NewGuid(),
                GenreId = Guid.NewGuid(),
                CreatedAt = DateTime.Now
            },
            new Book
            {
                Id = Guid.NewGuid(),
                Name = "Livro 2",
                AuthorId = Guid.NewGuid(),
                GenreId = Guid.NewGuid(),
                CreatedAt = DateTime.Now
            }
        );

        await context.SaveChangesAsync();

        Mock<ILogger<BookHandler>> loggerMock = new();
        Mock<BookRepository> bookRepo = new(context);
        Mock<IAuthorRepository> authorRepo = new();
        Mock<IGenreRepository> genreRepo = new();

        var bookHandler = new BookHandler(bookRepo.Object, authorRepo.Object, genreRepo.Object, loggerMock.Object);

        var request = new GetAllBooksRequest { PageNumber = 1, PageSize = 10 };

        // Act
        var response = await bookHandler.GetAllAsync(request);

        // Assert
        Assert.Equal((int)StatusCodeEnum.OK, response.StatusCode);
        Assert.NotNull(response.Data);
        Assert.Equal(2, response.Data.Count());
    }

    [Fact(DisplayName = "Deve obter livro por ID")]
    [Trait("Category", "BookHandler")]
    public async Task BookHandler_GetBookById_ShouldReturnBook()
    {
        // Arrange
        var bookHandler = _fixture.GetBookHandler();
        var bookId = Guid.NewGuid();
        var book = new Book
        {
            Id = bookId,
            Name = "Teste Livro",
            AuthorId = Guid.NewGuid(),
            GenreId = Guid.NewGuid(),
            CreatedAt = DateTime.Now
        };

        _fixture.Mocker.GetMock<IBookRepository>()
            .Setup(r => r.GetByIdAsync(bookId))
            .ReturnsAsync(book);

        // Act
        var response = await bookHandler.GetByIdAsync(new GetBookByIdRequest { Id = bookId });

        // Assert
        Assert.Equal((int)StatusCodeEnum.OK, response.StatusCode);
        Assert.NotNull(response.Data);
        Assert.Equal(book.Id, response.Data?.Id);
        Assert.Equal(book.Name, response.Data?.Name);
    }

    [Fact(DisplayName = "Deve retornar erro ao obter livro por ID inexistente")]
    [Trait("Category", "BookHandler")]
    public async Task BookHandler_GetBookById_WithNonExistentId_ShouldReturnNotFound()
    {
        // Arrange
        var bookHandler = _fixture.GetBookHandler();

        var bookId = Guid.NewGuid();

        _fixture.Mocker.GetMock<IBookRepository>()
            .Setup(r => r.GetByIdAsync(bookId))
            .ReturnsAsync((Book?)null);

        // Act
        var response = await bookHandler.GetByIdAsync(new GetBookByIdRequest { Id = bookId });

        // Assert
        Assert.Equal((int)StatusCodeEnum.NotFound, response.StatusCode);
        Assert.Null(response.Data);
        Assert.Contains("Livro não encontrado !", response.Message);
    }

    [Fact(DisplayName = "Deve obter o livro por ID")]
    [Trait("Category", "BookHandler")]
    public async Task BookHandler_GetBookById_ShouldReturnBookById()
    {
        // Arrange
        var bookHandler = _fixture.GetBookHandler();

        var bookId = Guid.NewGuid();

        var book = new Book
        {
            Id = bookId,
            Name = "Livro de Teste",
            AuthorId = Guid.NewGuid(),
            GenreId = Guid.NewGuid(),
            CreatedAt = DateTime.Now
        };

        _fixture.Mocker.GetMock<IBookRepository>()
            .Setup(r => r.GetByIdAsync(bookId))
            .ReturnsAsync(book);

        // Act
        var response = await bookHandler.GetByIdAsync(new GetBookByIdRequest { Id = bookId });

        // Assert
        Assert.Equal((int)StatusCodeEnum.OK, response.StatusCode);
        Assert.NotNull(response.Data);
        Assert.Equal(book.Name, response.Data?.Name);
    }

    [Fact(DisplayName = "Deve atualizar o livro")]
    [Trait("Category", "BookHandler")]
    public async Task BookHandler_UpdateBook_ShouldReturnUpdatedBook()
    {
        // Arrange
        var bookHandler = _fixture.GetBookHandler();

        var bookId = Guid.NewGuid();

        var request = new UpdateBookRequest
        {
            Id = bookId,
            Name = "Livro Atualizado",
            AuthorId = Guid.NewGuid(),
            GenreId = Guid.NewGuid()
        };

        var existingBook = new Book
        {
            Id = bookId,
            Name = "Livro Antigo",
            AuthorId = Guid.NewGuid(),
            GenreId = Guid.NewGuid(),
            CreatedAt = DateTime.Now
        };

        _fixture.Mocker.GetMock<IBookRepository>()
            .Setup(r => r.GetByIdAsync(bookId))
            .ReturnsAsync(existingBook);

        _fixture.Mocker.GetMock<IBookRepository>()
            .Setup(r => r.UpdateAsync(It.IsAny<Book>()))
            .ReturnsAsync(existingBook);

        // Act
        var response = await bookHandler.UpdateAsync(bookId, request);

        // Assert
        Assert.Equal((int)StatusCodeEnum.OK, response.StatusCode);
        Assert.NotNull(response.Data);
        Assert.Equal(request.Name, response.Data?.Name);
    }

    [Fact(DisplayName = "Deve retornar erro ao atualizar livro com ID diferente que foi fornecido na URL")]
    [Trait("Category", "BookHandler")]
    public async Task BookHandler_UpdateBook_WithMismatchedId_ShouldReturnBadRequest()
    {
        // Arrange
        var bookHandler = _fixture.GetBookHandler();

        var bookId = Guid.NewGuid();

        var request = new UpdateBookRequest
        {
            Id = Guid.NewGuid(), // ID diferente do fornecido na URL
            Name = "Livro Atualizado",
            AuthorId = Guid.NewGuid(),
            GenreId = Guid.NewGuid()
        };

        // Act
        var response = await bookHandler.UpdateAsync(bookId, request);

        // Assert
        Assert.Equal((int)StatusCodeEnum.BadRequest, response.StatusCode);
        Assert.Null(response.Data);
        Assert.Contains("O ID do livro não corresponde ao ID fornecido na URL", response.Message);
    }

    [Fact(DisplayName = "Deve retornar erro ao atualizar livro inexistente")]
    public async Task BookHandler_UpdateBook_WithNonExistentId_ShouldReturnNotFound()
    {
        // Arrange
        var bookHandler = _fixture.GetBookHandler();

        var bookId = Guid.NewGuid();

        var request = new UpdateBookRequest
        {
            Id = bookId,
            Name = "Livro Atualizado",
            AuthorId = Guid.NewGuid(),
            GenreId = Guid.NewGuid()
        };

        _fixture.Mocker.GetMock<IBookRepository>()
            .Setup(r => r.GetByIdAsync(bookId))
            .ReturnsAsync((Book?)null);

        // Act
        var response = await bookHandler.UpdateAsync(bookId, request);

        // Assert
        Assert.Equal((int)StatusCodeEnum.NotFound, response.StatusCode);
        Assert.Null(response.Data);
        Assert.Contains("Livro não encontrado !", response.Message);
    }

    [Fact(DisplayName = "Deve retornar erro ao atualizar livro com propriedades inválidas")]
    [Trait("Category", "BookHandler")]
    public async Task BookHandler_UpdateBook_WithInvalidProperties_ShouldReturnBadRequest()
    {
        // Arrange
        var bookHandler = _fixture.GetBookHandler();
        var bookId = Guid.NewGuid();
        var request = new UpdateBookRequest
        {
            Id = bookId,
            Name = "", // Nome inválido
            AuthorId = Guid.NewGuid(),
            GenreId = Guid.NewGuid()
        };
        // Act
        var response = await bookHandler.UpdateAsync(bookId, request);
        // Assert
        Assert.Equal((int)StatusCodeEnum.BadRequest, response.StatusCode);
        Assert.Null(response.Data);
        Assert.Contains("Não foi possível atualizar o livro", response.Message);
    }

    [Fact(DisplayName = "Deve excluir um livro")]
    [Trait("Category", "BookHandler")]
    public async Task BookHandler_DeleteBook_ShouldReturnNoContent()
    {
        // Arrange
        var bookHandler = _fixture.GetBookHandler();

        var book = new Book
        {
            Id = Guid.NewGuid(),
            Name = "Livro para Excluir",
            AuthorId = Guid.NewGuid(),
            GenreId = Guid.NewGuid(),
            CreatedAt = DateTime.Now
        };

        _fixture.Mocker.GetMock<IBookRepository>()
            .Setup(r => r.GetByIdAsync(book.Id))
            .ReturnsAsync(book);

        _fixture.Mocker.GetMock<IBookRepository>()
            .Setup(r => r.RemoveAsync(book))
            .Returns(Task.CompletedTask);

        // Act
        var response = await bookHandler.DeleteAsync(new DeleteBookRequest { Id = book.Id});

        // Assert
        Assert.Equal((int)StatusCodeEnum.OK, response.StatusCode);
        Assert.NotNull(response.Data);
        Assert.Equal("Livro excluído com sucesso !", response.Message);
    }
}
