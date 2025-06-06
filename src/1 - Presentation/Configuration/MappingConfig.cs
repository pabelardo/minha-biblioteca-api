using Mapster;
using MinhaBiblioteca.Core.DTOs;
using MinhaBiblioteca.Core.Entities;

namespace MinhaBiblioteca.Api.Configuration;

public static class MappingConfig
{
    public static void RegistrarMapeamentos()
    {
        TypeAdapterConfig<Livro, LivroDto>.NewConfig()
            .Map(dest => dest.Autor, src => src.Autor)
            .Map(dest => dest.Genero, src => src.Genero);

        TypeAdapterConfig<Autor, AutorDto>.NewConfig()
            .Map(dest => dest.Livros, src => src.Livros);

        TypeAdapterConfig<Genero, GeneroDto>.NewConfig()
            .Map(dest => dest.Livros, src => src.Livros);
    }
}
