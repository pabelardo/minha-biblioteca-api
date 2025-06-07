namespace MinhaBiblioteca.Api.DTOs;

public class AuthorDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public ICollection<BookDto> Books { get; set; } = [];
}
