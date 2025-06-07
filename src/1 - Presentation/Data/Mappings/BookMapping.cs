using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MinhaBiblioteca.Core.Models;

namespace MinhaBiblioteca.Api.Data.Mappings;

public class BookMapping : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("Books");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.Name)
            .IsRequired(true)
            .HasColumnType("nvarchar(100)");

        builder.Property(l => l.CreatedAt)
            .IsRequired(true);

        builder.Property(l => l.AuthorId).IsRequired(true);

        builder.Property(l => l.GenreId).IsRequired(true);

        // Configuração de relacionamento com Autor
        // N x 1 - Livro x Autor
        builder.HasOne(l => l.Author)
            .WithMany(a => a.Books)
            .HasForeignKey(l => l.AuthorId);

        // Configuração de relacionamento com Genero
        // N x 1 - Livro x Genero
        builder.HasOne(l => l.Genre)
            .WithMany(g => g.Books)
            .HasForeignKey(l => l.GenreId);
    }
}
