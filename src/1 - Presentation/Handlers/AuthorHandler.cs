using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using MinhaBiblioteca.Api.Common.Extensions;
using MinhaBiblioteca.Api.Common.Mappings;
using MinhaBiblioteca.Api.DTOs;
using MinhaBiblioteca.Api.Handlers.Interfaces;
using MinhaBiblioteca.Core.Enums;
using MinhaBiblioteca.Core.Repositories;
using MinhaBiblioteca.Core.Requests.Authors;
using MinhaBiblioteca.Core.Responses;

namespace MinhaBiblioteca.Api.Handlers;

public class AuthorHandler(IAuthorRepository repository, ILogger<BookHandler> logger) : IAuthorHandler
{
    public async Task<Response<AuthorDto?>> CreateAsync(CreateAuthorRequest request)
    {
        try
        {
            var validationResult = await request.GetValidationResult();

            if (!await IsValidRequest(request, validationResult))
                return new Response<AuthorDto?>(null,
                                               (int)StatusCodeEnum.BadRequest,
                                               "Não foi possível criar o(a) autor(a)",
                                               validationResult.Errors.Select(e => e.ErrorMessage));

            if(await AuthorAlreadyExists(request))
                return new Response<AuthorDto?>(null,
                                               (int)StatusCodeEnum.BadRequest,
                                               "Já existe um(a) autor(a) com o mesmo nome");

            var author = await repository.AddAsync(request.ToEntity());

            return new Response<AuthorDto?>(author.ToDto(), (int)StatusCodeEnum.Created, "Autor(a) criado com sucesso !");
        }
        catch
        {
            return new Response<AuthorDto?>(null, (int)StatusCodeEnum.InternalServerError, "Não foi possível criar o(a) autor(a)");
        }
    }

    public async Task<Response<IEnumerable<AuthorDto?>>> GetAllAsync(GetAllAuthorsRequest request)
    {
        try
        {
            var query = await repository.GetQueryable();

            var authors = await query
                             .OrderBy(a => a.Name)
                             .Skip((request.PageNumber - 1) * request.PageSize)
                             .Take(request.PageSize)
                             .ToListAsync();

            var count = await query.CountAsync();

            return new PagedResponse<IEnumerable<AuthorDto?>>(
                authors.ToList(),
                count,
                request.PageNumber,
                request.PageSize);
        }
        catch
        {
            return new Response<IEnumerable<AuthorDto?>>(null, (int)StatusCodeEnum.InternalServerError, "Não foi possível obter o(a)s autore(a)s");
        }
    }

    public async Task<Response<AuthorDto?>> GetByIdAsync(GetAuthorByIdRequest request)
    {
        try
        {
            var author = await repository.GetByIdAsync(request.Id);

            return author is null
                ? new Response<AuthorDto?>(null, (int)StatusCodeEnum.NotFound, "Autor não encontrado !")
                : new Response<AuthorDto?>(author.ToDto());
        }
        catch
        {
            return new Response<AuthorDto?>(null, (int)StatusCodeEnum.InternalServerError, "Não foi possível obter o(a) autor(a)");
        }
    }

    public async Task<Response<AuthorDto?>> UpdateAsync(Guid id, UpdateAuthorRequest request)
    {
        try
        {
            if(request.Id != id)
                return new Response<AuthorDto?>(null, (int)StatusCodeEnum.BadRequest, "O ID do(a) autor(a) não corresponde ao ID fornecido na URL.");

            var validationResult = await request.GetValidationResult();

            if (!validationResult.IsValid)
                return new Response<AuthorDto?>(
                    null,
                    (int)StatusCodeEnum.BadRequest,
                    "Não foi possível atualizar o autor",
                    validationResult.Errors.Select(e => e.ErrorMessage));

            var author = await repository.GetByIdAsync(request.Id);

            if (author is null)
                return new Response<AuthorDto?>(null, (int)StatusCodeEnum.NotFound, "Autor(a) não encontrado(a) !");

            author.ChangeEntityProperties(request);

            var authorUpdated = await repository.UpdateAsync(author);

            return new Response<AuthorDto?>(authorUpdated.ToDto(), message: "Autor(a) atualizado(a) com sucesso !");
        }
        catch
        {
            return new Response<AuthorDto?>(null, (int)StatusCodeEnum.InternalServerError, "Não foi possível atualizar o(a) Autor(a)");
        }
    }

    public async Task<Response<AuthorDto?>> DeleteAsync(DeleteAuthorRequest request)
    {
        try
        {
            var author = await repository.GetByIdAsync(request.Id);

            if (author is null)
                return new Response<AuthorDto?>(null, (int)StatusCodeEnum.NotFound, "Autor não encontrado !");

            if (await repository.ExistsAsync(g => g.Books.Any(b => b.AuthorId == author.Id)))
                return new Response<AuthorDto?>(null, (int)StatusCodeEnum.BadRequest, $"Não é possível excluir o autor [{author.Name}], pois existem livros associados ao mesmo.");

            await repository.RemoveAsync(author);

            return new Response<AuthorDto?>(author.ToDto(), message: "Autor excluído com sucesso !");
        }
        catch
        {
            return new Response<AuthorDto?>(null, (int)StatusCodeEnum.InternalServerError, "Não foi possível excluir o(a) Autor(a)");
        }
    }

    private async Task<bool> IsValidRequest(CreateAuthorRequest request, ValidationResult validationResult)
    {
        logger.LogInformation("Iniciando validação do autor: {Name}", request.Name);

        if (!validationResult.IsValid)
        {
            logger.LogWarning("Validação falhou. Erros: {Errors}", validationResult.Errors);

            return await Task.FromResult(false);
        }

        logger.LogInformation("Válido.");

        return await Task.FromResult(true);
    }

    private async Task<bool> AuthorAlreadyExists(CreateAuthorRequest request)
    {
        logger.LogInformation("Verificando se o autor já existe: {Name}", request.Name);

        var exists = await repository.ExistsAsync(a => a.Name == request.Name);

        if (exists)
            logger.LogWarning("O autor já existe na base.");

        return exists;
    }
}
