using MinhaBiblioteca.Core.Interfaces.Response;

namespace MinhaBiblioteca.Core.Interfaces.Services;

public interface IGenericService<TEntity, TDto>
{
    Task<IAppResponse> AdicionarAsync(TDto dto);
    Task<IAppResponse> AtualizarAsync(Guid id, TDto dto);
    Task<IAppResponse> ObterPorIdAsync(Guid id);
    Task<IAppResponse> ObterTodosAsync(bool semTrackear = false);
    Task<IAppResponse> ExcluirAsync(Guid id);
}
