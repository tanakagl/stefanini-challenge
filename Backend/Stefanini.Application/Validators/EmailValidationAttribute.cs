using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Stefanini.Application.Validators;

public class EmailValidationAttribute : ValidationAttribute
{
    public EmailValidationAttribute()
    {
        ErrorMessage = "Email inválido.";
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
        {
            return new ValidationResult("Email é obrigatório.");
        }

        if (value is not string email)
        {
            return new ValidationResult("Email deve ser uma string.");
        }

        email = email.Trim().ToLower();

        if (email.Length < 5 || email.Length > 254)
        {
            return new ValidationResult("Email deve ter entre 5 e 254 caracteres.");
        }

        var emailRegex = @"^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,}$";
        if (!Regex.IsMatch(email, emailRegex))
        {
            return new ValidationResult("Formato de email inválido.");
        }

        var parts = email.Split('@');
        if (parts.Length != 2)
        {
            return new ValidationResult("Email inválido.");
        }

        var localPart = parts[0];
        var domainPart = parts[1];

        if (localPart.Length > 64)
        {
            return new ValidationResult("Parte local do email muito longa (máximo 64 caracteres).");
        }

        if (localPart.StartsWith('.') || localPart.EndsWith('.') || localPart.Contains(".."))
        {
            return new ValidationResult("Email com formato inválido (uso incorreto de pontos).");
        }

        if (!domainPart.Contains('.'))
        {
            return new ValidationResult("Domínio do email inválido (deve conter um ponto).");
        }

        return ValidationResult.Success;
    }
}

