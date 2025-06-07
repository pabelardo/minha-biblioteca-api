namespace MinhaBiblioteca.Core.Requests.Genres;

public class CreateGenreRequest : Request
{
    public string Description { get; set; } = string.Empty;
}

