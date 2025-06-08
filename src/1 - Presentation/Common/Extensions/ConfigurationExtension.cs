namespace MinhaBiblioteca.Api.Common.Extensions;

public static class ConfigurationExtension
{
    private static IServiceProvider? _serviceProvider;

    public static void InitServiceProvider(this IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

    public static object? GetService(this Type serviceType) => _serviceProvider?.GetService(serviceType);
}
