using MinhaBiblioteca.Core.Entities;
using System.Text.Json.Serialization;

namespace MinhaBiblioteca.Core.DTOs;

public class GeneroDto : BaseDto
{
    [JsonPropertyName("nome")]
    public required string Nome { get; set; }

    [JsonPropertyName("descricao")]
    public required string Descricao { get; set; }

    [JsonIgnore]
    public ICollection<Livro> Livros { get; set; } = [];
}
