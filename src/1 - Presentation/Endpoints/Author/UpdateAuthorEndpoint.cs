using MinhaBiblioteca.Api.Common.Api;
using MinhaBiblioteca.Api.DTOs;
using MinhaBiblioteca.Api.Handlers.Interfaces;
using MinhaBiblioteca.Core.Enums;
using MinhaBiblioteca.Core.Requests.Authors;
using MinhaBiblioteca.Core.Responses;

namespace MinhaBiblioteca.Api.Endpoints.Author;

public class UpdateAuthorEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) =>
        app.MapPut("/{id:guid}", HandleAsync)
           .WithName("Autor: Atualizar")
           .WithSummary("Atualiza um autor")
           .WithDescription("Atualiza um autor")
           .WithOrder(4)
           .Produces<Response<AuthorDto>>()
           .Produces<Response<AuthorDto>>(400)
           .Produces<Response<AuthorDto>>(404);

    private static async Task<IResult> HandleAsync(
        IAuthorHandler handler,
        UpdateAuthorRequest request,
        Guid id)
    {
        request.Id = id;

        var result = await handler.UpdateAsync(id, request);

        return result.StatusCode switch
        {
            (int)StatusCodeEnum.OK => TypedResults.Ok(result),
            (int)StatusCodeEnum.NotFound => TypedResults.NotFound(result),
            _ => TypedResults.BadRequest(result),
        };
    }
}
