using MinhaBiblioteca.Core.Models;
using System.Linq.Expressions;

namespace MinhaBiblioteca.Core.Repositories;

public interface IRepository<TEntity> : IDisposable where TEntity : Entity
{
    Task<TEntity> AddAsync(TEntity entity);
    Task<TEntity?> GetByIdAsync(Guid id);
    Task<IEnumerable<TEntity>> GetAllAsync(bool noTracking = false);
    Task<IEnumerable<TEntity>> GetByFilterAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        params Expression<Func<TEntity, object>>[] includes);
    Task<TEntity> UpdateAsync(TEntity entity);
    Task RemoveAsync(Guid id);
    Task<IQueryable<TEntity>> GetQueryable(bool noTracking = false);
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
    Task<int> SaveChangesAsync();
}
