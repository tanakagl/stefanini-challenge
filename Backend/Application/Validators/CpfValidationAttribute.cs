using System.ComponentModel.DataAnnotations;

namespace Backend.Application.Validators;

public class CpfValidationAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not string cpf)
        {
            return new ValidationResult("CPF deve ser uma string.");
        }

        if (string.IsNullOrWhiteSpace(cpf))
        {
            return ValidationResult.Success;
        }

        cpf = cpf.Replace(".", "").Replace("-", "").Trim();

        if (cpf.Length != 11)
        {
            return new ValidationResult("CPF deve ter 11 dígitos.");
        }

        if (!cpf.All(char.IsDigit))
        {
            return new ValidationResult("CPF deve conter apenas números.");
        }

        if (cpf.Distinct().Count() == 1)
        {
            return new ValidationResult("CPF inválido (todos os dígitos são iguais).");
        }

        if (!ValidarDigitosVerificadores(cpf))
        {
            return new ValidationResult("CPF inválido (dígitos verificadores incorretos).");
        }

        return ValidationResult.Success;
    }

    private static bool ValidarDigitosVerificadores(string cpf)
    {
        var multiplicador1 = new[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        var multiplicador2 = new[] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        var tempCpf = cpf[..9];
        var soma = 0;

        for (int i = 0; i < 9; i++)
        {
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
        }

        var resto = soma % 11;
        resto = resto < 2 ? 0 : 11 - resto;

        var digito = resto.ToString();
        tempCpf += digito;
        soma = 0;

        for (int i = 0; i < 10; i++)
        {
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
        }

        resto = soma % 11;
        resto = resto < 2 ? 0 : 11 - resto;
        digito += resto.ToString();

        return cpf.EndsWith(digito);
    }
}

