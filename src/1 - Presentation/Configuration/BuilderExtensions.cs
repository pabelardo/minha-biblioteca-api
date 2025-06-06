using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MinhaBiblioteca.Core.Interfaces.Notificacoes;
using MinhaBiblioteca.Core.Interfaces.Repositories;
using MinhaBiblioteca.Core.Interfaces.Response;
using MinhaBiblioteca.Core.Interfaces.Services;
using MinhaBiblioteca.Core.Notificacoes;
using MinhaBiblioteca.Core.ResponsePadronizado;
using MinhaBiblioteca.Core.Services;
using MinhaBiblioteca.Core.Utils;
using MinhaBiblioteca.Core.Validations.FluentValidation;
using MinhaBiblioteca.Data.Context;
using MinhaBiblioteca.Data.Repositories;
using Serilog;

namespace MinhaBiblioteca.Api.Configuration;

public static class BuilderExtensions
{
    public static void AddEnvironmentConfig(this WebApplicationBuilder builder)
    {
        //EnvinronmentName é carregada através do launchSettings.json ou da variável de ambiente ASPNETCORE_ENVIRONMENT
        var configuration = builder.Configuration
            .SetBasePath(builder.Environment.ContentRootPath)
            .AddJsonFile("appsettings.json", true, true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
            .AddEnvironmentVariables();

        var config = configuration.Build();

        ConfigurationHelper.Iniciar(config);

        ConfigurationHelper.SetEnvironment(config);
    }

    public static void AddDataContexts(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<MeuDbContext>(options =>
        {
            options.UseSqlServer(ConfigurationHelper.ObterConnectionString())
            .EnableSensitiveDataLogging()
            .LogTo(Console.WriteLine, LogLevel.Information);
        });
    }

    public static void AddServices(this WebApplicationBuilder builder)
    {
        #region Repositórios

        builder.Services.AddScoped<MeuDbContext>();
        builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        #endregion

        #region Serviços

        builder.Services.AddScoped(typeof(IGenericService<,>), typeof(GenericService<,>));
        builder.Services.AddScoped<IAppResponse, AppResponse>();
        builder.Services.AddScoped<INotificacao, Notificacao>();
        builder.Services.AddScoped<INotificador, Notificador>();

        #endregion
    }

    public static void AddCrossOrigin(this WebApplicationBuilder builder)
    {
        // Configuração de CORS
        builder.Services.AddCors(options =>
        {
            if (ConfigurationHelper.EhProd())
            {
                // Em produção, restringe as origens permitidas
                options.AddPolicy("AllowSpecificOrigins",
                    builder => builder.WithOrigins("https://example.com", "https://another-example.com")
                                      .AllowAnyMethod()
                                      .AllowAnyHeader());
            }
            else
            {
                // Em desenvolvimento, permite todas as origens
                options.AddPolicy("AllowAllOrigins",
                    builder => builder.AllowAnyOrigin()
                                      .AllowAnyMethod()
                                      .AllowAnyHeader());
            }
        });
    }

    public static void AddLogging(this WebApplicationBuilder builder)
    {
        //Configuração de Log para o Serilog
        builder.Host.UseSerilog((context, services, config) =>
        {
            config.ReadFrom.Configuration(context.Configuration)
                  .ReadFrom.Services(services);
        });
    }

    public static void AddFluentValidationConfig(this WebApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssemblyContaining<LivroValidation>();
    }
}
