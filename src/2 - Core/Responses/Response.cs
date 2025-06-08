using MinhaBiblioteca.Core.Config;
using System.Text.Json.Serialization;

namespace MinhaBiblioteca.Core.Responses;

public class Response<TData>
{
    [JsonIgnore]
    public readonly int StatusCode;

    public bool Success => StatusCode is >= 200 and <= 299;

    public string? Message { get; set; }

    public TData? Data { get; set; }

    public IEnumerable<string>? Errors { get; set; }

    [JsonConstructor]
    public Response() => StatusCode = Configuration.DefaultStatusCode;

    public Response(
        TData? data,
        int code = Configuration.DefaultStatusCode,
        string? message = null,
        IEnumerable<string>? errors = null)
    {
        Data = data;
        Message = message ?? string.Empty;
        StatusCode = code;
        Errors = errors ?? [];
    }
}
