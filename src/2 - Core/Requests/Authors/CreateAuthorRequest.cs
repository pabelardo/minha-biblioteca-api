namespace MinhaBiblioteca.Core.Requests.Authors;

public class CreateAuthorRequest : Request
{
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
}

