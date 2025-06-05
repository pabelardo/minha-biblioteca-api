using MinhaBiblioteca.Domain.Entities;
using MinhaBiblioteca.Domain.Interfaces.Notificacoes;
using MinhaBiblioteca.Domain.Interfaces.Repositories;
using MinhaBiblioteca.Domain.Interfaces.Services;
using MinhaBiblioteca.Domain.Services.Base;

namespace MinhaBiblioteca.Domain.Services;

public class GeneroService : BaseService<Genero>, IGeneroService
{
    public GeneroService(
        IRepository<Genero> repository,
        INotificador notificador,
        INotificacao notificacao) : base(repository, notificador, notificacao) { }
}
