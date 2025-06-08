using MinhaBiblioteca.Api.Common.Api;
using MinhaBiblioteca.Api.DTOs;
using MinhaBiblioteca.Api.Handlers.Interfaces;
using MinhaBiblioteca.Core.Enums;
using MinhaBiblioteca.Core.Requests.Books;
using MinhaBiblioteca.Core.Responses;

namespace MinhaBiblioteca.Api.Endpoints.Books;

public class GetBookByIdEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) =>
        app.MapGet("/{id:guid}", HandleAsync)
           .WithName("Livro: Obter Por Id")
           .WithSummary("Recupera um livro")
           .WithDescription("Recupera um livro")
           .WithOrder(3)
           .Produces<Response<BookDto?>>()
           .Produces<Response<BookDto?>>(400)
           .Produces<Response<BookDto?>>(404);

    private static async Task<IResult> HandleAsync(
        IBookHandler handler,
        Guid id)
    {
        var request = new GetBookByIdRequest { Id = id };

        var result = await handler.GetByIdAsync(request);

        return result.StatusCode switch
        {
            (int)StatusCodeEnum.OK => TypedResults.Ok(result),
            (int)StatusCodeEnum.NotFound => TypedResults.NotFound(result),
            _ => TypedResults.BadRequest(result),
        };
    }
}
