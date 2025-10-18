namespace Stefanini.Domain.Entities;

public abstract class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime DataUltimaAtualizacao { get; set; }

    protected BaseEntity()
    {
        Id = Guid.NewGuid();
        DataCriacao = DateTime.UtcNow;
        DataUltimaAtualizacao = DateTime.UtcNow;
    }

    public void AtualizarDataModificacao()
    {
        DataUltimaAtualizacao = DateTime.UtcNow;
    }
}

