using MinhaBiblioteca.Core.Config;
using System.Text.Json.Serialization;

namespace MinhaBiblioteca.Core.Responses;

public class Response<TData>
{
    private readonly int _code;

    public bool Success => _code is >= 200 and <= 299;

    public string? Message { get; set; }

    public TData? Data { get; set; }

    public IEnumerable<string>? Errors { get; set; }

    [JsonConstructor]
    public Response() => _code = Configuration.DefaultStatusCode;

    public Response(
        TData? data,
        int code = Configuration.DefaultStatusCode,
        string? message = null,
        IEnumerable<string>? errors = null)
    {
        Data = data;
        Message = message;
        _code = code;
        Errors = errors;
    }
}
