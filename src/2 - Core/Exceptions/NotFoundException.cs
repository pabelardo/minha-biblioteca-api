namespace MinhaBiblioteca.Core.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException() { }
    public NotFoundException(string mensagem) : base(mensagem) { }
    public NotFoundException(string mensagem, Exception excessaoInterna) : base(mensagem, excessaoInterna) { }
}
