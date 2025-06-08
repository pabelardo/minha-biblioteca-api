using MinhaBiblioteca.Api.DTOs;
using MinhaBiblioteca.Core.Requests.Books;
using MinhaBiblioteca.Core.Responses;

namespace MinhaBiblioteca.Api.Handlers.Interfaces;

public interface IBookHandler
{
    Task<Response<BookDto?>> CreateAsync(CreateBookRequest request);
    Task<Response<BookDto?>> GetByIdAsync(GetBookByIdRequest request);
    Task<Response<IEnumerable<BookDto?>>> GetAllAsync(GetAllBooksRequest request);
    Task<Response<BookDto?>> UpdateAsync(Guid id, UpdateBookRequest request);
    Task<Response<BookDto?>> DeleteAsync(DeleteBookRequest request);
}
