namespace MinhaBiblioteca.Core.Requests.Books;

public class CreateBookRequest : Request
{
    public Guid GenreId { get; set; }
    public Guid AuthorId { get; set; }
}

