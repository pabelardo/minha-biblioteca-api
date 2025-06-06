using Microsoft.EntityFrameworkCore;
using MinhaBiblioteca.Core.Utils;
using MinhaBiblioteca.Data.Context;

namespace MinhaBiblioteca.Api.Configuration;

public static class DbContextConfig
{
    public static WebApplicationBuilder AddDbContextConfig(this WebApplicationBuilder builder)
    {
        // Aqui você pode configurar o DbContext, por exemplo, usando Entity Framework Core
        // builder.Services.AddDbContext<SeuDbContext>(options =>
        //     options.UseSqlServer(ConfigurationHelper.ObterConnectionString()));
        // Exemplo de configuração de um DbContext fictício
        // builder.Services.AddDbContext<MeuDbContext>(options =>
        //     options.UseInMemoryDatabase("MinhaBiblioteca"));
        builder.Services.AddDbContext<MeuDbContext>(options =>
        {
            options.UseSqlServer(ConfigurationHelper.ObterConnectionString())
            .EnableSensitiveDataLogging()
            .LogTo(Console.WriteLine, LogLevel.Information);
        });

        return builder;
    }
}
