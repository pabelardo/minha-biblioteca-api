using FluentValidation;
using FluentValidation.Results;
using Mapster;
using MinhaBiblioteca.Application.DTOs;
using MinhaBiblioteca.Application.Interfaces.Response;
using MinhaBiblioteca.Application.Interfaces.Services;
using MinhaBiblioteca.Application.Mappings;
using MinhaBiblioteca.Domain.Entities;
using MinhaBiblioteca.Domain.Exceptions;
using MinhaBiblioteca.Domain.Extensions;
using MinhaBiblioteca.Domain.Interfaces.Notificacoes;
using MinhaBiblioteca.Domain.Interfaces.Repositories;
using MinhaBiblioteca.Domain.Utils;

namespace MinhaBiblioteca.Application.Services;

public class ServicoGenerico<TEntity, TDto> : IServicoGenerico<TEntity, TDto>
    where TEntity : Entity, new()
    where TDto : BaseDto<Guid>
{
    protected readonly INotificador _notificador;
    protected readonly INotificacao _notificacao;
    protected readonly IAppResponse _appResponse;
    private readonly IRepository<TEntity> _repository;

    public ServicoGenerico(
        INotificador notificador,
        INotificacao notificacao,
        IAppResponse appResponse,
        IRepository<TEntity> repository)
    {
        _notificador = notificador;
        _notificacao = notificacao;
        _appResponse = appResponse;
        _repository = repository;
    }

    protected void Notificar(ValidationResult validationResult)
    {
        foreach (var error in validationResult.Errors)
            Notificar(error.ErrorMessage);
    }

    protected void Notificar(string mensagem) => _notificador.Handle(_notificacao.CriarNotificacao(mensagem));

    protected async Task<bool> DtoEstaValido(TDto dto)
    {
        if (dto is null)
        {
            Notificar($"O objeto não pode ser nulo.");
            return false;
        }

        try
        {
            Type genericType = typeof(IValidator<>).MakeGenericType(typeof(TDto));

            if (ConfigurationHelper.ObterServico(genericType) is not IValidator<TDto> validator)
            {
                Notificar("Validator não encontrado");
                return false;
            }

            var validation = await validator.ObterValidationResult(dto);

            if (validation.IsValid) return true;

            Notificar(validation);

            return false;
        }
        catch (Exception ex)
        {
            throw new ValidacaoException(ex.Message);
        }
    }

    public async Task<IAppResponse> AdicionarAsync(TDto dto)
    {
        if (!await DtoEstaValido(dto))
            return await _appResponse.BadRequest(mensagem: $"Ops :( ... Dados inválidos no cadastro.");

        try
        {
            var entidade = dto.MapearParaEntidade<TEntity, TDto>();

            var entidadeInserida = await _repository.AdicionarAsync(entidade, true);

            return await _appResponse.Ok(entidadeInserida.MapearParaDto<TEntity, TDto>(), "Cadastro feito com sucesso.");
        }
        catch (Exception ex)
        {
            Notificar(ex.Message);
            return await _appResponse.BadRequest(mensagem: $"Ops :( ... Ocorreu um erro ao tentar cadastrar este registro.");
        }
    }

    public async Task<IAppResponse> ObterPorIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            Notificar("O id não pode ser nulo ou vazio.");
            return await _appResponse.BadRequest(mensagem: $"Ops :( ... Ocorreu um erro ao tentar buscar este registro.");
        }

        var entidade = await _repository.ObterPorIdAsync(id);

        if (entidade is null)
        {
            Notificar($"Não foi encontrado nenhum registro de ID [{id}].");
            return await _appResponse.NotFound(mensagem: $"Nenhum registro foi encontrado.");
        }

        return await _appResponse.Ok(entidade.MapearParaDto<TEntity, TDto>());
    }

    public async Task<IAppResponse> ObterTodosAsync(bool semTrackear = false)
    {
        var entidades = await _repository.ObterTodosAsync(semTrackear);

        return await _appResponse.Ok(entidades.MapearParaListaDto<TEntity, TDto>());
    }

    public async Task<IAppResponse> AtualizarAsync(Guid id, TDto dto)
    {
        if (id != dto.Id)
        {
            Notificar("O id passado na rota não possui o mesmo valor que a propriedade 'Id' do livro a ser atualizado.");
            return await _appResponse.BadRequest(mensagem: $"Ops :( ... Ocorreu um erro ao tentar atualizar este registro.");
        }

        if (!await DtoEstaValido(dto))
            return await _appResponse.BadRequest(mensagem: $"Ops :( ... Dados inválidos no cadastro.");

        try
        {
            if (!await _repository.ExisteAsync(id, true))
            {
                Notificar($"Não foi encontrado nenhum registro de ID [{id}] para atualização.");
                return await _appResponse.NotFound(mensagem: "Nenhum registro foi encontrado.");
            }

            var entidade = dto.Adapt<TEntity>();

            var entidadeAtualizada = await _repository.AtualizarAsync(id, dto.MapearParaEntidade<TEntity, TDto>());

            return await _appResponse.Ok(entidadeAtualizada.MapearParaDto<TEntity, TDto>(), "Atualização feita com sucesso.");
        }
        catch (Exception ex)
        {
            Notificar(ex.Message);
            return await _appResponse.BadRequest(mensagem: $"Ops :( ... Ocorreu um erro ao tentar atualizar este registro.");
        }
    }

    public async Task<IAppResponse> ExcluirAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            Notificar("O id não pode ser nulo ou vazio.");
            return await _appResponse.BadRequest(mensagem: "Não foi possível excluir o registro. Fale com o admin do sistema.");
        }

        if (!await _repository.ExisteAsync(id, true))
        {
            Notificar($"Não foi encontrado nenhum registro de ID [{id}] para exclusão.");
            return await _appResponse.BadRequest(mensagem: "Não foi possível excluir o registro, pois o mesmo já foi excluído ou não existe.");
        }

        try
        {
            await _repository.RemoverAsync(id);

            return await _appResponse.Ok(mensagem: "Registro excluído com sucesso.");
        }
        catch (Exception ex)
        {
            Notificar(ex.Message);
            return await _appResponse.BadRequest(mensagem: "Ops :( ... Ocorreu um erro ao tentar excluir esse registro.");
        }
    }
}
