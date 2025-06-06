using LanguageExt;
using Microsoft.EntityFrameworkCore;
using MinhaBiblioteca.Data.Context;
using MinhaBiblioteca.Domain.Entities;
using MinhaBiblioteca.Domain.Exceptions;
using MinhaBiblioteca.Domain.Interfaces.Repositories;
using System.Data.Common;

namespace MinhaBiblioteca.Data.Repositories;

public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity, new()
{
    protected readonly MeuDbContext Db;
    protected readonly DbSet<TEntity> DbSet;
    private const string ERRO_INSERCAO = "Ocorreu um erro ao tentar inserir entidade na base de dados. Detalhes: [{0}]";
    private const string ERRO_ATUALIZACAO = "Ocorreu um erro ao tentar atualizar a entidade na base de dados. Detalhes: [{0}]";
    private const string ERRO_EXCLUSAO = "Ocorreu um erro ao tentar excluir a entidade na base de dados. Detalhes: [{0}]";
    private const string ERRO_CONCORRENCIA_INSERIR = "Ocorreu um erro de concorrência ao tentar inserir entidade na base de dados. Detalhes: [{0}]";
    private const string ERRO_CONCORRENCIA_ATUALIZAR = "Ocorreu um erro de concorrência ao tentar atualizar entidade na base de dados. Detalhes: [{0}]";
    private const string ERRO_CONCORRENCIA_EXCLUIR = "Ocorreu um erro de concorrência ao tentar excluir entidade na base de dados. Detalhes: [{0}]";
    private const string ERRO_GENERICO = "Ocorreu um erro. Detalhes: [{0}]";

    protected Repository(MeuDbContext db)
    {
        Db = db;
        DbSet = db.Set<TEntity>();
    }

    public virtual async Task<IEnumerable<TEntity>> ObterTodosAsync(bool semTrackear = false)
    {
        try
        {
            if (semTrackear)
                return await DbSet.AsNoTrackingWithIdentityResolution().ToListAsync();

            return await DbSet.ToListAsync();
        }
        catch (Exception ex)
        {
            throw new RepositoryException(string.Format(ERRO_GENERICO, ex.Message), ex);
        }
    }

    public virtual async Task<TEntity?> ObterPorIdAsync(Guid id)
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

    public virtual async Task AdicionarAsync(TEntity entity)
    {
        try
        {
            await DbSet.AddAsync(entity);

            await SalvarAlteracoesAsync();
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

    public virtual async Task<TEntity> AdicionarAsync(TEntity entity, bool retornarEntidade = true)
    {
        try
        {
            var result = await DbSet.AddAsync(entity);

            await SalvarAlteracoesAsync();

            return retornarEntidade ? result.Entity : null!;
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

    public virtual async Task<TEntity> AtualizarAsync(TEntity entity)
    {
        try
        {
            var result = DbSet.Update(entity);
            await SalvarAlteracoesAsync();
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

    public virtual async Task<TEntity> AtualizarAsync(Guid id, TEntity entity)
    {
        try
        {
            if (id != entity.Id)
                throw new RepositoryException("O id não é o mesmo que o objeto a ser atualizado.");

            var result = DbSet.Update(entity);

            await SalvarAlteracoesAsync();

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

    public async Task RemoverAsync(Guid id)
    {
        try
        {
            DbSet.Remove(new TEntity { Id = id });

            await SalvarAlteracoesAsync();
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

    //public async Task<bool> ExisteAsync(bool semTrackear = false)
    //{
    //    try
    //    {
    //        if (semTrackear)
    //            return await DbSet.AsNoTrackingWithIdentityResolution().AnyAsync();

    //        return await DbSet.AnyAsync();
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new RepositoryException(string.Format(ERRO_GENERICO, ex.Message), ex);
    //    }
    //}

    public Task<bool> ExisteAsync(Guid id, bool semTrackear = false)
    {
        try
        {
            if (semTrackear)
                return DbSet.AsNoTrackingWithIdentityResolution().AnyAsync(e => e.Id == id);

            return DbSet.AnyAsync(e => e.Id == id);
        }
        catch (Exception ex)
        {
            throw new RepositoryException(string.Format(ERRO_GENERICO, ex.Message), ex);
        }
    }

    public Task<int> SalvarAlteracoesAsync() => Db.SaveChangesAsync();

    public void Dispose() => GC.SuppressFinalize(this);
}
