using MinhaBiblioteca.Domain.Entities;
using System.Text.Json.Serialization;

namespace MinhaBiblioteca.Application.DTOs;

public class LivroDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("nome")]
    public required string Nome { get; set; }

    [JsonPropertyName("generoId")]
    public Guid GeneroId { get; set; }

    [JsonPropertyName("autorId")]
    public Guid AutorId { get; set; }

    [JsonPropertyName("dataCadastro")]
    public DateTime DataCadastro { get; set; }

    [JsonPropertyName("autor")]
    public required AutorDTO Autor { get; set; }

    [JsonPropertyName("genero")]
    public required GeneroDTO Genero { get; set; }

    public static implicit operator Livro(LivroDto dto)
    {
        return new Livro
        {
            Id = dto.Id,
            Nome = dto.Nome,
            GeneroId = dto.GeneroId,
            AutorId = dto.AutorId,
            DataCadastro = dto.DataCadastro,
            Autor = dto.Autor,
            Genero = dto.Genero
        };
    }
}
