namespace MinhaBiblioteca.Core.Interfaces.Response;

public interface IAppResponse
{
    bool Sucesso { get; set; }
    int CodigoDeStatus { get; set; }
    string? Mensagem { get; set; }
    object? Data { get; set; }
    string? ExcessaoInterna { get; set; }
    string? RastreamentoDePilha { get; set; }
    IEnumerable<string> Erros { get; set; }

    Task<IAppResponse> Ok(object? data = null, string? mensagem = null);

    Task<IAppResponse> BadRequest(IEnumerable<string>? mensagensDeErro = null, string? mensagem = null);

    Task<IAppResponse> NotFound(IEnumerable<string>? mensagensDeErro = null, string? mensagem = null);

    Task<IAppResponse> InternalServerError(Exception ex, string? mensagem = null);

    Task<IAppResponse> CriarResponseAsync(
        bool sucesso,
        int codigoDeStatus,
        string? mensagem = null,
        IEnumerable<string>? mensagensDeErro = null,
        object? data = null,
        string? erroInterno = null,
        string? rastreamentoDoErro = null);
}
