using MinhaBiblioteca.Core.Models;
using System.Linq.Expressions;

namespace MinhaBiblioteca.Core.Repositories;

public interface IRepository<TEntity> : IDisposable where TEntity : Entity
{
    Task<TEntity> AddAsync(TEntity entity);
    Task<TEntity?> GetByIdAsync(Guid id);
    Task<IEnumerable<TEntity>> GetAllAsync(bool noTracking = false);
    Task<TEntity> UpdateAsync(TEntity entity);
    Task RemoveAsync(Guid id);
    Task<IQueryable<TEntity>> GetQueryable(bool noTracking = false);
    Task<int> SaveChangesAsync();
}
