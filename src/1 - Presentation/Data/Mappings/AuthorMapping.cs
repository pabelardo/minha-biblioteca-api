using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MinhaBiblioteca.Core.Models;

namespace MinhaBiblioteca.Api.Data.Mappings;

public class AuthorMapping : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.ToTable("Authors");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Name)
            .IsRequired(true)
            .HasColumnType("nvarchar(100)");

        builder.Property(a => a.Email)
            .IsRequired(false)
            .HasColumnType("varchar(100)");

        builder.Property(a => a.Phone)
            .IsRequired(false)
            .HasColumnType("varchar(25)");

        builder.Property(l => l.CreatedAt)
            .IsRequired();

        // Configuração de relacionamento com Livro
        // 1 x N - Autor x Livro

        builder.HasMany(a => a.Books)
            .WithOne(l => l.Author)
            .HasForeignKey(l => l.AuthorId);
    }
}
