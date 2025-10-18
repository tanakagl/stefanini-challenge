using FluentAssertions;
using Stefanini.Application.Validators;
using System.ComponentModel.DataAnnotations;

namespace Stefanini.Tests.Unit.Validators;

public class DataNascimentoValidationAttributeTests
{
    private readonly DataNascimentoValidationAttribute _validator = new();

    [Fact]
    public void IsValid_DeveRetornarTrue_ParaDataNascimentoValida()
    {
        var dataNascimento = DateTime.Now.AddYears(-25);
        var validationContext = new ValidationContext(new object());
        var result = _validator.GetValidationResult(dataNascimento, validationContext);

        result.Should().Be(ValidationResult.Success);
    }

    [Fact]
    public void IsValid_DeveRetornarFalse_ParaDataNascimentoFutura()
    {
        var dataNascimento = DateTime.Now.AddDays(1);
        var validationContext = new ValidationContext(new object());
        var result = _validator.GetValidationResult(dataNascimento, validationContext);

        result.Should().NotBe(ValidationResult.Success);
    }

    [Fact]
    public void IsValid_DeveRetornarFalse_ParaDataNascimentoAntesDe1900()
    {
        var dataNascimento = new DateTime(1899, 12, 31);
        var validationContext = new ValidationContext(new object());
        var result = _validator.GetValidationResult(dataNascimento, validationContext);

        result.Should().NotBe(ValidationResult.Success);
    }

    [Fact]
    public void ErrorMessage_DeveRetornarMensagemPadrao()
    {
        var errorMessage = _validator.ErrorMessage;

        errorMessage.Should().NotBeNullOrEmpty();
    }
}

