using MinhaBiblioteca.Domain.Entities;
using MinhaBiblioteca.Domain.Interfaces.Notificacoes;
using MinhaBiblioteca.Domain.Interfaces.Repositories;
using MinhaBiblioteca.Domain.Interfaces.Services;
using MinhaBiblioteca.Domain.Services.Base;

namespace MinhaBiblioteca.Domain.Services;

public class LivroService : BaseService<Livro>, ILivroService
{
    public LivroService(
        IRepository<Livro> repository,
        INotificador notificador,
        INotificacao notificacao) : base(repository, notificador, notificacao) { }
}
