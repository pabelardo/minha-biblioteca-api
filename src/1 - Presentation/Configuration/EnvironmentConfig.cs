using MinhaBiblioteca.Core.Utils;

namespace MinhaBiblioteca.Api.Configuration;

public static class EnvironmentConfig
{
    public static WebApplicationBuilder AddEnvironmentConfig(this WebApplicationBuilder builder)
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

        return builder;
    }
}
