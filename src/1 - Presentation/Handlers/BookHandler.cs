using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using MinhaBiblioteca.Api.Common.Extensions;
using MinhaBiblioteca.Api.Common.Mappings;
using MinhaBiblioteca.Api.DTOs;
using MinhaBiblioteca.Api.Handlers.Interfaces;
using MinhaBiblioteca.Core.Enums;
using MinhaBiblioteca.Core.Repositories;
using MinhaBiblioteca.Core.Requests.Books;
using MinhaBiblioteca.Core.Responses;

namespace MinhaBiblioteca.Api.Handlers;

public class BookHandler(
    IBookRepository repository,
    IAuthorRepository authorRepository,
    IGenreRepository genreRepository,
    ILogger<BookHandler> logger) : IBookHandler
{
    public async Task<Response<BookDto?>> CreateAsync(CreateBookRequest request)
    {
        logger.LogInformation("Iniciando criação de Livro");

        try
        {
            var validationResult = await request.GetValidationResult();

            if (!await IsValidRequest(validationResult))
                return new Response<BookDto?>(null,
                                             (int)StatusCodeEnum.BadRequest,
                                             "Não foi possível criar o livro",
                                             validationResult.Errors.Select(e => e.ErrorMessage));

            if(await NotExistsAuthorAndGenre(request))
                return new Response<BookDto?>(null,
                                             (int)StatusCodeEnum.BadRequest,
                                             "Autor e gênero não foram encontrados na base");

            if(await ExistsBookWithAuthorAndGenre(request))
                return new Response<BookDto?>(null,
                                             (int)StatusCodeEnum.BadRequest,
                                             "Já existe um livro com o mesmo autor e gênero");

            var book = await repository.AddAsync(request.ToEntity());

            logger.LogInformation("Livro criado com sucesso: ID = [{Id}]", book?.Id);

            return new Response<BookDto?>(book?.ToDto(), (int)StatusCodeEnum.Created, "Livro criado com sucesso !");
        }
        catch(Exception ex)
        {
            logger.LogError(ex, "Erro ao criar o livro {Name}", request.Name);

            return new Response<BookDto?>(null, (int)StatusCodeEnum.InternalServerError, "Não foi possível criar o livro", [ex.Message]);
        }
    }

    public async Task<Response<IEnumerable<BookDto?>>> GetAllAsync(GetAllBooksRequest request)
    {
        logger.LogInformation("Iniciando obtenção de todos os livros de {PageNumber} até {PageSize}",
                              request.PageNumber,
                              request.PageSize);

        try
        {
            var query = await repository.GetQueryable();

            var books = await query
                             .OrderBy(b => b.Name)
                             .Skip((request.PageNumber - 1) * request.PageSize)
                             .Take(request.PageSize)
                             .ToListAsync();

            var count = await query.CountAsync();

            return new PagedResponse<IEnumerable<BookDto?>>(
                books.ToList(),
                count,
                request.PageNumber,
                request.PageSize);
        }
        catch(Exception ex)
        {
            logger.LogError(ex, "Erro ao obter os livros");

            return new Response<IEnumerable<BookDto?>>(null, (int)StatusCodeEnum.InternalServerError, "Não foi possível obter os livros", [ex.Message]);
        }
    }

    public async Task<Response<BookDto?>> GetByIdAsync(GetBookByIdRequest request)
    {
        logger.LogInformation("Iniciando obtenção do Livro de ID: [{Id}]", request.Id);

        try
        {
            var book = await repository.GetByIdAsync(request.Id);

            if(book is null)
            {
                logger.LogWarning("O livro de ID [{Id}] não foi encontrado.", request.Id);

                return new Response<BookDto?>(null, (int)StatusCodeEnum.NotFound, "Livro não encontrado !");
            }

            return new Response<BookDto?>(book.ToDto());
        }
        catch(Exception ex)
        {
            logger.LogError(ex, "Erro ao obter o livro de ID: [{Id}]", request.Id);

            return new Response<BookDto?>(null, (int)StatusCodeEnum.InternalServerError, "Não foi possível obter os livros", [ex.Message]);
        }
    }

    public async Task<Response<BookDto?>> UpdateAsync(Guid id, UpdateBookRequest request)
    {
        logger.LogInformation("Iniciando atualização de Livro");

        try
        {
            if(await IsRequestIdNotEqualToUrlId(id, request))
                return new Response<BookDto?>(null, (int)StatusCodeEnum.BadRequest, "O ID do livro não corresponde ao ID fornecido na URL");

            var validationResult = await request.GetValidationResult();

            if (!await IsValidRequest(validationResult))
                return new Response<BookDto?>(
                    null,
                    (int)StatusCodeEnum.BadRequest,
                    "Não foi possível atualizar o livro",
                    validationResult.Errors.Select(e => e.ErrorMessage));

            var book = await repository.GetByIdAsync(request.Id);

            if (book is null)
            {
                logger.LogWarning("O livro de ID [{Id}] não foi encontrado para atualização.", request.Id);

                return new Response<BookDto?>(null, (int)StatusCodeEnum.NotFound, "Livro não encontrado !");
            }

            book.ChangeEntityProperties(request);

            var bookUpdated = await repository.UpdateAsync(book);

            return new Response<BookDto?>(bookUpdated.ToDto(), message: "Livro atualizado com sucesso !");
        }
        catch(Exception ex)
        {
            logger.LogError(ex, "Erro ao atualizar o livro de ID: [{Id}]", request.Id);

            return new Response<BookDto?>(null, (int)StatusCodeEnum.InternalServerError, "Não foi possível atualizar o livro", [ex.Message]);
        }
    }

    public async Task<Response<BookDto?>> DeleteAsync(DeleteBookRequest request)
    {
        logger.LogInformation("Iniciando exclusão de Livro");

        try
        {
            var book = await repository.GetByIdAsync(request.Id);

            if(book is null)
            {
                logger.LogWarning("O livro de ID [{Id}] não foi encontrado para exclusão.", request.Id);

                return new Response<BookDto?>(null, (int)StatusCodeEnum.NotFound, "Livro não encontrado !");
            }

            await repository.RemoveAsync(book);

            logger.LogInformation("Livro excluído com sucesso: {Id}", request.Id);

            return new Response<BookDto?>(book.ToDto(), message: "Livro excluído com sucesso !");
        }
        catch(Exception ex)
        {
            logger.LogError(ex, "Erro ao excluir o livro de ID: [{Id}]", request.Id);

            return new Response<BookDto?>(null, (int)StatusCodeEnum.InternalServerError, "Não foi possível excluir o livro", [ex.Message]);
        }
    }

    private async Task<bool> IsRequestIdNotEqualToUrlId(Guid id, UpdateBookRequest request)
    {
        logger.LogInformation("Verificando se o ID do livro é igual ao ID fornecido na URL.");

        if (id != request.Id)
        {
            logger.LogWarning("O ID do livro na requisição [{Id}] não corresponde ao ID fornecido na URL [{Id}].", request.Id, id);
            return true;
        }

        return await Task.FromResult(false);
    }

    private async Task<bool> IsValidRequest(ValidationResult validationResult)
    {
        logger.LogInformation("Iniciando validação do livro");

        if (!validationResult.IsValid)
        {
            logger.LogWarning("Validação falhou. Erros: {Errors}", validationResult.Errors);

            return await Task.FromResult(false);
        }

        logger.LogInformation("Válido.");

        return await Task.FromResult(true);
    }

    private async Task<bool> NotExistsAuthorAndGenre(CreateBookRequest request)
    {
        logger.LogInformation("Validando se o autor e gênero existem na base.");

        if (!await authorRepository.ExistsAsync(a => a.Id == request.AuthorId) && !await genreRepository.ExistsAsync(g => g.Id == request.GenreId))
        {
            logger.LogWarning("O autor de ID [{AuthorId}] e o gênero de ID [{GenreId}] não existem na base de dados.", request.AuthorId, request.GenreId);

            return true;
        }

        return false;
    }

    private async Task<bool> ExistsBookWithAuthorAndGenre(CreateBookRequest request)
    {
        logger.LogInformation("Verificando se já existe um livro com o mesmo autor e gênero.");

        if (await repository.ExistsAsync(b => b.AuthorId == request.AuthorId && b.GenreId == request.GenreId))
        {
            logger.LogWarning("Já existe um livro com o autor de ID [{AuthorId}] e gênero de ID [{GenreId}].", request.AuthorId, request.GenreId);
            return true;
        }

        return false;
    }
}
