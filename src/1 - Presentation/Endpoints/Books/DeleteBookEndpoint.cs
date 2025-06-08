using MinhaBiblioteca.Api.Common.Api;
using MinhaBiblioteca.Api.DTOs;
using MinhaBiblioteca.Api.Handlers.Interfaces;
using MinhaBiblioteca.Core.Enums;
using MinhaBiblioteca.Core.Requests.Books;
using MinhaBiblioteca.Core.Responses;

namespace MinhaBiblioteca.Api.Endpoints.Books;

public class DeleteBookEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) => 
        app.MapDelete("/{id:guid}", HandleAsync)
           .WithName("Livros: Excluir")
           .WithSummary("Exclui um livro")
           .WithDescription("Exclui um livro")
           .WithOrder(5)
           .Produces<Response<BookDto>>()
           .Produces<Response<BookDto>>(400)
           .Produces<Response<BookDto>>(404);

    private static async Task<IResult> HandleAsync(
        IBookHandler handler,
        Guid id)
    {
        var request = new DeleteBookRequest { Id = id };

        var result = await handler.DeleteAsync(request);

        return result.StatusCode switch
        {
            (int)StatusCodeEnum.OK => TypedResults.Ok(result),
            (int)StatusCodeEnum.NotFound => TypedResults.NotFound(result),
            _ => TypedResults.BadRequest(result),
        };
    }
}
