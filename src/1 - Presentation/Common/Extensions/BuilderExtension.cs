using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MinhaBiblioteca.Api.Common.Api;
using MinhaBiblioteca.Api.Data.Context;
using MinhaBiblioteca.Api.Data.Repositories;
using MinhaBiblioteca.Api.Handlers;
using MinhaBiblioteca.Api.Handlers.Interfaces;
using MinhaBiblioteca.Api.Validations.FluentValidation.Books;
using MinhaBiblioteca.Core.Config;
using MinhaBiblioteca.Core.Repositories;
using Serilog;

namespace MinhaBiblioteca.Api.Common.Extensions;

public static class BuilderExtension
{
    public static WebApplicationBuilder AddConfiguration(
        this WebApplicationBuilder builder)
    {
        //var configuration = builder.Configuration
        //    .SetBasePath(builder.Environment.ContentRootPath)
        //    .AddJsonFile("appsettings.json", true, true)
        //    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
        //    .AddEnvironmentVariables();

        //var config = configuration.Build();

        Configuration.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
        Configuration.BackendUrl = builder.Configuration.GetValue<string>("BackendUrl") ?? string.Empty;
        Configuration.FrontendUrl = builder.Configuration.GetValue<string>("FrontendUrl") ?? string.Empty;

        return builder;
    }

    public static WebApplicationBuilder AddDocumentation(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(x => { x.CustomSchemaIds(n => n.FullName); });

        return builder;
    }

    public static WebApplicationBuilder AddDataContexts(this WebApplicationBuilder builder)
    {
        builder
            .Services
            .AddDbContext<AppDbContext>(
                x => { x.UseSqlServer(Configuration.ConnectionString); });

        return builder;
    }

    public static WebApplicationBuilder AddCrossOrigin(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors(
            options => options.AddPolicy(
                ApiConfiguration.CorsPolicyName,
                policy => policy
                    .WithOrigins([
                        Configuration.BackendUrl,
                        Configuration.FrontendUrl
                    ])
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
            ));

        return builder;
    }

    public static WebApplicationBuilder AddLogging(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((ctx, config) =>
        {
            config.ReadFrom.Configuration(ctx.Configuration);
        });

        return builder;
    }

    public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
    {
        builder
            .Services
            .AddTransient<IBookHandler, BookHandler>();

        builder
            .Services
            .AddTransient<IAuthorHandler, AuthorHandler>();

        builder
            .Services
            .AddTransient<IGenreHandler, GenreHandler>();

        builder
            .Services
            .AddTransient<IBookRepository, BookRepository>();

        builder
            .Services
            .AddTransient<IAuthorRepository, AuthorRepository>();

        builder
            .Services
            .AddTransient<IGenreRepository, GenreRepository>();

        builder
            .Services
            .AddValidatorsFromAssemblyContaining<CreateBookValidation>();

        builder
            .Services
            .BuildServiceProvider()
            .InitServiceProvider();

        return builder;
    }
}
