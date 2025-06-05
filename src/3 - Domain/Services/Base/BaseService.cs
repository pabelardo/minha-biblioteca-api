using FluentValidation.Results;
using MinhaBiblioteca.Domain.Entities;
using MinhaBiblioteca.Domain.Exceptions;
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

    //protected async Task<bool> Validar<T>(T obj) where T : class
    //{
    //    try
    //    {
    //        Type genericType = typeof(IValidator<>).MakeGenericType(typeof(T));

    //        if (ConfigurationHelper.ObterServico(genericType) is not IValidator<T> validator)
    //        {
    //            Notificar("Validator não encontrado");
    //            return false;
    //        }

    //        var validation = await validator.ObterValidationResult(obj);

    //        if (validation.IsValid) return true;

    //        Notificar(validation);

    //        return false;
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new ValidacaoException(ex.Message);
    //    }
    //}

    public Task AdicionarAsync(TEntity entity) => _repository.AdicionarAsync(entity);

    public Task<TEntity> AdicionarAsync(TEntity entity, bool returnEntity = true) => _repository.AdicionarAsync(entity, returnEntity);

    public Task<IEnumerable<TEntity>> ObterTodosAsync(bool semTrackear = false) => _repository.ObterTodosAsync(semTrackear);

    public Task<TEntity?> ObterPorIdAsync(Guid id) => _repository.ObterPorIdAsync(id);

    public async Task RemoverAsync(Guid id)
    {
        if(! await _repository.ExisteAsync(id, true))
        {
            Notificar("O registro não pode ser excluído porque não foi encontrado.");
            return;
        }

        await _repository.RemoverAsync(id);
    }

    public Task<TEntity> AtualizarAsync(TEntity entity) => _repository.AtualizarAsync(entity);

    public async Task<TEntity> AtualizarAsync(Guid id, TEntity entity)
    {
        if (id != entity.Id)
            throw new NotFoundException("O id passado na rota não possui o mesmo valor que a propriedade 'Id' do objeto a ser atualizado.");

        if(!await _repository.ExisteAsync(id, true))
        {
            Notificar("O registro não pode ser atualizado porque não foi encontrado.");
            return entity;
        }

        return await _repository.AtualizarAsync(id, entity);
    }
}
