using MinhaBiblioteca.Domain.Entities;
using System.Text.Json.Serialization;

namespace MinhaBiblioteca.Application.DTOs;

public class AutorDto : BaseDto<Guid>
{
    [JsonPropertyName("nome")]
    public required string Nome { get; set; }

    [JsonPropertyName("email")]
    public required string Email { get; set; }

    [JsonPropertyName("telefone")]
    public required string Telefone { get; set; }

    [JsonIgnore]
    public ICollection<Livro> Livros { get; set; } = [];
}
