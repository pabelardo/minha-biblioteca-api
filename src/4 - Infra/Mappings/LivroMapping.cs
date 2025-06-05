using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MinhaBiblioteca.Domain.Entities;

namespace MinhaBiblioteca.Data.Mappings;

public class LivroMapping : IEntityTypeConfiguration<Livro>
{
    public void Configure(EntityTypeBuilder<Livro> builder)
    {
        builder.ToTable("Livros");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.Nome)
            .IsRequired()
            .HasColumnType("varchar(100)");

        builder.Property(l => l.DataCadastro)
            .IsRequired()
            .HasColumnType("datetime");

        builder.Property(l => l.AutorId).IsRequired();

        builder.Property(l => l.GeneroId).IsRequired();

        // Configuração de relacionamento com Autor
        // N x 1 - Livro x Autor
        builder.HasOne(l => l.Autor)
            .WithMany(a => a.Livros)
            .HasForeignKey(l => l.AutorId);

        // Configuração de relacionamento com Genero
        // N x 1 - Livro x Genero
        builder.HasOne(l => l.Genero)
            .WithMany(g => g.Livros)
            .HasForeignKey(l => l.GeneroId);
    }
}
