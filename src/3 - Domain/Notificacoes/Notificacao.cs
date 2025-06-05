using MinhaBiblioteca.Domain.Interfaces.Notificacoes;

namespace MinhaBiblioteca.Domain.Notificacoes;

public class Notificacao : INotificacao
{
    public string Mensagem { get; }

    public Notificacao(string mensagem)
    {
        Mensagem = mensagem;
    }

    public Notificacao CriarNotificacao(string message) => new(message);
}