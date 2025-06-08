using MinhaBiblioteca.Api.Common.Api;
using MinhaBiblioteca.Api.Endpoints.Author;
using MinhaBiblioteca.Api.Endpoints.Books;
using MinhaBiblioteca.Api.Endpoints.Genres;

namespace MinhaBiblioteca.Api.Endpoints;

public static class Endpoint
{
    public static void MapEndpoints(this WebApplication app)
    {
        var endpoints = app
            .MapGroup("");

        endpoints.MapGroup("/")
            .WithTags("Health Check")
            .MapGet("/", () => new { message = "OK" });

        endpoints.MapGroup("api/v1/books")
            .WithTags("Livros")
            .MapEndpoint<CreateBookEndpoint>()
            .MapEndpoint<UpdateBookEndpoint>()
            .MapEndpoint<DeleteBookEndpoint>()
            .MapEndpoint<GetBookByIdEndpoint>()
            .MapEndpoint<GetAllBooksEndpoint>();

        endpoints.MapGroup("api/v1/authors")
            .WithTags("Autores")
            .MapEndpoint<CreateAuthorEndpoint>()
            .MapEndpoint<UpdateAuthorEndpoint>()
            .MapEndpoint<DeleteAuthorEndpoint>()
            .MapEndpoint<GetAuthorByIdEndpoint>()
            .MapEndpoint<GetAllAuthorsEndpoint>();

        endpoints.MapGroup("api/v1/genres")
            .WithTags("Gêneros")
            .MapEndpoint<CreateGenreEndpoint>()
            .MapEndpoint<UpdateGenreEndpoint>()
            .MapEndpoint<DeleteGenreEndpoint>()
            .MapEndpoint<GetGenreByIdEndpoint>()
            .MapEndpoint<GetAllGenresEndpoint>();
    }

    private static IEndpointRouteBuilder MapEndpoint<TEndpoint>(this IEndpointRouteBuilder app)
        where TEndpoint : IEndpoint
    {
        TEndpoint.Map(app);

        return app;
    }
}
