using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MinhaBiblioteca.Api.Common.Mappings;
using MinhaBiblioteca.Api.Data.Repositories;
using MinhaBiblioteca.Core.Enums;
using MinhaBiblioteca.Core.Handlers;
using MinhaBiblioteca.Core.Models;
using MinhaBiblioteca.Core.Requests.Books;
using MinhaBiblioteca.Core.Responses;

namespace MinhaBiblioteca.Api.Handlers;

public class BookHandler(BookRepository repository, IValidator<CreateBookRequest> validatorPost, IValidator<UpdateBookRequest> validatorPut) : IBookHandler
{
    public async Task<Response<Book?>> CreateAsync(CreateBookRequest request)
    {
        try
        {
            var validationResult = validatorPost.Validate(request);

            if(!validationResult.IsValid)
                return new Response<Book?>(
                    null,
                    (int)StatusCodeEnum.BadRequest,
                    "Não foi possível criar o livro",
                    validationResult.Errors.Select(e => e.ErrorMessage));

            var book = await repository.AddAsync(request.ToEntity());

            return new Response<Book?>(book, (int)StatusCodeEnum.Created, "Categoria criada com sucesso !");
        }
        catch
        {
            return new Response<Book?>(null, (int)StatusCodeEnum.InternalServerError, "Não foi possível criar o livro");
        }
    }

    public async Task<Response<IEnumerable<Book?>>> GetAllAsync(GetAllBooksRequest request)
    {
        try
        {
            var query = await repository.GetQueryable();

            var books = await query.Skip((request.PageNumber - 1) * request.PageSize)
                             .Take(request.PageSize)
                             .ToListAsync();

            var count = await query.CountAsync();

            return new PagedResponse<IEnumerable<Book?>>(
                books,
                count,
                request.PageNumber,
                request.PageSize);
        }
        catch
        {
            return new Response<IEnumerable<Book?>>(null, (int)StatusCodeEnum.InternalServerError, "Não foi possível obter os livros");
        }
    }

    public async Task<Response<Book?>> GetByIdAsync(GetBookByIdRequest request)
    {
        try
        {
            var book = await repository.GetByIdAsync(request.Id);

            return book is null                 
                ? new Response<Book?>(null, (int)StatusCodeEnum.NotFound, "Livro não encontrado !")
                : new Response<Book?>(book);
        }
        catch
        {
            return new Response<Book?> (null, (int)StatusCodeEnum.InternalServerError, "Não foi possível obter o livro");
        }
    }

    public async Task<Response<Book?>> UpdateAsync(UpdateBookRequest request)
    {
        try
        {
            var book = await repository.GetByIdAsync(request.Id);

            if (book is null)
                return new Response<Book?>(null, (int)StatusCodeEnum.NotFound, "Livro não encontrado !");

            var validationResult = validatorPut.Validate(request);

            if (!validationResult.IsValid)
                return new Response<Book?>(
                    null,
                    (int)StatusCodeEnum.BadRequest,
                    "Não foi possível atualizar o livro",
                    validationResult.Errors.Select(e => e.ErrorMessage));

            var bookUpdated = await repository.UpdateAsync(request.ToEntity());

            return new Response<Book?>(bookUpdated, message: "Livro atualizado com sucesso !");
        }
        catch
        {
            return new Response<Book?>(null, (int)StatusCodeEnum.InternalServerError, "Não foi possível atualizar o livro");
        }
    }

    public async Task<Response<Book?>> DeleteAsync(DeleteBookRequest request)
    {
        try
        {
            var book = await repository.GetByIdAsync(request.Id);

            if(book is null)
                return new Response<Book?>(null, (int)StatusCodeEnum.NotFound, "Livro não encontrado !");

            await repository.RemoveAsync(request.Id);

            return new Response<Book?>(book, message: "Livro excluído com sucesso !");
        }
        catch
        {
            return new Response<Book?>(null, (int)StatusCodeEnum.InternalServerError, "Não foi possível excluir o livro");
        }
    }
}
