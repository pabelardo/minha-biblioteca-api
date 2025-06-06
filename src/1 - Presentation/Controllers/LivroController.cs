using Microsoft.AspNetCore.Mvc;
using MinhaBiblioteca.Application.DTOs;
using MinhaBiblioteca.Application.Interfaces.Response;
using MinhaBiblioteca.Application.Interfaces.Services;
using MinhaBiblioteca.Domain.Entities;
using MinhaBiblioteca.Domain.Interfaces.Notificacoes;

namespace MinhaBiblioteca.Api.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class LivroController : BaseController<Livro, LivroDto>
{
    public LivroController(
        INotificador notificador,
        INotificacao notificacao,
        IAppResponse appResponse,
        IServicoGenerico<Livro, LivroDto> genericoAppService) : base(
            notificador,
            notificacao,
            appResponse,
            genericoAppService) { }
}
