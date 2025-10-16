namespace Backend.Domain.Entities;

public class User : BaseEntity
{
    public required string NomeCompleto { get; set; }
    public string Sexo { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public required DateTime DataNascimento { get; set; }
    public string Nacionalidade { get; set; } = string.Empty;
    public string Naturalidade { get; set; } = string.Empty;
    public required string Cpf { get; set; }
}
