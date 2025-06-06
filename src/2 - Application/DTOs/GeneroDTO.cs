using MinhaBiblioteca.Domain.Entities;
using System.Text.Json.Serialization;

namespace MinhaBiblioteca.Application.DTOs;

public class GeneroDto : BaseDto<Guid>
{
    [JsonPropertyName("nome")]
    public required string Nome { get; set; }

    [JsonPropertyName("descricao")]
    public required string Descricao { get; set; }

    [JsonIgnore]
    public ICollection<Livro> Livros { get; set; } = [];
}
