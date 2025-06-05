using MinhaBiblioteca.Domain.Notificacoes;

namespace MinhaBiblioteca.Domain.Interfaces.Notificacoes;

public interface INotificador
{
    bool TemNotificacao();
    List<Notificacao> ObterNotificacoes();
    void Handle(Notificacao notificacao);
}
