using LanguageExt;
using MinhaBiblioteca.Domain.Entities;

namespace MinhaBiblioteca.Domain.Interfaces.Base;

public interface IBaseService<TEntity> where TEntity : Entity
{
    Task AdicionarAsync(TEntity entity);
    Task<Option<TEntity>> AdicionarAsync(TEntity entity, bool retornarEntidade = true);
    Task<Option<TEntity>> ObterPorIdAsync(Guid id);
    Task<IEnumerable<TEntity>> ObterTodosAsync(bool semTrackear = false);
    Task<TEntity> AtualizarAsync(TEntity entity);
    Task<Option<TEntity>> AtualizarAsync(Guid id, TEntity entity);
    Task<Option<bool>> RemoverAsync(Guid id);
}
