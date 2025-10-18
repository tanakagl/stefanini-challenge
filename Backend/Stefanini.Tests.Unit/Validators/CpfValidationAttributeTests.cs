using FluentAssertions;
using Stefanini.Application.Validators;
using System.ComponentModel.DataAnnotations;

namespace Stefanini.Tests.Unit.Validators;

public class CpfValidationAttributeTests
{
    private readonly CpfValidationAttribute _validator = new();

    [Theory]
    [InlineData("12345678909")]
    [InlineData("11144477735")]
    [InlineData("00000000191")]
    public void IsValid_DeveRetornarTrue_ParaCpfsValidos(string cpf)
    {
        var validationContext = new ValidationContext(new object());
        var result = _validator.GetValidationResult(cpf, validationContext);

        result.Should().Be(ValidationResult.Success);
    }

    [Theory]
    [InlineData("00000000000")]
    [InlineData("11111111111")]
    [InlineData("12345678900")]
    [InlineData("123456789")]
    [InlineData("123456789012")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("abcdefghijk")]
    public void IsValid_DeveRetornarFalse_ParaCpfsInvalidos(string? cpf)
    {
        var validationContext = new ValidationContext(new object());
        var result = _validator.GetValidationResult(cpf, validationContext);

        result.Should().NotBe(ValidationResult.Success);
    }

    [Fact]
    public void ErrorMessage_DeveRetornarMensagemPadrao()
    {
        var errorMessage = _validator.ErrorMessage;

        errorMessage.Should().NotBeNullOrEmpty();
        errorMessage.Should().Contain("CPF");
    }
}

