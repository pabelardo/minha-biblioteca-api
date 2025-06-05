using Microsoft.EntityFrameworkCore;
using MinhaBiblioteca.Data.Context;
using MinhaBiblioteca.Domain.Entities;
using MinhaBiblioteca.Domain.Interfaces.Repositories;

namespace MinhaBiblioteca.Data.Repositories;

public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity, new()
{
    protected readonly MeuDbContext Db;
    protected readonly DbSet<TEntity> DbSet;

    protected Repository(MeuDbContext db)
    {
        Db = db;
        DbSet = db.Set<TEntity>();
    }

    public virtual async Task<IEnumerable<TEntity>> ObterTodosAsync(bool semTrackear = false)
    {
        if (semTrackear)
            return await DbSet.AsNoTrackingWithIdentityResolution().ToListAsync();

        return await DbSet.ToListAsync();
    }

    public virtual async Task<TEntity?> ObterPorIdAsync(Guid id) => await DbSet.FindAsync(id);

    public virtual async Task AdicionarAsync(TEntity entity)
    {
        await DbSet.AddAsync(entity);

        await SalvarAlteracoesAsync();
    }

    public virtual async Task<TEntity> AdicionarAsync(TEntity entity, bool returnEntity = true)
    {
        var result = await DbSet.AddAsync(entity);

        await SalvarAlteracoesAsync();

        return returnEntity ? result.Entity : null!;
    }

    public virtual async Task<TEntity> AtualizarAsync(TEntity entity)
    {
        var result = DbSet.Update(entity);
        await SalvarAlteracoesAsync();
        return result.Entity;
    }

    public virtual async Task<TEntity> AtualizarAsync(Guid id, TEntity entity)
    {
        if (id != entity.Id)
            throw new InvalidOperationException("O id não é o mesmo que o objeto a ser atualizado.");

        var result = DbSet.Update(entity);

        await SalvarAlteracoesAsync();

        return result.Entity;
    }

    public async Task RemoverAsync(Guid id)
    {
        DbSet.Remove(new TEntity { Id = id });

        await SalvarAlteracoesAsync();
    }

    public Task<bool> ExisteAsync(Guid id, bool semTrackear = false)
    {
        if(semTrackear)
            return DbSet.AsNoTrackingWithIdentityResolution().AnyAsync(e => e.Id == id);

        return DbSet.AnyAsync(e => e.Id == id);
    }

    public Task<int> SalvarAlteracoesAsync() => Db.SaveChangesAsync();

    public void Dispose() => GC.SuppressFinalize(this);
}
