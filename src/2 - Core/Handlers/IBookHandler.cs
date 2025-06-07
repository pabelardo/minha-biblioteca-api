using MinhaBiblioteca.Core.Models;
using MinhaBiblioteca.Core.Requests.Books;
using MinhaBiblioteca.Core.Responses;

namespace MinhaBiblioteca.Core.Handlers;

public interface IBookHandler
{
    Task<Response<Book?>> CreateAsync(CreateBookRequest request);
    Task<Response<Book?>> GetByIdAsync(GetBookByIdRequest request);
    Task<Response<IEnumerable<Book?>>> GetAllAsync(GetAllBooksRequest request);
    Task<Response<Book?>> UpdateAsync(UpdateBookRequest request);
    Task<Response<Book?>> DeleteAsync(DeleteBookRequest request);
}
