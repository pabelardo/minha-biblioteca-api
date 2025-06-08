using MinhaBiblioteca.Api.Common.Api;
using MinhaBiblioteca.Api.DTOs;
using MinhaBiblioteca.Api.Handlers.Interfaces;
using MinhaBiblioteca.Core.Requests.Genres;
using MinhaBiblioteca.Core.Responses;

namespace MinhaBiblioteca.Api.Endpoints.Genres;

public class CreateGenreEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) =>
        app.MapPost("/", HandleAsync)
           .WithName("Gêneros: Criar")
           .WithSummary("Cria um novo gênero")
           .WithDescription("Cria um novo gênero")
           .WithOrder(1)
           .Produces<Response<GenreDto?>>(201)
           .Produces<Response<GenreDto?>>(400);

    private static async Task<IResult> HandleAsync(
        IGenreHandler handler,
        CreateGenreRequest request)
    {
        var result = await handler.CreateAsync(request);

        return result.Success
            ? TypedResults.Created($"/{result.Data?.Id}", result)
            : TypedResults.BadRequest(result);
    }
}
