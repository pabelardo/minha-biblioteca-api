using Microsoft.EntityFrameworkCore;
using MinhaBiblioteca.Api.Common.Extensions;
using MinhaBiblioteca.Api.Data.Context;
using MinhaBiblioteca.Api.Handlers;
using MinhaBiblioteca.Tests.Helpers;
using Moq.AutoMock;

namespace MinhaBiblioteca.Tests.Handlers;

[CollectionDefinition(nameof(HandlerCollection))]
public class HandlerCollection : ICollectionFixture<HandlerTestsFixture> { }

public class HandlerTestsFixture : IDisposable
{
    public AuthorHandler AuthorHandler = null!;
    public BookHandler BookHandler = null!;
    public GenreHandler GenreHandler = null!;
    public AutoMocker Mocker = null!;

    public GenreHandler GetGenreHandler()
    {
        Mocker = new AutoMocker();

        GenreHandler = Mocker.CreateInstance<GenreHandler>();

        var services = MockHelpers.GetServiceProvider();

        services.InitServiceProvider();

        return GenreHandler;
    }

    public BookHandler GetBookHandler()
    {
        Mocker = new AutoMocker();

        BookHandler = Mocker.CreateInstance<BookHandler>();

        var services = MockHelpers.GetServiceProvider();

        services.InitServiceProvider();

        return BookHandler;
    }

    public AuthorHandler GetAuthorHandler()
    {
        Mocker = new AutoMocker();

        AuthorHandler = Mocker.CreateInstance<AuthorHandler>();

        var services = MockHelpers.GetServiceProvider();

        services.InitServiceProvider();

        return AuthorHandler;
    }

    public AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid())
            .Options;

        return new AppDbContext(options);
    }

    public void Dispose() { }
}
