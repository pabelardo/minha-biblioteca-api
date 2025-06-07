using Microsoft.EntityFrameworkCore;
using MinhaBiblioteca.Api.Common.Mappings;
using MinhaBiblioteca.Api.Data.Context;
using MinhaBiblioteca.Api.DTOs;
using MinhaBiblioteca.Core.Requests.Authors;
using MinhaBiblioteca.Core.Responses;

var builder = WebApplication.CreateBuilder(args);

var cnnStr = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(cnnStr));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.CustomSchemaIds(type => type.FullName);
});

builder.Services.AddTransient<Handler>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost(
    "/v1/authors", 
    (CreateAuthorRequest request, Handler handler)
    => handler.Handle(request))
    .WithName("Authors: Create")
    .WithSummary("Cria um novo Autor")
    .Produces<Response<AuthorDto>>();

app.Run();

public class Handler(AppDbContext context)
{
    public Response<AuthorDto> Handle(CreateAuthorRequest request)
    {
        //ToDo: Validar request

        var result = context.Author.Add(request.ToEntity());

        context.SaveChanges();

        return new Response<AuthorDto>
        {
            Data = result.Entity.ToDto(),
            Message = "Autor criado com sucesso!"
        };
    }
}