using MinhaBiblioteca.Domain.Entities;
using System.Text.Json.Serialization;

namespace MinhaBiblioteca.Application.DTOs;

public class AutorDTO
{
    public required string Nome { get; set; }
    public required string Email { get; set; }
    public required string Telefone { get; set; }

    [JsonIgnore]
    public ICollection<Livro> Livros { get; set; } = [];
}
