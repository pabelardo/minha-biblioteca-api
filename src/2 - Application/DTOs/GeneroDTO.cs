using MinhaBiblioteca.Domain.Entities;
using System.Text.Json.Serialization;

namespace MinhaBiblioteca.Application.DTOs;

public class GeneroDTO
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("nome")]
    public required string Nome { get; set; }

    [JsonPropertyName("descricao")]
    public required string Descricao { get; set; }

    [JsonIgnore]
    public ICollection<Livro> Livros { get; set; } = [];
}
