using MinhaBiblioteca.Domain.Entities;

namespace MinhaBiblioteca.Application.Interfaces.Base;

public interface IBaseAppService<TEntity, TDTO>
    where TEntity : Entity
    where TDTO : class
{
    Task<TDTO> AdicionarAsync(TDTO dto);
    Task<TDTO> AtualizarAsync(TDTO dto);
    Task<TDTO?> ObterPorIdAsync(Guid id);
    Task<IEnumerable<TDTO>> ObterTodosAsync(bool semTrackear = false);
    Task RemoverAsync(Guid id);
}
