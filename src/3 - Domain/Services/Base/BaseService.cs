using FluentValidation.Results;
using LanguageExt;
using MinhaBiblioteca.Domain.Entities;
using MinhaBiblioteca.Domain.Interfaces.Base;
using MinhaBiblioteca.Domain.Interfaces.Notificacoes;
using MinhaBiblioteca.Domain.Interfaces.Repositories;

namespace MinhaBiblioteca.Domain.Services.Base;

public abstract class BaseService<TEntity> : IBaseService<TEntity> where TEntity : Entity
{
    private readonly INotificador _notificador;
    private readonly INotificacao _notificacao;
    protected readonly IRepository<TEntity> _repository;

    protected BaseService(
        IRepository<TEntity> repository,
        INotificador notificador,
        INotificacao notificacao)
    {
        _repository = repository;
        _notificador = notificador;
        _notificacao = notificacao;
    }

    protected void Notificar(ValidationResult validationResult)
    {
        foreach (var error in validationResult.Errors)
            Notificar(error.ErrorMessage);
    }

    protected void Notificar(string mensagem) => _notificador.Handle(_notificacao.CriarNotificacao(mensagem));

    public async Task AdicionarAsync(TEntity entity) => await _repository.AdicionarAsync(entity);

    public async Task<Option<TEntity>> AdicionarAsync(TEntity entity, bool retornarEntidade = true)
    {
        try
        {
            return await _repository.AdicionarAsync(entity, retornarEntidade);
        }
        catch (Exception ex)
        {
            Notificar(ex.Message);
            return Option<TEntity>.None;
        }
    }

    public Task<IEnumerable<TEntity>> ObterTodosAsync(bool semTrackear = false) => _repository.ObterTodosAsync(semTrackear);

    public async Task<Option<TEntity>> ObterPorIdAsync(Guid id)
    {
        try
        {
            var entity = await _repository.ObterPorIdAsync(id);

            if (entity == null)
                return Option<TEntity>.None;

            return Option<TEntity>.Some(entity);
        }
        catch (Exception ex)
        {
            Notificar(ex.Message);
            return Option<TEntity>.None;
        }
    }

    public async Task<Option<bool>> RemoverAsync(Guid id)
    {
        try
        {
            if (!await _repository.ExisteAsync(id, true))
            {
                Notificar("O registro não pode ser excluído porque não foi encontrado.");
                return Option<bool>.None;
            }

            await _repository.RemoverAsync(id);

            return Option<bool>.Some(true);
        }
        catch (Exception ex)
        {
            Notificar(ex.Message);
            return Option<bool>.None;
        }
    }

    public async Task<TEntity> AtualizarAsync(TEntity entity) => await _repository.AtualizarAsync(entity);

    public async Task<Option<TEntity>> AtualizarAsync(Guid id, TEntity entity)
    {
        try
        {
            if (!await _repository.ExisteAsync(id, true))
            {
                Notificar("O registro não pode ser atualizado porque não foi encontrado.");
                return Option<TEntity>.None;
            }

            return await _repository.AtualizarAsync(id, entity);
        }
        catch (Exception ex)
        {
            Notificar(ex.Message);
            return Option<TEntity>.None;
        }
    }
}
