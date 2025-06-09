using MinhaBiblioteca.Api.Data.Repositories;
using MinhaBiblioteca.Core.Models;
using MinhaBiblioteca.Core.Repositories;
using Moq;
using System.Linq.Expressions;

namespace MinhaBiblioteca.Tests.Data;

[Collection(nameof(RepositoryCollection))]
public class RepositoryTests(RepositoryTestsFixture repositoryTestsFixture)
{
    [Fact(DisplayName = "Deve adicionar uma entidade no banco")]
    [Trait("Category", "Repository")]
    public async Task AddAsync_Repository_ShouldAddAnEntity()
    {
        // Arrange
        var repository = new Mock<IBookRepository>();

        var entity = repositoryTestsFixture.GetBook();

        repository.Setup(r => r.AddAsync(It.IsAny<Book>()))
                  .ReturnsAsync(entity);

        // Act
        var result = await repository.Object.AddAsync(entity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(entity.Id, result.Id);
        Assert.Equal(entity.Name, result.Name);
        Assert.Equal(entity.AuthorId, result.AuthorId);
        Assert.Equal(entity.GenreId, result.GenreId);
        Assert.IsType<Book>(result);
        repository.Verify(r => r.AddAsync(It.IsAny<Book>()), Times.Once);
    }

    [Fact(DisplayName = "Deve obter todas as entidades")]
    [Trait("Category", "Repository")]
    public async Task GetAllAsync_Repository_ShouldReturnAllEntities()
    {
        // Arrange
        var repository = new Mock<IBookRepository>();

        var entities = new List<Book>
        {
            new() { Id = Guid.NewGuid(), Name = "Entity 1", AuthorId = Guid.NewGuid(), GenreId = Guid.NewGuid() },
            new() { Id = Guid.NewGuid(), Name = "Entity 2", AuthorId = Guid.NewGuid(), GenreId = Guid.NewGuid() }
        };

        repository.Setup(r => r.GetAllAsync(It.IsAny<bool>()))
                  .ReturnsAsync(entities);

        // Act
        var result = await repository.Object.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        repository.Verify(r => r.GetAllAsync(It.IsAny<bool>()), Times.Once);
    }

    [Fact(DisplayName = "Deve obter uma entidade pelo ID")]
    [Trait("Category", "Repository")]
    public async Task GetByIdAsync_Repository_ShouldReturnEntityById()
    {
        // Arrange
        var repository = new Mock<IBookRepository>();

        var entity = repositoryTestsFixture.GetBook();

        repository.Setup(r => r.GetByIdAsync(entity.Id))
                  .ReturnsAsync(entity);
        // Act
        var result = await repository.Object.GetByIdAsync(entity.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(entity.Id, result?.Id);
        repository.Verify(r => r.GetByIdAsync(entity.Id), Times.Once);
    }

    [Fact(DisplayName = "Deve retornar nulo ao buscar entidade inexistente pelo ID")]
    [Trait("Category", "Repository")]
    public async Task GetByIdAsync_Repository_ShouldReturnNullWhenEntityNotFound()
    {
        // Arrange
        var repository = new Mock<IBookRepository>();
        var entityId = Guid.NewGuid();
        repository.Setup(r => r.GetByIdAsync(entityId))
                  .ReturnsAsync((Book?)null);
        // Act
        var result = await repository.Object.GetByIdAsync(entityId);
        // Assert
        Assert.Null(result);
        repository.Verify(r => r.GetByIdAsync(entityId), Times.Once);
    }

    [Fact(DisplayName = "Deve atualizar uma entidade no banco")]
    [Trait("Category", "Repository")]
    public async Task UpdateAsync_Repository_ShouldUpdateAnEntity()
    {
        // Arrange
        var repository = new Mock<IBookRepository>();

        var entity = repositoryTestsFixture.GetBook();

        entity.Name = "Updated Book Name";

        repository.Setup(r => r.UpdateAsync(It.IsAny<Book>()))
                  .ReturnsAsync(entity);

        // Act
        var result = await repository.Object.UpdateAsync(entity);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(entity.Id, result.Id);
        Assert.Equal(entity.Name, result.Name);
        Assert.Equal(entity.AuthorId, result.AuthorId);
        Assert.Equal(entity.GenreId, result.GenreId);
        Assert.IsType<Book>(result);
        repository.Verify(r => r.UpdateAsync(It.IsAny<Book>()), Times.Once);
    }

    [Fact(DisplayName = "Deve excluir uma entidade do banco")]
    [Trait("Category", "Repository")]
    public async Task DeleteAsync_Repository_ShouldDeleteAnEntity()
    {
        // Arrange
        var repository = new Mock<IBookRepository>();

        var entity = repositoryTestsFixture.GetBook();

        repository.Setup(r => r.RemoveAsync(entity))
                  .Returns(Task.CompletedTask);

        // Act
        await repository.Object.RemoveAsync(entity);

        // Assert
        repository.Verify(r => r.RemoveAsync(entity), Times.Once);
    }

    [Fact(DisplayName = "Deve retornar um IQueryable<TEntity>")]
    [Trait("Category", "Repository")]
    public async Task GetQueryable_Repository_ShouldReturnIQueryable()
    {
        // Arrange
        var repository = new Mock<IBookRepository>();

        // Act
        var result = await repository.Object.GetQueryable(true);

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IQueryable<Book>>(result);
        repository.Verify(r => r.GetQueryable(true), Times.Once);
    }

    [Fact(DisplayName = "Deve retornar a Entidade através de um Filtro")]
    [Trait("Category", "Repository")]
    public async Task GetByFilterAsync_Repository_ShouldReturnEntityByFilter()
    {
        // Arrange
        var repository = new Mock<IBookRepository>();
        var entity = repositoryTestsFixture.GetBook();
        repository.Setup(r => r.GetByFilterAsync(It.IsAny<Expression<Func<Book, bool>>>()))
                  .ReturnsAsync([entity]);

        // Act
        var result = await repository.Object.GetByFilterAsync(b => b.Id == entity.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(entity.Id, result.First().Id);
        repository.Verify(r => r.GetByFilterAsync(It.IsAny<Expression<Func<Book, bool>>>()), Times.Once);
    }

    [Fact(DisplayName = "Deve verificar a expressão é verdadeira ou falsa")]
    [Trait("Category", "Repository")]
    public async Task AnyAsync_Repository_ShouldReturnTrueIfEntityExistsByFilter()
    {
        // Arrange
        var repository = new Mock<IBookRepository>();

        var entity = repositoryTestsFixture.GetBook();

        repository.Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Book, bool>>>()))
                  .ReturnsAsync(true);

        // Act
        var result = await repository.Object.ExistsAsync(b => b.Id == entity.Id);

        // Assert
        Assert.True(result);
        repository.Verify(r => r.ExistsAsync(It.IsAny<Expression<Func<Book, bool>>>()), Times.Once);
    }
}
