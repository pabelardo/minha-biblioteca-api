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

public class AuthorHandler(IAuthorRepository repository) : IAuthorHandler
{
    public async Task<Response<AuthorDto?>> CreateAsync(CreateAuthorRequest request)
    {
        try
        {
            var validationResult = await request.GetValidationResult();

            if (!validationResult.IsValid)
                return new Response<AuthorDto?>(
                    null,
                    (int)StatusCodeEnum.BadRequest,
                    "Não foi possível criar o autor",
                    validationResult.Errors.Select(e => e.ErrorMessage));

            var author = await repository.AddAsync(request.ToEntity());

            return new Response<AuthorDto?>(author.ToDto(), (int)StatusCodeEnum.Created, "Autor criado com sucesso !");
        }
        catch (Exception)
        {
            return new Response<AuthorDto?>(null, (int)StatusCodeEnum.InternalServerError, "Não foi possível criar o autor");
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
            return new Response<IEnumerable<AuthorDto?>>(null, (int)StatusCodeEnum.InternalServerError, "Não foi possível obter os autores");
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
        catch(Exception ex)
        {
            return new Response<AuthorDto?>(null, (int)StatusCodeEnum.InternalServerError, "Não foi possível obter o autor");
        }
    }

    public async Task<Response<AuthorDto?>> UpdateAsync(Guid id, UpdateAuthorRequest request)
    {
        try
        {
            if(request.Id != id)
                return new Response<AuthorDto?>(null, (int)StatusCodeEnum.BadRequest, "O ID do autor não corresponde ao ID fornecido na URL.");

            var author = await repository.GetByIdAsync(request.Id);

            if (author is null)
                return new Response<AuthorDto?>(null, (int)StatusCodeEnum.NotFound, "Autor não encontrado !");

            var validationResult = await request.GetValidationResult();

            if (!validationResult.IsValid)
                return new Response<AuthorDto?>(
                    null,
                    (int)StatusCodeEnum.BadRequest,
                    "Não foi possível atualizar o autor",
                    validationResult.Errors.Select(e => e.ErrorMessage));

            var authorUpdated = await repository.UpdateAsync(request.ToEntity());

            return new Response<AuthorDto?>(authorUpdated.ToDto(), message: "Autor atualizado com sucesso !");
        }
        catch
        {
            return new Response<AuthorDto?>(null, (int)StatusCodeEnum.InternalServerError, "Não foi possível atualizar o Autor");
        }
    }

    public async Task<Response<AuthorDto?>> DeleteAsync(DeleteAuthorRequest request)
    {
        try
        {
            var author = await repository.GetByIdAsync(request.Id);

            if (author is null)
                return new Response<AuthorDto?>(null, (int)StatusCodeEnum.NotFound, "Autor não encontrado !");

            await repository.RemoveAsync(request.Id);

            return new Response<AuthorDto?>(author.ToDto(), message: "Autor excluído com sucesso !");
        }
        catch
        {
            return new Response<AuthorDto?>(null, (int)StatusCodeEnum.InternalServerError, "Não foi possível excluir o Autor");
        }
    }
}
