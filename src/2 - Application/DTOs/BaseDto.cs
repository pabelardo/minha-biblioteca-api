using System.Text.Json.Serialization;

namespace MinhaBiblioteca.Application.DTOs;

public abstract class BaseDto<T>
{
    [JsonPropertyName("id")]
    public T Id { get; set; }
}
