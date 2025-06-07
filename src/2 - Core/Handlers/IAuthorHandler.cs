using MinhaBiblioteca.Core.Models;
using MinhaBiblioteca.Core.Requests.Authors;
using MinhaBiblioteca.Core.Responses;

namespace MinhaBiblioteca.Core.Handlers;

public interface IAuthorHandler
{
    Task<Response<Author?>> CreateAsync(CreateAuthorRequest request);
    Task<Response<Author?>> GetByIdAsync(GetAuthorByIdRequest request);
    Task<Response<IEnumerable<Author?>>> GetAllAsync(GetAllAuthorsRequest request);
    Task<Response<Author?>> UpdateAsync(UpdateAuthorRequest request);
    Task<Response<Author?>> DeleteAsync(DeleteAuthorRequest request);
}
