namespace MinhaBiblioteca.Core.Models;

public class Genre : Entity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    /* EF Relation */
    public ICollection<Book> Books { get; set; } = [];
}
