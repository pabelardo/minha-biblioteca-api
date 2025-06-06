using MinhaBiblioteca.Application.Interfaces.Response;
using MinhaBiblioteca.Domain.Enums;
using System.Text.Json.Serialization;

namespace MinhaBiblioteca.Application.ResponsePadronizado;

public class AppResponse : IAppResponse
{
    [JsonPropertyName("sucesso")]
    public bool Sucesso { get; set; }

    [JsonPropertyName("codigoDeStatus")]
    public int CodigoDeStatus { get; set; }

    [JsonPropertyName("mensagem")]
    public string? Mensagem { get; set; }

    [JsonPropertyName("data")]
    public object? Data { get; set; }

    [JsonPropertyName("erros")]
    public IEnumerable<string> Erros { get; set; }

    [JsonIgnore]
    public string? ExcessaoInterna { get; set; }

    [JsonIgnore]
    public string? RastreamentoDePilha { get; set; }

    public AppResponse()
    {
        Sucesso = false;
        CodigoDeStatus = (int)StatusCodeEnum.BadRequest;
        Erros = [];
        Data = string.Empty;
    }

    public async Task<IAppResponse> Ok(object? data = null, string? mensagem = null)
    {
        Sucesso = true;
        Mensagem = mensagem ?? string.Empty;
        CodigoDeStatus = (int)StatusCodeEnum.OK;
        Data = data ?? new { };
        Erros = [];

        return await Task.FromResult(this);
    }

    public async Task<IAppResponse> BadRequest(IEnumerable<string>? mensagensDeErro = null, string? mensagem = null)
    {
        Sucesso = false;
        Mensagem = mensagem ?? string.Empty;
        CodigoDeStatus = (int)StatusCodeEnum.BadRequest;
        Data = string.Empty;
        Erros = mensagensDeErro ?? [];

        return await Task.FromResult(this);
    }

    public async Task<IAppResponse> NotFound(IEnumerable<string>? mensagensDeErro = null, string? mensagem = null)
    {
        Sucesso = false;
        Mensagem = mensagem ?? string.Empty;
        CodigoDeStatus = (int)StatusCodeEnum.NotFound;
        Data = string.Empty;
        Erros = mensagensDeErro ?? [];

        return await Task.FromResult(this);
    }

    public async Task<IAppResponse> InternalServerError(Exception ex, string? mensagem = null)
    {
        Sucesso = false;
        Mensagem = mensagem ?? string.Empty;
        CodigoDeStatus = (int)StatusCodeEnum.InternalServerError;
        Data = string.Empty;
        Erros = [ ex.Message ];
        ExcessaoInterna = ex.InnerException != null ? ex.InnerException.Message : string.Empty;
        RastreamentoDePilha = ex.StackTrace ?? string.Empty;

        return await Task.FromResult(this);
    }

    public async Task<IAppResponse> CriarResponseAsync(
        bool sucesso,
        int codigoDeStatus,
        string? mensagem = null,
        IEnumerable<string>? mensagensDeErro = null,
        object? data = null,
        string? erroInterno = null,
        string? rastreamentoDoErro = null)
    {
        Sucesso = sucesso;
        CodigoDeStatus = codigoDeStatus;
        Mensagem = mensagem ?? string.Empty;
        Erros = mensagensDeErro ?? [];
        Data = data ?? new { };
        ExcessaoInterna = erroInterno ?? string.Empty;
        RastreamentoDePilha = rastreamentoDoErro ?? string.Empty;

        return await Task.FromResult(this);
    }
}
