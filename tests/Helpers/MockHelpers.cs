using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MinhaBiblioteca.Api.Validations.FluentValidation.Books;

namespace MinhaBiblioteca.Tests.Helpers;

public static class MockHelpers
{
    public static IServiceProvider GetServiceProvider()
    {
        var services = new ServiceCollection();

        services.AddValidatorsFromAssemblyContaining<CreateBookValidation>();

        var provider = services.BuildServiceProvider();

        return provider;
    }
}
