namespace MinhaBiblioteca.Domain.Exceptions;

public class ValidacaoException : Exception
{
    public ValidacaoException() { }

    public ValidacaoException(string mensagem) : base(mensagem) { }

    public ValidacaoException(string mensagem, Exception excessaoInterna) : base(mensagem, excessaoInterna) { }
}
