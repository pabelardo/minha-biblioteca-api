using MinhaBiblioteca.Api.Common.Api;
using MinhaBiblioteca.Api.DTOs;
using MinhaBiblioteca.Api.Handlers.Interfaces;
using MinhaBiblioteca.Core.Enums;
using MinhaBiblioteca.Core.Requests.Authors;
using MinhaBiblioteca.Core.Responses;

namespace MinhaBiblioteca.Api.Endpoints.Author;

public class DeleteAuthorEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) =>
        app.MapDelete("/{id:guid}", HandleAsync)
           .WithOrder(5)
           .WithName("Autor: Excluir")
           .WithSummary("Exclui um autor")
           .WithDescription("Exclui um autor")
           .Produces<Response<AuthorDto>>()
           .Produces<Response<AuthorDto>>(400)
           .Produces<Response<AuthorDto>>(404);

    private static async Task<IResult> HandleAsync(
        IAuthorHandler handler,
        Guid id)
    {
        var request = new DeleteAuthorRequest { Id = id };

        var result = await handler.DeleteAsync(request);

        return result.StatusCode switch
        {
            (int)StatusCodeEnum.OK => TypedResults.Ok(result),
            (int)StatusCodeEnum.NotFound => TypedResults.NotFound(result),
            _ => TypedResults.BadRequest(result),
        };
    }
}
