using Microsoft.AspNetCore.Mvc;
using MinhaBiblioteca.Application.DTOs;
using MinhaBiblioteca.Application.Interfaces.Response;
using MinhaBiblioteca.Application.Interfaces.Services;
using MinhaBiblioteca.Domain.Entities;
using MinhaBiblioteca.Domain.Enums;
using MinhaBiblioteca.Domain.Interfaces.Notificacoes;
using MinhaBiblioteca.Domain.Utils;

namespace MinhaBiblioteca.Api.Controllers;

[ApiController]
public class BaseController<TEntity, TDto> : ControllerBase
    where TEntity : Entity, new()
    where TDto : BaseDto<Guid>
{
    protected readonly INotificador _notificador;
    protected readonly INotificacao _notificacao;
    protected readonly IAppResponse _appResponse;
    protected readonly IServicoGenerico<TEntity, TDto> _servicoGenerico;

    public BaseController(
        INotificador notificador,
        INotificacao notificacao,
        IAppResponse appResponse,
        IServicoGenerico<TEntity, TDto> servicoGenerico)
    {
        _notificador = notificador;
        _notificacao = notificacao;
        _appResponse = appResponse;
        _servicoGenerico = servicoGenerico;
    }

    protected bool OperacaoValida() => !_notificador.TemNotificacao();

    protected void Notificar(string mensagem) => _notificador.Handle(_notificacao.CriarNotificacao(mensagem));

    protected IActionResult RespostaPersonalizada(IAppResponse response)
    {
        if(OperacaoValida()) return Ok(response);

        if (!response.Erros.Any() && _notificador.TemNotificacao())
        {
            response.Erros = _notificador.ObterNotificacoes().Select(n => n.Mensagem);

            if(response.Sucesso)
                response.Sucesso = false;
        }

        return response.CodigoDeStatus switch
        {
            (int)StatusCodeEnum.BadRequest => BadRequest(response),
            (int)StatusCodeEnum.NotFound => NotFound(response),
            (int)StatusCodeEnum.InternalServerError => Problem(
                response.ExcessaoInterna,
                HttpContext.Request.HttpContext.Request.Path,
                (int)StatusCodeEnum.InternalServerError,
                response.Erros.UnificarStrings()),
            _ => BadRequest(response)
        };
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    protected async Task<IActionResult> Post([FromBody] TDto dto)
    {
        var response = await _servicoGenerico.AdicionarAsync(dto);

        return RespostaPersonalizada(response);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    protected async Task<IActionResult> Get(bool semTrackear = true)
    {
        var response = await _servicoGenerico.ObterTodosAsync(semTrackear);

        return RespostaPersonalizada(response);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    protected async Task<IActionResult> Get(Guid id)
    {
        var response = await _servicoGenerico.ObterPorIdAsync(id);

        return RespostaPersonalizada(response);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    protected async Task<IActionResult> Put(Guid id, [FromBody] TDto dto)
    {
        var response = await _servicoGenerico.AtualizarAsync(id, dto);

        return RespostaPersonalizada(response);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    protected async Task<IActionResult> Delete(Guid id)
    {
        var response = await _servicoGenerico.ExcluirAsync(id);

        return RespostaPersonalizada(response);
    }

}
