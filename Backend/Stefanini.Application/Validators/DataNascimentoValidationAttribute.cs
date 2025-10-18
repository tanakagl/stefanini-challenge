using System.ComponentModel.DataAnnotations;

namespace Stefanini.Application.Validators;

public class DataNascimentoValidationAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not DateTime dataNascimento)
        {
            return new ValidationResult("Data de nascimento deve ser uma data válida.");
        }

        if (dataNascimento.Date > DateTime.Now.Date)
        {
            return new ValidationResult("A data de nascimento não pode ser futura.");
        }

        if (dataNascimento.Year < 1900)
        {
            return new ValidationResult("Data de nascimento inválida.");
        }

        return ValidationResult.Success;
    }
}

