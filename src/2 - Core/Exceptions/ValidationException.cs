namespace MinhaBiblioteca.Domain.Exceptions;

public class ValidationException : Exception
{
    public ValidationException() { }

    public ValidationException(string mensagem) : base(mensagem) { }

    public ValidationException(string mensagem, Exception excessaoInterna) : base(mensagem, excessaoInterna) { }
}
