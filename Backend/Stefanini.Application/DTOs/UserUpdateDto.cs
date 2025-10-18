using Stefanini.Application.Validators;
using Stefanini.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Stefanini.Application.DTOs;

public class UserUpdateDto
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

    public string Nacionalidade { get; set; } = string.Empty;

    public string Naturalidade { get; set; } = string.Empty;
    [Required(ErrorMessage = "O CPF é obrigatório.")]
    [CpfValidation]
    public string Cpf { get; set; } = string.Empty;
}

