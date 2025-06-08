using MinhaBiblioteca.Api.Common.Api;
using MinhaBiblioteca.Api.DTOs;
using MinhaBiblioteca.Api.Handlers.Interfaces;
using MinhaBiblioteca.Core.Enums;
using MinhaBiblioteca.Core.Requests.Genres;
using MinhaBiblioteca.Core.Responses;

namespace MinhaBiblioteca.Api.Endpoints.Genres;

public class GetGenreByIdEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) =>
        app.MapGet("/{id:guid}", HandleAsync)
           .WithName("Gênero: Obter Por Id")
           .WithSummary("Recupera um gênero")
           .WithDescription("Recupera um gênero")
           .WithOrder(3)
           .Produces<Response<GenreDto?>>()
           .Produces<Response<GenreDto?>>(400)
           .Produces<Response<GenreDto?>>(404);

    private static async Task<IResult> HandleAsync(
        IGenreHandler handler,
        Guid id)
    {
        var request = new GetGenreByIdRequest { Id = id };

        var result = await handler.GetByIdAsync(request);

        return result.StatusCode switch
        {
            (int)StatusCodeEnum.OK => TypedResults.Ok(result),
            (int)StatusCodeEnum.NotFound => TypedResults.NotFound(result),
            _ => TypedResults.BadRequest(result),
        };
    }
}
