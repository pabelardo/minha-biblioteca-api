using FluentValidation;
using FluentValidation.Results;
using MinhaBiblioteca.Application.Interfaces.Base;
using MinhaBiblioteca.Domain.Entities;
using MinhaBiblioteca.Domain.Exceptions;
using MinhaBiblioteca.Domain.Extensions;
using MinhaBiblioteca.Domain.Helpers;
using MinhaBiblioteca.Domain.Interfaces.Notificacoes;
using MinhaBiblioteca.Domain.Interfaces.Repositories;
using MinhaBiblioteca.Domain.Interfaces.Response;

namespace MinhaBiblioteca.Application.Services.Base;

public abstract class BaseAppService<TEntity, TDTO> : IBaseAppService<TEntity, TDTO>
    where TEntity : Entity
    where TDTO : class
{
    protected readonly INotificador _notificador;
    protected readonly INotificacao _notificacao;
    protected readonly IMapper _mapper;
    protected readonly IRepository<TEntity> _repository;
    protected readonly IApplicationResponse _applicationResponse;

    protected BaseAppService(
        INotificador notificador,
        INotificacao notificacao)
    {
        _notificador = notificador;
        _notificacao = notificacao;
    }

    protected void Notificar(ValidationResult validationResult)
    {
        foreach (var error in validationResult.Errors)
            Notificar(error.ErrorMessage);
    }

    protected void Notificar(string mensagem) => _notificador.Handle(_notificacao.CriarNotificacao(mensagem));

    protected async Task<bool> DtoEstaValido<T>(T obj) where T : class
    {
        try
        {
            Type genericType = typeof(IValidator<>).MakeGenericType(typeof(T));

            if (ConfigurationHelper.ObterServico(genericType) is not IValidator<T> validator)
            {
                Notificar("Validator não encontrado");
                return false;
            }

            var validation = await validator.ObterValidationResult(obj);

            if (validation.IsValid) return true;

            Notificar(validation);

            return false;
        }
        catch (Exception ex)
        {
            throw new ValidacaoException(ex.Message);
        }
    }


    public Task<TDTO> AdicionarAsync(TDTO dto)
    {
        throw new NotImplementedException();
    }

    public Task<TDTO> AtualizarAsync(TDTO dto)
    {
        throw new NotImplementedException();
    }

    public Task<TDTO?> ObterPorIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TDTO>> ObterTodosAsync(bool semTrackear = false)
    {
        throw new NotImplementedException();
    }

    public Task RemoverAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}
