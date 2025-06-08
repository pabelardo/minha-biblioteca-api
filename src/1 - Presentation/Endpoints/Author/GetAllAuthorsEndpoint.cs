using Microsoft.AspNetCore.Mvc;
using MinhaBiblioteca.Api.Common.Api;
using MinhaBiblioteca.Api.DTOs;
using MinhaBiblioteca.Api.Handlers.Interfaces;
using MinhaBiblioteca.Core.Config;
using MinhaBiblioteca.Core.Requests.Authors;
using MinhaBiblioteca.Core.Responses;

namespace MinhaBiblioteca.Api.Endpoints.Author;

public class GetAllAuthorsEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) =>
        app.MapGet("/", HandleAsync)
           .WithName("Autores: Obter Todos")
           .WithSummary("Lista todos os autores")
           .WithDescription("Lista todos os autores")
           .WithOrder(2)
           .Produces<Response<IEnumerable<AuthorDto?>>>()
           .Produces<Response<IEnumerable<AuthorDto?>>>(400);

    private static async Task<IResult> HandleAsync(
        IAuthorHandler handler,
        [FromQuery] int pageNumber = Configuration.DefaultPageNumber,
        [FromQuery] int pageSize = Configuration.DefaultPageSize)
    {
        var request = new GetAllAuthorsRequest
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
        };

        var result = await handler.GetAllAsync(request);

        return result.Success
            ? TypedResults.Ok(result)
            : TypedResults.BadRequest(result);
    }
}
