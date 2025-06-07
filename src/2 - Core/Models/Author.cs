namespace MinhaBiblioteca.Core.Models;

public class Author : Entity
{
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    /* EF Relation */
    public ICollection<Book> Books { get; set; } = [];
}
