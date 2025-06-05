using MinhaBiblioteca.Domain.Entities;
using MinhaBiblioteca.Domain.Interfaces.Notificacoes;
using MinhaBiblioteca.Domain.Interfaces.Repositories;
using MinhaBiblioteca.Domain.Interfaces.Services;
using MinhaBiblioteca.Domain.Services.Base;

namespace MinhaBiblioteca.Domain.Services;

public class AutorService : BaseService<Autor>, IAutorService
{
    public AutorService(
        IRepository<Autor> repository,
        INotificador notificador,
        INotificacao notificacao) : base(repository, notificador, notificacao) { }
}