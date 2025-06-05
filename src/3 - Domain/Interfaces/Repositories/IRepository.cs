using MinhaBiblioteca.Domain.Entities;

namespace MinhaBiblioteca.Domain.Interfaces.Repositories;

public interface IRepository<TEntity> : IDisposable where TEntity : Entity
{
    Task AdicionarAsync(TEntity entity);
    Task<TEntity> AdicionarAsync(TEntity entity, bool returnEntity = true);
    Task<TEntity?> ObterPorIdAsync(Guid id);
    Task<IEnumerable<TEntity>> ObterTodosAsync(bool semTrackear = false);
    Task<TEntity> AtualizarAsync(TEntity entity);
    Task<TEntity> AtualizarAsync(Guid id, TEntity entity);
    Task RemoverAsync(Guid id);
    Task<bool> ExisteAsync(Guid id, bool semTrackear = false);
    Task<int> SalvarAlteracoesAsync();
}
