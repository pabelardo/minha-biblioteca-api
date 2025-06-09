using MinhaBiblioteca.Api.Common.Api;
using MinhaBiblioteca.Api.DTOs;
using MinhaBiblioteca.Api.Handlers.Interfaces;
using MinhaBiblioteca.Core.Enums;
using MinhaBiblioteca.Core.Requests.Books;
using MinhaBiblioteca.Core.Responses;

namespace MinhaBiblioteca.Api.Endpoints.Books;

public class UpdateBookEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) =>
        app.MapPut("/{id:guid}", HandleAsync)
           .WithName("Livros: Atualizar")
           .WithSummary("Atualiza um livro")
           .WithDescription("Atualiza um livro")
           .WithOrder(4)
           .Produces<Response<BookDto>>()
           .Produces<Response<BookDto>>(400)
           .Produces<Response<BookDto>>(404);

    private static async Task<IResult> HandleAsync(
        IBookHandler handler,
        UpdateBookRequest request,
        Guid id)
    {
        var result = await handler.UpdateAsync(id, request);

        return result.StatusCode switch
        {
            (int)StatusCodeEnum.OK => TypedResults.Ok(result),
            (int)StatusCodeEnum.NotFound => TypedResults.NotFound(result),
            _ => TypedResults.BadRequest(result),
        };
    }
}
