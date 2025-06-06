namespace MinhaBiblioteca.Core.Entities;

public class Autor : Entity
{
    public required string Nome { get; set; }
    public required string Email { get; set; }
    public required string Telefone { get; set; }

    /* EF Relation */
    public ICollection<Livro> Livros { get; set; } = [];

    public Autor() { }

    public Autor(string nome, string email, string telefone)
    {
        Nome = nome;
        Email = email;
        Telefone = telefone;
    }
}
