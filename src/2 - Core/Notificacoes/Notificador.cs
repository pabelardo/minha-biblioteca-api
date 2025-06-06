using MinhaBiblioteca.Core.Interfaces.Notificacoes;

namespace MinhaBiblioteca.Core.Notificacoes;

public class Notificador : INotificador
{
    private List<Notificacao> _notificacoes;

    public Notificador() => _notificacoes = [];

    public void Handle(Notificacao notificacao) => _notificacoes.Add(notificacao);

    public List<Notificacao> ObterNotificacoes() => _notificacoes;

    public bool TemNotificacao() => _notificacoes.Any();
}
