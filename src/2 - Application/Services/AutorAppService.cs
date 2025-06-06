using MinhaBiblioteca.Application.DTOs;
using MinhaBiblioteca.Application.Interfaces.Response;
using MinhaBiblioteca.Application.Interfaces.Services;
using MinhaBiblioteca.Application.Services.Base;
using MinhaBiblioteca.Domain.Entities;
using MinhaBiblioteca.Domain.Interfaces.Notificacoes;
using MinhaBiblioteca.Domain.Interfaces.Repositories;

namespace MinhaBiblioteca.Application.Services;

public class AutorAppService : BaseAppService<Livro, LivroDto>, IAutorAppService
{
    public AutorAppService(
        INotificador notificador,
        INotificacao notificacao,
        IAppResponse appResponse,
        IRepository<Livro> repository) : base(notificador, notificacao, appResponse, repository) { }
}
