using MinhaBiblioteca.Api.Common.Api;
using MinhaBiblioteca.Api.Common.Extensions;
using MinhaBiblioteca.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddConfiguration()
    .AddLogging()
    .AddDataContexts()
    .AddCrossOrigin()
    .AddDocumentation()
    .AddServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.ConfigureDevEnvironment();

app.UseMiddleware<ExceptionMiddleware>();
app.UseCors(ApiConfiguration.CorsPolicyName);
app.MapEndpoints();

app.Run();