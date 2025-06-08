using MinhaBiblioteca.Api.Common.Api;
using MinhaBiblioteca.Api.DTOs;
using MinhaBiblioteca.Api.Handlers.Interfaces;
using MinhaBiblioteca.Core.Requests.Books;
using MinhaBiblioteca.Core.Responses;

namespace MinhaBiblioteca.Api.Endpoints.Books;

public class CreateBookEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) => 
        app.MapPost("/", HandleAsync)
           .WithName("Livros: Criar")
           .WithSummary("Cria um novo livro")
           .WithDescription("Cria um novo livro")
           .WithOrder(1)
           .Produces<Response<BookDto?>>(201)
           .Produces<Response<BookDto?>>(400);

    private static async Task<IResult> HandleAsync(
        IBookHandler handler,
        CreateBookRequest request)
    {
        var result = await handler.CreateAsync(request);

        return result.Success
            ? TypedResults.Created($"/{result.Data?.Id}", result)
            : TypedResults.BadRequest(result);
    }
}
