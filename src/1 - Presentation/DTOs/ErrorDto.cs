namespace MinhaBiblioteca.Api.DTOs;

public class ErrorDto
{
    public string? ErrorMessage { get; set; }
    public string? InnerException { get; set; }
    public string? StackTrace { get; set; }
}
