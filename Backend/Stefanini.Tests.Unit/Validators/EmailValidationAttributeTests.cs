using FluentAssertions;
using Stefanini.Application.Validators;
using System.ComponentModel.DataAnnotations;

namespace Stefanini.Tests.Unit.Validators;

public class EmailValidationAttributeTests
{
    private readonly EmailValidationAttribute _validator = new();

    [Theory]
    [InlineData("teste@example.com")]
    [InlineData("usuario.teste@empresa.com.br")]
    [InlineData("user123@test-domain.org")]
    [InlineData("a@b.co")]
    public void IsValid_DeveRetornarTrue_ParaEmailsValidos(string email)
    {
        var validationContext = new ValidationContext(new object());
        var result = _validator.GetValidationResult(email, validationContext);

        result.Should().Be(ValidationResult.Success);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("emailsemarroba.com")]
    [InlineData("@semdominio.com")]
    [InlineData("usuario@")]
    [InlineData("usuario@@example.com")]
    [InlineData("usuario @example.com")]
    [InlineData("usuario@.com")]
    public void IsValid_DeveRetornarFalse_ParaEmailsInvalidos(string? email)
    {
        var validationContext = new ValidationContext(new object());
        var result = _validator.GetValidationResult(email, validationContext);

        result.Should().NotBe(ValidationResult.Success);
    }

    [Fact]
    public void ErrorMessage_DeveRetornarMensagemPadrao()
    {
        var errorMessage = _validator.ErrorMessage;

        errorMessage.Should().NotBeNullOrEmpty();
        errorMessage.Should().Contain("Email");
    }
}

