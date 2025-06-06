using MinhaBiblioteca.Core.Entities;
using System.Text.Json.Serialization;

namespace MinhaBiblioteca.Core.DTOs;

public class AutorDto : BaseDto
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
