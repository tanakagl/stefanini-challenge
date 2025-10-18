using System.ComponentModel.DataAnnotations;

namespace Stefanini.Application.DTOs;

public class AddressDto
{
    [Required(ErrorMessage = "A rua é obrigatória.")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "A rua deve ter entre 3 e 200 caracteres.")]
    public string Rua { get; set; } = string.Empty;

    [Required(ErrorMessage = "O número é obrigatório.")]
    [StringLength(20, ErrorMessage = "O número deve ter no máximo 20 caracteres.")]
    public string Numero { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "O complemento deve ter no máximo 100 caracteres.")]
    public string? Complemento { get; set; }

    [Required(ErrorMessage = "O bairro é obrigatório.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "O bairro deve ter entre 2 e 100 caracteres.")]
    public string Bairro { get; set; } = string.Empty;

    [Required(ErrorMessage = "A cidade é obrigatória.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "A cidade deve ter entre 2 e 100 caracteres.")]
    public string Cidade { get; set; } = string.Empty;

    [Required(ErrorMessage = "O estado é obrigatório.")]
    [StringLength(2, MinimumLength = 2, ErrorMessage = "O estado deve ter 2 caracteres (UF).")]
    public string Estado { get; set; } = string.Empty;

    [Required(ErrorMessage = "O CEP é obrigatório.")]
    [RegularExpression(@"^\d{5}-?\d{3}$", ErrorMessage = "CEP inválido. Use o formato 12345-678 ou 12345678.")]
    public string Cep { get; set; } = string.Empty;
}

