using MinhaBiblioteca.Core.Utils;

namespace MinhaBiblioteca.Api.Configuration;

public static class CorsConfig
{
    public static WebApplicationBuilder AddCorsConfig(this WebApplicationBuilder builder)
    {
        // Configuração de CORS
        builder.Services.AddCors(options =>
        {
            if(ConfigurationHelper.EhProd())
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
        return builder;
    }
}
