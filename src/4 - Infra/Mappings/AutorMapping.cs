using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MinhaBiblioteca.Domain.Entities;

namespace MinhaBiblioteca.Data.Mappings;

public class AutorMapping : IEntityTypeConfiguration<Autor>
{
    public void Configure(EntityTypeBuilder<Autor> builder)
    {
        builder.ToTable("Autores");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Nome)
            .IsRequired()
            .HasColumnType("varchar(100)");

        builder.Property(a => a.Email)
            .IsRequired()
            .HasColumnType("varchar(100)");

        builder.Property(a => a.Telefone)
            .IsRequired()
            .HasColumnType("varchar(13)");

        // Configuração de relacionamento com Livro
        // 1 x N - Autor x Livro

        builder.HasMany(a => a.Livros)
            .WithOne(l => l.Autor)
            .HasForeignKey(l => l.AutorId);
    }
}
