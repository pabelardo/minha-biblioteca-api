using Serilog;

namespace MinhaBiblioteca.Api.Configuration;

public static class LoggingConfig
{
    public static WebApplicationBuilder AddLoggingConfig(this WebApplicationBuilder builder)
    {
        //Configuração de Log para o Serilog
        builder.Host.UseSerilog((context, services, config) =>
        {
            config.ReadFrom.Configuration(context.Configuration)
                  .ReadFrom.Services(services);
        });
        return builder;
    }
}
