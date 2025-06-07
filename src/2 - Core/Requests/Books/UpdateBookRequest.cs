namespace MinhaBiblioteca.Core.Requests.Books;

public class UpdateBookRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid GenreId { get; set; }
    public Guid AuthorId { get; set; }
}
