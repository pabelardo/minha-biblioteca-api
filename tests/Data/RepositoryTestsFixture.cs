using MinhaBiblioteca.Core.Models;
using Moq.AutoMock;

namespace MinhaBiblioteca.Tests.Data;

[CollectionDefinition(nameof(RepositoryCollection))]
public class RepositoryCollection : ICollectionFixture<RepositoryTestsFixture> { }

public class RepositoryTestsFixture : IDisposable
{
    public Book GetBook() => new()
    {
        Id = Guid.NewGuid(),
        Name = "Test Book",
        AuthorId = Guid.NewGuid(),
        GenreId = Guid.NewGuid()
    };

    public void Dispose() { }
}
