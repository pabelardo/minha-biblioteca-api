using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MinhaBiblioteca.Core.Models;

namespace MinhaBiblioteca.Api.Data.Mappings;

public class GenreMapping : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.ToTable("Genres");

        builder.HasKey(g => g.Id);

        builder.Property(g => g.Name)
            .IsRequired(true)
            .HasColumnType("nvarchar(100)");

        builder.Property(g => g.Description)
            .IsRequired(false)
            .HasColumnType("nvarchar(255)");

        builder.Property(l => l.CreatedAt)
            .IsRequired(true);

        // Configuração de relacionamento com Livro
        // 1 x N - Genero x Livro
        builder.HasMany(g => g.Books)
            .WithOne(l => l.Genre)
            .HasForeignKey(l => l.GenreId);
    }
}
