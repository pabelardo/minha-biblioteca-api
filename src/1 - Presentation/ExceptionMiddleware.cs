using MinhaBiblioteca.Core.Enums;
using MinhaBiblioteca.Core.Interfaces.Response;

namespace MinhaBiblioteca.Api;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context, IAppResponse appResponse)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, appResponse);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception, IAppResponse appResponse)
    {
        context.Response.StatusCode = (int)StatusCodeEnum.InternalServerError;
        context.Response.ContentType = "application/json";

        string innerExceptionMessage = exception.InnerException?.Message ?? string.Empty;
        string stackTrace = exception.StackTrace ?? string.Empty;

        var resposta = await appResponse.CriarResponseAsync(
            sucesso: false,
            codigoDeStatus: (int)StatusCodeEnum.InternalServerError,
            mensagem: "Ops :( ... Ocorreu um erro inexperado.",
            mensagensDeErro: [exception.Message, innerExceptionMessage],
            data: null,
            erroInterno: exception.Message,
            rastreamentoDoErro: stackTrace
        );

        await context.Response.WriteAsJsonAsync(resposta);
    }
}
