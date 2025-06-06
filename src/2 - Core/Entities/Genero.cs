namespace MinhaBiblioteca.Core.Entities;

public class Genero : Entity
{
    public required string Nome { get; set; }
    public required string Descricao { get; set; }

    /* EF Relation */
    public ICollection<Livro> Livros { get; set; } = [];

    public Genero() { }

    public Genero(string nome, string descricao)
    {
        Nome = nome;
        Descricao = descricao;
    }
}
