using Microsoft.AspNetCore.Mvc;
using MinhaBiblioteca.Api.Common.Api;
using MinhaBiblioteca.Api.DTOs;
using MinhaBiblioteca.Api.Handlers.Interfaces;
using MinhaBiblioteca.Core.Config;
using MinhaBiblioteca.Core.Enums;
using MinhaBiblioteca.Core.Requests.Books;
using MinhaBiblioteca.Core.Responses;

namespace MinhaBiblioteca.Api.Endpoints.Books;

public class GetAllBooksEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) =>
        app.MapGet("/", HandleAsync)
           .WithName("Livros: Obter Todos")
           .WithSummary("Lista todos os livros")
           .WithDescription("Lista todos os livros")
           .WithOrder(2)
           .Produces<Response<IEnumerable<BookDto?>>>()
           .Produces<Response<IEnumerable<BookDto?>>>(400);

    private static async Task<IResult> HandleAsync(
        IBookHandler handler,
        [FromQuery] int pageNumber = Configuration.DefaultPageNumber,
        [FromQuery] int pageSize = Configuration.DefaultPageSize)
    {
        var request = new GetAllBooksRequest
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
