using Stefanini.Domain.Enums;

namespace Stefanini.Application.DTOs.V2;

public class UserResponseDtoV2
{
    public Guid Id { get; set; }
    public string NomeCompleto { get; set; } = string.Empty;
    public SexoUsuario Sexo { get; set; }
    public string Email { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }
    public string Nacionalidade { get; set; } = string.Empty;
    public string Naturalidade { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public AddressDto? Endereco { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime DataUltimaAtualizacao { get; set; }
}

