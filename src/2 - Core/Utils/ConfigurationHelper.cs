using Microsoft.Extensions.Configuration;

namespace MinhaBiblioteca.Core.Utils;

public static class ConfigurationHelper
{
    private static IConfiguration _config;
    private static IServiceProvider _serviceProvider;
    private static bool _dev;
    private static bool _prod;

    public static void Iniciar(IConfiguration config) => _config = config;

    public static void IniciarServiceProvider(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    public static void SetEnvironment(IConfiguration config)
    {
        _dev = config["Environment"] == "Development";
        _prod = config["Environment"] == "Production";
    }

    public static void SetEnvironment(bool dev, bool prod, bool localHost)
    {
        _dev = dev;
        _prod = prod;
    }

    public static T? ObterValorDaSection<T>(string sectionName, string key, T defaultValue) => _config.GetSection(sectionName).GetValue(key, defaultValue);

    public static object? ObterServico(Type serviceType) => _serviceProvider?.GetService(serviceType);

    public static string ObterConnectionString()
    {
        //Caso queira obter uma connection string no appsettings.json, descomente a linha abaixo e ajuste conforme necessário.
        //var connectionString = ObterValorDaSection("ConnectionStrings", "DefaultConnection", string.Empty);

        // Caso queira obter uma connection string do ambiente, utilize a variável de ambiente CONNECTION_STRING.
        var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

        return string.IsNullOrEmpty(connectionString)
            ? throw new Exception("Connection String não encontrada na configuração.")
            : connectionString;
    }

    public static bool EhDev() => _dev;

    public static bool EhProd() => _prod;

    public static string ObterStringEnvironment()
    {
        if(EhDev()) return "Development";

        if(EhProd()) return "Production";

        return "Development";
    }
}
