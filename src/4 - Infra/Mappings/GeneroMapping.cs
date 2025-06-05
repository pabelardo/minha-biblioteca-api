using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MinhaBiblioteca.Domain.Entities;

namespace MinhaBiblioteca.Data.Mappings;

public class GeneroMapping : IEntityTypeConfiguration<Genero>
{
    public void Configure(EntityTypeBuilder<Genero> builder)
    {
        builder.ToTable("Generos");

        builder.HasKey(g => g.Id);

        builder.Property(g => g.Nome)
            .IsRequired()
            .HasColumnType("varchar(100)");

        builder.Property(g => g.Descricao)
            .IsRequired()
            .HasColumnType("varchar(255)");

        // Configuração de relacionamento com Livro
        // 1 x N - Genero x Livro
        builder.HasMany(g => g.Livros)
            .WithOne(l => l.Genero)
            .HasForeignKey(l => l.GeneroId);
    }
}
