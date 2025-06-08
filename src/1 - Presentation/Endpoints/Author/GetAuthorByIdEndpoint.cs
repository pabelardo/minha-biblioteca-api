using MinhaBiblioteca.Api.Common.Api;
using MinhaBiblioteca.Api.DTOs;
using MinhaBiblioteca.Api.Handlers.Interfaces;
using MinhaBiblioteca.Core.Enums;
using MinhaBiblioteca.Core.Requests.Authors;
using MinhaBiblioteca.Core.Responses;

namespace MinhaBiblioteca.Api.Endpoints.Author;

public class GetAuthorByIdEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) =>
        app.MapGet("/{id:guid}", HandleAsync)
           .WithName("Autor: Obter Por Id")
           .WithSummary("Recupera um autor")
           .WithDescription("Recupera um autor")
           .WithOrder(3)
           .Produces<Response<AuthorDto?>>()
           .Produces<Response<AuthorDto?>>(400)
           .Produces<Response<AuthorDto?>>(404);

    private static async Task<IResult> HandleAsync(
        IAuthorHandler handler,
        Guid id)
    {
        var request = new GetAuthorByIdRequest { Id = id };

        var result = await handler.GetByIdAsync(request);

        return result.StatusCode switch
        {
            (int)StatusCodeEnum.OK => TypedResults.Ok(result),
            (int)StatusCodeEnum.NotFound => TypedResults.NotFound(result),
            _ => TypedResults.BadRequest(result),
        };
    }
}
