using MinhaBiblioteca.Api.Common.Api;
using MinhaBiblioteca.Api.DTOs;
using MinhaBiblioteca.Api.Handlers.Interfaces;
using MinhaBiblioteca.Core.Requests.Authors;
using MinhaBiblioteca.Core.Responses;

namespace MinhaBiblioteca.Api.Endpoints.Author;

public class CreateAuthorEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app) =>
        app.MapPost("/", HandleAsync)
           .WithName("Autor: Criar")
           .WithSummary("Cria um novo autor")
           .WithDescription("Cria um novo autor")
           .WithOrder(1)
           .Produces<Response<AuthorDto?>>(201)
           .Produces<Response<AuthorDto?>>(400); 

    private static async Task<IResult> HandleAsync(
        IAuthorHandler handler,
        CreateAuthorRequest request)
    {
        var result = await handler.CreateAsync(request);

        return result.Success
            ? TypedResults.Created($"/{result.Data?.Id}", result)
            : TypedResults.BadRequest(result);
    }
}
