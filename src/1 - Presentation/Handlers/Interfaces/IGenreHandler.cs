using MinhaBiblioteca.Api.DTOs;
using MinhaBiblioteca.Core.Requests.Genres;
using MinhaBiblioteca.Core.Responses;

namespace MinhaBiblioteca.Api.Handlers.Interfaces;

public interface IGenreHandler
{
    Task<Response<GenreDto?>> CreateAsync(CreateGenreRequest request);
    Task<Response<GenreDto?>> GetByIdAsync(GetGenreByIdRequest request);
    Task<Response<IEnumerable<GenreDto?>>> GetAllAsync(GetAllGenresRequest request);
    Task<Response<GenreDto?>> UpdateAsync(Guid id, UpdateGenreRequest request);
    Task<Response<GenreDto?>> DeleteAsync(DeleteGenreRequest request);
}
