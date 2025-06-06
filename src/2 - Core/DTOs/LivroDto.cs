using System.Text.Json.Serialization;

namespace MinhaBiblioteca.Core.DTOs;

public class LivroDto : BaseDto
{
    [JsonPropertyName("nome")]
    public required string Nome { get; set; }

    [JsonPropertyName("generoId")]
    public Guid GeneroId { get; set; }

    [JsonPropertyName("autorId")]
    public Guid AutorId { get; set; }

    [JsonPropertyName("dataCadastro")]
    public DateTime DataCadastro { get; set; }

    [JsonPropertyName("autor")]
    public required AutorDto Autor { get; set; }

    [JsonPropertyName("genero")]
    public required GeneroDto Genero { get; set; }
}
