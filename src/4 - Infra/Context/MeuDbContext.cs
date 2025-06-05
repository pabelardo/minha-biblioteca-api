using Microsoft.EntityFrameworkCore;
using MinhaBiblioteca.Domain.Entities;

namespace MinhaBiblioteca.Data.Context;

public class MeuDbContext(DbContextOptions<MeuDbContext> options) : DbContext(options)
{
    public DbSet<Autor> Autores { get; set; }
    public DbSet<Livro> Livros { get; set; }
    public DbSet<Genero> Generos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Caso esqueça de definir o tipo de coluna para strings, o EF Core assume o varchar(100). 
        foreach (var property in modelBuilder.Model.GetEntityTypes()
            .SelectMany(e => e.GetProperties()
                .Where(p => p.ClrType == typeof(string))))
        {
            property.SetColumnType("varchar(100)");
        }

        // Aplica as configurações de entidades do assembly atual.
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MeuDbContext).Assembly);

        // Configura o comportamento de exclusão em cascata para relacionamentos.
        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Cascade;
        }

        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("CreatedAt") != null))
        {
            if (entry.State == EntityState.Added)
                entry.Property("CreatedAt").CurrentValue = DateTime.Now;

            if (entry.State == EntityState.Modified)
                entry.Property("CreatedAt").IsModified = false;
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
