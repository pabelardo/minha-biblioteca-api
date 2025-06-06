namespace MinhaBiblioteca.Core.Entities;

public class Livro : Entity
{
    public string Nome { get; set; }
    public Guid GeneroId { get; set; }
    public Guid AutorId { get; set; }
    public DateTime DataCadastro { get; set; }


    /* EF Relation */

    public Autor Autor { get; set; }
    public Genero Genero { get; set; }

    public Livro() { }

    public Livro(string nome, Guid generoId, Guid autorId, DateTime dataCadastro)
    {
        Nome = nome;
        GeneroId = generoId;
        AutorId = autorId;
        DataCadastro = dataCadastro;
    }
}
