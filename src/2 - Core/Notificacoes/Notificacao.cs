using MinhaBiblioteca.Core.Interfaces.Notificacoes;

namespace MinhaBiblioteca.Core.Notificacoes;

public class Notificacao : INotificacao
{
    public string Mensagem { get; }

    public Notificacao(string mensagem)
    {
        Mensagem = mensagem;
    }

    public Notificacao CriarNotificacao(string message) => new(message);
}