namespace MinhaBiblioteca.Core.Models;

public class Book : Entity
{
    public string Name { get; set; } = string.Empty;
    public Guid GenreId { get; set; }
    public Guid AuthorId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;


    /* EF Relation */

    public Author Author { get; set; } = null!;
    public Genre Genre { get; set; } = null!;
}
