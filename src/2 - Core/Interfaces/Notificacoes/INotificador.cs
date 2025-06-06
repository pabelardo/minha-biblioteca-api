using MinhaBiblioteca.Core.Notificacoes;

namespace MinhaBiblioteca.Core.Interfaces.Notificacoes;

public interface INotificador
{
    bool TemNotificacao();
    List<Notificacao> ObterNotificacoes();
    void Handle(Notificacao notificacao);
}
