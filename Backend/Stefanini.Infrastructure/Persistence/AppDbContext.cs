using Stefanini.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Stefanini.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // DbSets
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configurações de entidades virão aqui
    }

    public override int SaveChanges()
    {
        AtualizarTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        AtualizarTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void AtualizarTimestamps()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in entries)
        {
            var entity = (BaseEntity)entry.Entity;

            if (entry.State == EntityState.Added)
            {
                entity.DataCriacao = DateTime.UtcNow;
                entity.DataUltimaAtualizacao = DateTime.UtcNow;
            }
            else if (entry.State == EntityState.Modified)
            {
                entity.DataUltimaAtualizacao = DateTime.UtcNow;
            }
        }
    }
}

