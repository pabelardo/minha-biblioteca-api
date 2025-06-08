namespace MinhaBiblioteca.Api.DTOs;

public class BookDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public Guid AuthorId { get; set; }
    public Guid GenreId { get; set; }

    public AuthorDto? Author { get; set; }
    public GenreDto? Genre { get; set; }
}
