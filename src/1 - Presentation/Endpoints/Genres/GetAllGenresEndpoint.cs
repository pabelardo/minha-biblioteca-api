using Microsoft.AspNetCore.Mvc;
using MinhaBiblioteca.Api.Common.Api;
using MinhaBiblioteca.Api.DTOs;
using MinhaBiblioteca.Api.Handlers.Interfaces;
using MinhaBiblioteca.Core.Config;
using MinhaBiblioteca.Core.Enums;
using MinhaBiblioteca.Core.Requests.Genres;
using MinhaBiblioteca.Core.Responses;

namespace MinhaBiblioteca.Api.Endpoints.Genres;

public class GetAllGenresEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) =>
        app.MapGet("/", HandleAsync)
           .WithName("Gêneros: Listar")
           .WithSummary("Lista todos os gêneros")
           .WithDescription("Lista todos os gêneros")
           .WithOrder(1)
           .Produces<Response<IEnumerable<GenreDto>>>()
           .Produces<Response<IEnumerable<GenreDto>>>(400);

    private static async Task<IResult> HandleAsync(
        IGenreHandler handler,
        [FromQuery] int pageNumber = Configuration.DefaultPageNumber,
        [FromQuery] int pageSize = Configuration.DefaultPageSize)
    {
        var request = new GetAllGenresRequest
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
        };

        var result = await handler.GetAllAsync(request);

        return result.StatusCode switch
        {
            (int)StatusCodeEnum.OK => TypedResults.Ok(result),
            _ => TypedResults.BadRequest(result),
        };
    }
}
