using MinhaBiblioteca.Api.DTOs;
using MinhaBiblioteca.Core.Responses;
using System.Net;
using System.Text.Json;

namespace MinhaBiblioteca.Api.Common.Extensions;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        //exception.Ship(context);
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = "application/json";

        string innerException = ex.InnerException != null ? ex.InnerException.Message : string.Empty;
        string stackTrace = ex.StackTrace ?? "";

        var errorDto = new ErrorDto
        {
            ErrorMessage = ex.Message,
            InnerException = innerException,
            StackTrace = stackTrace
        };

        var rpse = new Response<ErrorDto>(
            errorDto,
            (int)HttpStatusCode.InternalServerError,
            "Ops :( ... Ocorreu um erro inexperado. Tente novamente mais tarde."
        );

        await context.Response.WriteAsync(JsonSerializer.Serialize(rpse));
    }
}
