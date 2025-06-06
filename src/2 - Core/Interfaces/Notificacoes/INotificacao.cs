using MinhaBiblioteca.Core.Notificacoes;

namespace MinhaBiblioteca.Core.Interfaces.Notificacoes;

public interface INotificacao
{
    Notificacao CriarNotificacao(string message);
}
