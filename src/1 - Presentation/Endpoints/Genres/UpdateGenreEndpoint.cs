using MinhaBiblioteca.Api.Common.Api;
using MinhaBiblioteca.Api.DTOs;
using MinhaBiblioteca.Api.Handlers.Interfaces;
using MinhaBiblioteca.Core.Enums;
using MinhaBiblioteca.Core.Requests.Genres;
using MinhaBiblioteca.Core.Responses;

namespace MinhaBiblioteca.Api.Endpoints.Genres;

public class UpdateGenreEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) =>
        app.MapPut("/{id:guid}", HandleAsync)
           .WithName("Gêneros: Atualizar")
           .WithSummary("Atualiza um gênero")
           .WithDescription("Atualiza um gênero")
           .WithOrder(4)
           .Produces<Response<GenreDto>>()
           .Produces<Response<GenreDto>>(400)
           .Produces<Response<GenreDto>>(404);

    private static async Task<IResult> HandleAsync(
        IGenreHandler handler,
        UpdateGenreRequest request,
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
