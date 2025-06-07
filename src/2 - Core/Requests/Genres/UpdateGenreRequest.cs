namespace MinhaBiblioteca.Core.Requests.Genres;

public class UpdateGenreRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}
