using MinhaBiblioteca.Api.Data.Repositories;
using MinhaBiblioteca.Api.Handlers;
using MinhaBiblioteca.Api.Handlers.Interfaces;
using MinhaBiblioteca.Core.Enums;
using MinhaBiblioteca.Core.Models;
using MinhaBiblioteca.Core.Repositories;
using MinhaBiblioteca.Core.Requests.Genres;
using Moq;
using System.Linq.Expressions;

namespace MinhaBiblioteca.Tests.Handlers.GenreTest;

[Collection(nameof(HandlerCollection))]
public class GenreHandlerTests(HandlerTestsFixture fixture)
{
    private readonly HandlerTestsFixture _fixture = fixture;

    [Fact(DisplayName = "Deve criar um gênero")]
    [Trait("Category", "GenreHandler")]
    public async Task GenreHandler_CreateGenre_ShouldReturnCreatedGenre()
    {
        // Arrange
        var genreHandler = _fixture.GetGenreHandler();

        var request = new CreateGenreRequest
        {
            Name = "Ficção Científica",
            Description = "Test Description"
        };

        var genreCreated = new Genre
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            CreatedAt = DateTime.Now
        };

        _fixture.Mocker.GetMock<IGenreRepository>()
            .Setup(r => r.AddAsync(It.IsAny<Genre>()))
            .ReturnsAsync(genreCreated);

        _fixture.Mocker.GetMock<IGenreRepository>()
            .Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Genre, bool>>>()))
            .ReturnsAsync(false);

        // Act
        var response = await genreHandler.CreateAsync(request);

        // Assert
        Assert.Equal((int)StatusCodeEnum.Created, response.StatusCode);
        Assert.NotNull(response.Data);
        Assert.Equal(genreCreated.Id, response.Data?.Id);
        Assert.Equal("Ficção Científica", response.Data?.Name);
        _fixture.Mocker.GetMock<IGenreRepository>()
            .Verify(r => r.AddAsync(It.IsAny<Genre>()), Times.Once);
    }

    [Fact(DisplayName = "Deve retornar erro ao tentar criar gênero com dados inválidos")]
    [Trait("Category", "GenreHandler")]
    public async Task GenreHandler_CreateGenre_ShouldReturnBadRequestWhenInvalidData()
    {
        // Arrange
        var genreHandler = _fixture.GetGenreHandler();
        var request = new CreateGenreRequest
        {
            Name = "", // Nome inválido
            Description = "Test Description"
        };
        // Act
        var response = await genreHandler.CreateAsync(request);
        // Assert
        Assert.Equal((int)StatusCodeEnum.BadRequest, response.StatusCode);
        Assert.Null(response.Data);
        Assert.Contains("O campo Name é de preenchimento obrigatório", response.Errors!);
        Assert.Contains("O campo Name precisa ter entre 2 e 100 caracteres", response.Errors!);
    }

    [Fact(DisplayName = "Obter todos os gêneros")]
    [Trait("Category", "GenreHandler")]
    public async Task GenreHandler_GetAllGenres_ShouldReturnPagedGenres()
    {
        // Arrange
        var context = _fixture.CreateContext();

        context.Genre.AddRange(
            new Genre { Id = Guid.NewGuid(), Name = "Ficção Científica", Description = "teste description 1", CreatedAt = DateTime.Now },
            new Genre { Id = Guid.NewGuid(), Name = "Fantasia", Description = "teste description 2", CreatedAt = DateTime.Now }
        );

        await context.SaveChangesAsync();

        Mock<GenreRepository> mockRepoGenre = new(context);

        GenreHandler genreHandler = new(mockRepoGenre.Object);

        var request = new GetAllGenresRequest
        {
            PageNumber = 1,
            PageSize = 10
        };

        // Act
        var response = await genreHandler.GetAllAsync(request);

        // Assert
        Assert.Equal((int)StatusCodeEnum.OK, response.StatusCode);
        Assert.NotNull(response.Data);
        Assert.Equal(2, response.Data.Count());
    }

    [Fact(DisplayName = "Obter gênero por ID")]
    [Trait("Category", "GenreHandler")]
    public async Task GenreHandler_GetGenreById_ShouldReturnGenre()
    {
        // Arrange
        var genreHandler = _fixture.GetGenreHandler();

        var request = new GetGenreByIdRequest
        {
            Id = Guid.NewGuid()
        };

        var genre = new Genre
        {
            Id = request.Id,
            Name = "Ficção Científica",
            Description = "Test Description",
            CreatedAt = DateTime.Now
        };

        _fixture.Mocker.GetMock<IGenreRepository>()
            .Setup(r => r.GetByIdAsync(request.Id))
            .ReturnsAsync(genre);

        // Act
        var response = await genreHandler.GetByIdAsync(request);

        // Assert
        Assert.Equal((int)StatusCodeEnum.OK, response.StatusCode);
        Assert.NotNull(response.Data);
        Assert.Equal(request.Id, response.Data?.Id);
        Assert.Equal("Ficção Científica", response.Data?.Name);
        _fixture.Mocker.GetMock<IGenreRepository>()
            .Verify(r => r.GetByIdAsync(request.Id), Times.Once);
    }

    [Fact(DisplayName = "Deve retornar erro ao tentar obter gênero por ID inexistente")]
    [Trait("Category", "GenreHandler")]
    public async Task GenreHandler_GetGenreById_ShouldReturnNotFoundWhenGenreDoesNotExist()
    {
        // Arrange
        var genreHandler = _fixture.GetGenreHandler();
        var request = new GetGenreByIdRequest
        {
            Id = Guid.NewGuid()
        };
        _fixture.Mocker.GetMock<IGenreRepository>()
            .Setup(r => r.GetByIdAsync(request.Id))
            .ReturnsAsync((Genre?)null);
        // Act
        var response = await genreHandler.GetByIdAsync(request);
        // Assert
        Assert.Equal((int)StatusCodeEnum.NotFound, response.StatusCode);
        Assert.Null(response.Data);
        _fixture.Mocker.GetMock<IGenreRepository>()
            .Verify(r => r.GetByIdAsync(request.Id), Times.Once);
    }

    [Fact(DisplayName = "Deve atualizar um gênero")]
    [Trait("Category", "GenreHandler")]
    public async Task GenreHandler_UpdateGenre_ShouldReturnUpdatedGenre()
    {
        // Arrange
        var genreHandler = _fixture.GetGenreHandler();
        var request = new UpdateGenreRequest
        {
            Id = Guid.NewGuid(),
            Name = "Ficção Científica Atualizado",
            Description = "Test Description Updated"
        };
        var genreUpdated = new Genre
        {
            Id = request.Id,
            Name = request.Name,
            CreatedAt = DateTime.Now
        };

        _fixture.Mocker.GetMock<IGenreRepository>()
            .Setup(r => r.UpdateAsync(It.IsAny<Genre>()))
            .ReturnsAsync(genreUpdated);

        _fixture.Mocker.GetMock<IGenreRepository>()
            .Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Genre, bool>>>()))
            .ReturnsAsync(true);

        _fixture.Mocker.GetMock<IGenreRepository>()
            .Setup(r => r.GetByIdAsync(request.Id))
            .ReturnsAsync(genreUpdated);

        // Act
        var response = await genreHandler.UpdateAsync(request.Id, request);

        // Assert
        Assert.Equal((int)StatusCodeEnum.OK, response.StatusCode);
        Assert.NotNull(response.Data);
        Assert.Equal(genreUpdated.Id, response.Data?.Id);
        Assert.Equal("Ficção Científica Atualizado", response.Data?.Name);
        _fixture.Mocker.GetMock<IGenreRepository>()
            .Verify(r => r.UpdateAsync(It.IsAny<Genre>()), Times.Once);
    }

    [Fact(DisplayName = "Deve retornar um erro, pois o ID do gênero não corresponde ao ID fornecido na URL")]
    [Trait("Category", "GenreHandler")]
    public async Task GenreHandler_UpdateGenre_ShouldReturnBadRequestWhenIdMismatch()
    {
        // Arrange
        var genreHandler = _fixture.GetGenreHandler();
        var request = new UpdateGenreRequest
        {
            Id = Guid.NewGuid(),
            Name = "Ficção Científica Atualizado",
            Description = "Test Description Updated"
        };
        // Act
        var response = await genreHandler.UpdateAsync(Guid.NewGuid(), request);
        // Assert
        Assert.Equal((int)StatusCodeEnum.BadRequest, response.StatusCode);
        Assert.Null(response.Data);
        Assert.Contains("O ID do gênero não corresponde ao ID fornecido na URL.", response.Message);
    }

    [Fact(DisplayName = "Deve retornar erro ao tentar atualizar gênero com dados inválidos")]
    [Trait("Category", "GenreHandler")]
    public async Task GenreHandler_UpdateGenre_ShouldReturnBadRequestWhenInvalidData()
    {
        // Arrange
        var genreHandler = _fixture.GetGenreHandler();
        var request = new UpdateGenreRequest
        {
            Id = Guid.NewGuid(),
            Name = "", // Nome inválido
            Description = "Test Description Updated"
        };
        // Act
        var response = await genreHandler.UpdateAsync(request.Id, request);
        // Assert
        Assert.Equal((int)StatusCodeEnum.BadRequest, response.StatusCode);
        Assert.Null(response.Data);
        Assert.Contains("O campo Name é de preenchimento obrigatório", response.Errors!);
        Assert.Contains("O campo Name precisa ter entre 2 e 100 caracteres", response.Errors!);
    }

    [Fact(DisplayName = "Deve retornar erro ao tentar atualizar gênero inexistente")]
    [Trait("Category", "GenreHandler")]
    public async Task GenreHandler_UpdateGenre_ShouldReturnNotFoundWhenGenreDoesNotExist()
    {
        // Arrange
        var genreHandler = _fixture.GetGenreHandler();
        var request = new UpdateGenreRequest
        {
            Id = Guid.NewGuid(),
            Name = "Ficção Científica Atualizado",
            Description = "Test Description Updated"
        };
        _fixture.Mocker.GetMock<IGenreRepository>()
            .Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Genre, bool>>>()))
            .ReturnsAsync(false);
        // Act
        var response = await genreHandler.UpdateAsync(request.Id, request);
        // Assert
        Assert.Equal((int)StatusCodeEnum.NotFound, response.StatusCode);
        Assert.Null(response.Data);
        Assert.Contains("Gênero não encontrado !", response.Message);
    }

    [Fact(DisplayName = "Deve excluir um gênero")]
    [Trait("Category", "GenreHandler")]
    public async Task GenreHandler_DeleteGenre_ShouldReturnNoContent()
    {
        // Arrange
        var genreHandler = _fixture.GetGenreHandler();

        var genre = new Genre
        {
            Id = Guid.NewGuid(),
            Name = "Teste",
            CreatedAt = DateTime.Now
        };

        _fixture.Mocker.GetMock<IGenreRepository>()
            .Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Genre, bool>>>()))
            .ReturnsAsync(false);

        _fixture.Mocker.GetMock<IGenreRepository>()
            .Setup(r => r.RemoveAsync(genre))
            .Returns(Task.CompletedTask);
        
        _fixture.Mocker.GetMock<IGenreRepository>()
            .Setup(r => r.GetByIdAsync(genre.Id))
            .ReturnsAsync(genre);

        // Act
        var response = await genreHandler.DeleteAsync(new DeleteGenreRequest { Id = genre.Id});

        // Assert
        Assert.Equal((int)StatusCodeEnum.OK, response.StatusCode);
        Assert.NotNull(response.Data);
        _fixture.Mocker.GetMock<IGenreRepository>()
            .Verify(r => r.RemoveAsync(genre), Times.Once);
    }
}
