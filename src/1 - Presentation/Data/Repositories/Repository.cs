using Microsoft.EntityFrameworkCore;
using MinhaBiblioteca.Api.Data.Context;
using MinhaBiblioteca.Core.Exceptions;
using MinhaBiblioteca.Core.Models;
using MinhaBiblioteca.Core.Repositories;
using System.Data.Common;
using System.Linq.Expressions;

namespace MinhaBiblioteca.Api.Data.Repositories;

public abstract class Repository<TEntity>(AppDbContext db) : IRepository<TEntity> where TEntity : Entity, new()
{
    protected readonly AppDbContext Db = db;
    protected readonly DbSet<TEntity> DbSet = db.Set<TEntity>();
    private const string ERRO_INSERCAO = "Ocorreu um erro ao tentar inserir entidade na base de dados. Detalhes: [{0}]";
    private const string ERRO_ATUALIZACAO = "Ocorreu um erro ao tentar atualizar a entidade na base de dados. Detalhes: [{0}]";
    private const string ERRO_EXCLUSAO = "Ocorreu um erro ao tentar excluir a entidade na base de dados. Detalhes: [{0}]";
    private const string ERRO_CONCORRENCIA_INSERIR = "Ocorreu um erro de concorrência ao tentar inserir entidade na base de dados. Detalhes: [{0}]";
    private const string ERRO_CONCORRENCIA_ATUALIZAR = "Ocorreu um erro de concorrência ao tentar atualizar entidade na base de dados. Detalhes: [{0}]";
    private const string ERRO_CONCORRENCIA_EXCLUIR = "Ocorreu um erro de concorrência ao tentar excluir entidade na base de dados. Detalhes: [{0}]";
    private const string ERRO_GENERICO = "Ocorreu um erro. Detalhes: [{0}]";

    public virtual async Task<TEntity> AddAsync(TEntity entity)
    {
        try
        {
            var result = await DbSet.AddAsync(entity);

            await SaveChangesAsync();

            return result.Entity;
        }
        catch (DbUpdateException ex)
        {
            throw new RepositoryException(string.Format(ERRO_INSERCAO, ex.Message), ex);
        }
        catch (DbException ex)
        {
            throw new RepositoryException(string.Format(ERRO_CONCORRENCIA_INSERIR, ex.Message), ex);
        }
        catch (Exception ex)
        {
            throw new RepositoryException(string.Format(ERRO_GENERICO, ex.Message), ex);
        }
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(bool noTracking = false)
    {
        try
        {
            if (noTracking)
                return await DbSet.AsNoTrackingWithIdentityResolution().ToListAsync();

            return await DbSet.ToListAsync();
        }
        catch (Exception ex)
        {
            throw new RepositoryException(string.Format(ERRO_GENERICO, ex.Message), ex);
        }
    }

    public virtual async Task<TEntity?> GetByIdAsync(Guid id)
    {
        try
        {
            return await DbSet.FindAsync(id);
        }
        catch (Exception ex)
        {
            throw new RepositoryException(string.Format(ERRO_GENERICO, ex.Message), ex);
        }
    }

    public Task<IQueryable<TEntity>> GetQueryable(bool noTracking = false)
    {
        try
        {
            return Task.FromResult(DbSet.AsNoTrackingWithIdentityResolution());
        }
        catch (Exception ex)
        {
            throw new RepositoryException(string.Format(ERRO_GENERICO, ex.Message), ex);
        }
    }

    public virtual async Task<IEnumerable<TEntity>> GetByFilterAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = DbSet;

        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        return await query.ToListAsync();
    }

    public virtual async Task<TEntity> UpdateAsync(TEntity entity)
    {
        try
        {
            var result = DbSet.Update(entity);
            await SaveChangesAsync();
            return result.Entity;
        }
        catch (DbUpdateException ex)
        {
            throw new RepositoryException(string.Format(ERRO_ATUALIZACAO, ex.Message), ex);
        }
        catch (DbException ex)
        {
            throw new RepositoryException(string.Format(ERRO_CONCORRENCIA_ATUALIZAR, ex.Message), ex);
        }
        catch (Exception ex)
        {
            throw new RepositoryException(string.Format(ERRO_GENERICO, ex.Message), ex);
        }
    }

    public virtual async Task RemoveAsync(Guid id)
    {
        try
        {
            DbSet.Remove(new TEntity { Id = id });

            await SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new RepositoryException(string.Format(ERRO_EXCLUSAO, ex.Message), ex);
        }
        catch (DbException ex)
        {
            throw new RepositoryException(string.Format(ERRO_CONCORRENCIA_EXCLUIR, ex.Message), ex);
        }
        catch (Exception ex)
        {
            throw new RepositoryException(string.Format(ERRO_GENERICO, ex.Message), ex);
        }
    }

    public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            return await DbSet.AsNoTrackingWithIdentityResolution().AnyAsync(predicate);
        }
        catch (Exception ex)
        {
            throw new RepositoryException(string.Format(ERRO_GENERICO, ex.Message), ex);
        }
    }

    public virtual Task<int> SaveChangesAsync() => Db.SaveChangesAsync();

    public virtual void Dispose() => GC.SuppressFinalize(this);
}
