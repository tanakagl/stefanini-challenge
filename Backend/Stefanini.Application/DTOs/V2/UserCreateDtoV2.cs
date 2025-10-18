using Stefanini.Application.Validators;
using Stefanini.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Stefanini.Application.DTOs.V2;

public class UserCreateDtoV2
{
    [Required(ErrorMessage = "O nome completo é obrigatório.")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 200 caracteres.")]
    public string NomeCompleto { get; set; } = string.Empty;

    public SexoUsuario Sexo { get; set; }

    [EmailValidation]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "A data de nascimento é obrigatória.")]
    [DataNascimentoValidation]
    public DateTime DataNascimento { get; set; }

    [Required(ErrorMessage = "A nacionalidade é obrigatória.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "A nacionalidade deve ter entre 2 e 100 caracteres.")]
    public string Nacionalidade { get; set; } = string.Empty;

    [Required(ErrorMessage = "A naturalidade é obrigatória.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "A naturalidade deve ter entre 2 e 100 caracteres.")]
    public string Naturalidade { get; set; } = string.Empty;

    [Required(ErrorMessage = "O CPF é obrigatório.")]
    [CpfValidation]
    public string Cpf { get; set; } = string.Empty;

    // Endereço obrigatório na v2
    [Required(ErrorMessage = "O endereço é obrigatório.")]
    public AddressDto Endereco { get; set; } = new();

    // Senha obrigatória para autenticação
    [Required(ErrorMessage = "A senha é obrigatória.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter entre 6 e 100 caracteres.")]
    public string Password { get; set; } = string.Empty;
}

