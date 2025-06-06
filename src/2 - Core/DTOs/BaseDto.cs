using System.Text.Json.Serialization;

namespace MinhaBiblioteca.Core.DTOs;

public abstract class BaseDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
}
