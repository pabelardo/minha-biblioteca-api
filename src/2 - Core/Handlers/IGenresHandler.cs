using MinhaBiblioteca.Core.Models;
using MinhaBiblioteca.Core.Requests.Genres;
using MinhaBiblioteca.Core.Responses;

namespace MinhaBiblioteca.Core.Handlers;

public interface IGenresHandler
{
    Task<Response<Genre?>> CreateAsync(CreateGenreRequest request);
    Task<Response<Genre?>> GetByIdAsync(GetGenreByIdRequest request);
    Task<Response<IEnumerable<Genre?>>> GetAllAsync(GetAllGenresRequest request);
    Task<Response<Genre?>> UpdateAsync(UpdateGenreRequest request);
    Task<Response<Genre?>> DeleteAsync(DeleteGenreRequest request);
}
