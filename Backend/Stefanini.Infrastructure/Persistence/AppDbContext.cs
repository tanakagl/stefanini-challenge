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
        
        // Configuração da entidade User
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            // Address como objeto de valor (owned type)
            entity.OwnsOne(e => e.Endereco, address =>
            {
                address.Property(a => a.Rua).HasMaxLength(200);
                address.Property(a => a.Numero).HasMaxLength(20);
                address.Property(a => a.Complemento).HasMaxLength(100);
                address.Property(a => a.Bairro).HasMaxLength(100);
                address.Property(a => a.Cidade).HasMaxLength(100);
                address.Property(a => a.Estado).HasMaxLength(2);
                address.Property(a => a.Cep).HasMaxLength(8);
            });
            
            entity.Property(e => e.NomeCompleto).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Email).HasMaxLength(254);
            entity.Property(e => e.Cpf).IsRequired().HasMaxLength(11);
            entity.Property(e => e.Nacionalidade).HasMaxLength(100);
            entity.Property(e => e.Naturalidade).HasMaxLength(100);
            
            // Índices
            entity.HasIndex(e => e.Email);
            entity.HasIndex(e => e.Cpf).IsUnique();
        });
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

