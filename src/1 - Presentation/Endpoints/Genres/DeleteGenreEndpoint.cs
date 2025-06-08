using MinhaBiblioteca.Api.Common.Api;
using MinhaBiblioteca.Api.DTOs;
using MinhaBiblioteca.Api.Handlers.Interfaces;
using MinhaBiblioteca.Core.Enums;
using MinhaBiblioteca.Core.Requests.Genres;
using MinhaBiblioteca.Core.Responses;

namespace MinhaBiblioteca.Api.Endpoints.Genres;

public class DeleteGenreEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) =>
        app.MapDelete("/{id:guid}", HandleAsync)
           .WithName("Gêneros: Excluir")
           .WithSummary("Exclui um gênero")
           .WithDescription("Exclui um gênero")
           .WithOrder(5)
           .Produces<Response<GenreDto>>()
           .Produces<Response<GenreDto>>(400)
           .Produces<Response<GenreDto>>(404);

    private static async Task<IResult> HandleAsync(
        IGenreHandler handler,
        Guid id)
    {
        var request = new DeleteGenreRequest { Id = id };

        var result = await handler.DeleteAsync(request);

        return result.StatusCode switch
        {
            (int)StatusCodeEnum.OK => TypedResults.Ok(result),
            (int)StatusCodeEnum.NotFound => TypedResults.NotFound(result),
            _ => TypedResults.BadRequest(result),
        };
    }
}
