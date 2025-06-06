using MinhaBiblioteca.Application.Interfaces.Response;

namespace MinhaBiblioteca.Application.Interfaces.Services;

public interface IServicoGenerico<TEntity, TDto>
{
    Task<IAppResponse> AdicionarAsync(TDto dto);
    Task<IAppResponse> AtualizarAsync(Guid id, TDto dto);
    Task<IAppResponse> ObterPorIdAsync(Guid id);
    Task<IAppResponse> ObterTodosAsync(bool semTrackear = false);
    Task<IAppResponse> ExcluirAsync(Guid id);
}
