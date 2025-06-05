using MinhaBiblioteca.Domain.Notificacoes;

namespace MinhaBiblioteca.Domain.Interfaces.Notificacoes;

public interface INotificacao
{
    Notificacao CriarNotificacao(string message);
}
