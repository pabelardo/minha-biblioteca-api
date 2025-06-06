using MinhaBiblioteca.Core.Interfaces.Notificacoes;
using MinhaBiblioteca.Core.Interfaces.Repositories;
using MinhaBiblioteca.Core.Interfaces.Response;
using MinhaBiblioteca.Core.Interfaces.Services;
using MinhaBiblioteca.Core.Notificacoes;
using MinhaBiblioteca.Core.ResponsePadronizado;
using MinhaBiblioteca.Core.Services;
using MinhaBiblioteca.Data.Context;
using MinhaBiblioteca.Data.Repositories;

namespace MinhaBiblioteca.Api.Configuration;

public static class DependencyInjectionConfig
{
    public static WebApplicationBuilder AddDependencyInjectionConfig(this WebApplicationBuilder builder)
    {
        // Aqui você pode registrar seus serviços, repositórios, etc.
        // Exemplo de registro de um serviço fictício
        // builder.Services.AddScoped<IMeuServico, MeuServico>();
        // Exemplo de registro de um repositório fictício
        // builder.Services.AddScoped<IMeuRepositorio, MeuRepositorio>();

        // Registrar o DbContext configurado
        #region Repositorios

        builder.Services.AddScoped<MeuDbContext>();
        builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        #endregion

        #region Serviços

        builder.Services.AddScoped(typeof(IGenericService<,>), typeof(GenericService<,>));
        builder.Services.AddScoped<IAppResponse, AppResponse>();
        builder.Services.AddScoped<INotificacao, Notificacao>();
        builder.Services.AddScoped<INotificador, Notificador>();

        #endregion

        return builder;
    }
}
