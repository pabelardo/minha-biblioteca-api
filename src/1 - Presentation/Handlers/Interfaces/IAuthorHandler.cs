using MinhaBiblioteca.Api.DTOs;
using MinhaBiblioteca.Core.Requests.Authors;
using MinhaBiblioteca.Core.Responses;

namespace MinhaBiblioteca.Api.Handlers.Interfaces;

public interface IAuthorHandler
{
    Task<Response<AuthorDto?>> CreateAsync(CreateAuthorRequest request);
    Task<Response<AuthorDto?>> GetByIdAsync(GetAuthorByIdRequest request);
    Task<Response<IEnumerable<AuthorDto?>>> GetAllAsync(GetAllAuthorsRequest request);
    Task<Response<AuthorDto?>> UpdateAsync(Guid id, UpdateAuthorRequest request);
    Task<Response<AuthorDto?>> DeleteAsync(DeleteAuthorRequest request);
}
