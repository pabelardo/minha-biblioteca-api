using Microsoft.EntityFrameworkCore;
using MinhaBiblioteca.Api.Common.Extensions;
using MinhaBiblioteca.Api.Common.Mappings;
using MinhaBiblioteca.Api.DTOs;
using MinhaBiblioteca.Api.Handlers.Interfaces;
using MinhaBiblioteca.Core.Enums;
using MinhaBiblioteca.Core.Repositories;
using MinhaBiblioteca.Core.Requests.Genres;
using MinhaBiblioteca.Core.Responses;

namespace MinhaBiblioteca.Api.Handlers;

public class GenreHandler(IGenreRepository repository) : IGenreHandler
{
    public async Task<Response<GenreDto?>> CreateAsync(CreateGenreRequest request)
    {
        try
        {
            var validationResult = await request.GetValidationResult();

            if (!validationResult.IsValid)
                return new Response<GenreDto?>(
                    null,
                    (int)StatusCodeEnum.BadRequest,
                    "Não foi possível criar o gênero",
                    validationResult.Errors.Select(e => e.ErrorMessage));

            var genre = await repository.AddAsync(request.ToEntity());

            return new Response<GenreDto?>(genre.ToDto(), (int)StatusCodeEnum.Created, "Gênero criado com sucesso !");
        }
        catch (Exception)
        {
            return new Response<GenreDto?>(null, (int)StatusCodeEnum.InternalServerError, "Não foi possível criar o gênero");
        }
    }

    public async Task<Response<IEnumerable<GenreDto?>>> GetAllAsync(GetAllGenresRequest request)
    {
        try
        {
            var query = await repository.GetQueryable(true);

            var genres = await query
                             .OrderBy(g => g.Name)
                             .Skip((request.PageNumber - 1) * request.PageSize)
                             .Take(request.PageSize)
                             .ToListAsync();

            var count = await query.CountAsync();

            return new PagedResponse<IEnumerable<GenreDto?>>(
                genres.ToList(),
                count,
                request.PageNumber,
                request.PageSize);
        }
        catch
        {
            return new Response<IEnumerable<GenreDto?>>(null, (int)StatusCodeEnum.InternalServerError, "Não foi possível obter os gêneros");
        }
    }

    public async Task<Response<GenreDto?>> GetByIdAsync(GetGenreByIdRequest request)
    {
        try
        {
            var genre = await repository.GetByIdAsync(request.Id);

            return genre is null
                ? new Response<GenreDto?>(null, (int)StatusCodeEnum.NotFound, "Gênero não encontrado !")
                : new Response<GenreDto?>(genre.ToDto());
        }
        catch
        {
            return new Response<GenreDto?>(null, (int)StatusCodeEnum.InternalServerError, "Não foi possível obter o gênero");
        }
    }

    public async Task<Response<GenreDto?>> UpdateAsync(Guid id, UpdateGenreRequest request)
    {
        try
        {
            if (id != request.Id)
                return new Response<GenreDto?>(null, (int)StatusCodeEnum.BadRequest, "O ID do gênero não corresponde ao ID fornecido na URL.");

            var validationResult = await request.GetValidationResult();

            if (!validationResult.IsValid)
                return new Response<GenreDto?>(
                    null,
                    (int)StatusCodeEnum.BadRequest,
                    "Não foi possível atualizar o gênero",
                    validationResult.Errors.Select(e => e.ErrorMessage));

            var genres = await repository.GetByIdAsync(request.Id);

            if (genres is null)
                return new Response<GenreDto?>(null, (int)StatusCodeEnum.NotFound, "Gênero não encontrado !");

            genres.ChangeEntityProperties(request);

            var authorUpdated = await repository.UpdateAsync(genres);

            return new Response<GenreDto?>(authorUpdated.ToDto(), message: "Gênero atualizado com sucesso !");
        }
        catch
        {
            return new Response<GenreDto?>(null, (int)StatusCodeEnum.InternalServerError, "Não foi possível atualizar o gênero");
        }
    }

    public async Task<Response<GenreDto?>> DeleteAsync(DeleteGenreRequest request)
    {
        try
        {
            var genre = await repository.GetByIdAsync(request.Id);

            if (genre is null)
                return new Response<GenreDto?>(null, (int)StatusCodeEnum.NotFound, "Gênero não encontrado !");

            if(await repository.ExistsAsync(g => g.Books.Any(b => b.GenreId == genre.Id)))
                return new Response<GenreDto?>(null, (int)StatusCodeEnum.BadRequest, $"Não é possível excluir o gênero [{genre.Name}], pois existem livros associados ao mesmo.");

            await repository.RemoveAsync(genre);

            return new Response<GenreDto?>(genre.ToDto(), message: "Gênero excluído com sucesso !");
        }
        catch
        {
            return new Response<GenreDto?>(null, (int)StatusCodeEnum.InternalServerError, "Não foi possível excluir o gênero");
        }
    }
}
