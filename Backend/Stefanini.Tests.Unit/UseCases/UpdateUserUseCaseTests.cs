using FluentAssertions;
using Moq;
using Stefanini.Application.DTOs;
using Stefanini.Application.UseCases;
using Stefanini.Domain.Entities;
using Stefanini.Domain.Enums;
using Stefanini.Domain.Interfaces;

namespace Stefanini.Tests.Unit.UseCases;

public class UpdateUserUseCaseTests
{
    private readonly Mock<IUserRepository> _mockRepository;
    private readonly UpdateUserUseCase _useCase;

    public UpdateUserUseCaseTests()
    {
        _mockRepository = new Mock<IUserRepository>();
        _useCase = new UpdateUserUseCase(_mockRepository.Object);
    }

    [Fact]
    public async Task ExecuteAsync_DeveRetornarUserAtualizado_QuandoUsuarioExiste()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var existingUser = new User
        {
            Id = userId,
            NomeCompleto = "Nome Original",
            Sexo = SexoUsuario.Masculino,
            Email = "original@example.com",
            DataNascimento = new DateTime(1990, 1, 1),
            Nacionalidade = "Brasileiro",
            Naturalidade = "São Paulo",
            Cpf = "12345678909",
            DataCriacao = DateTime.Now.AddDays(-10),
            DataUltimaAtualizacao = DateTime.Now.AddDays(-10)
        };

        var updateDto = new UserUpdateDto
        {
            NomeCompleto = "Nome Atualizado",
            Sexo = SexoUsuario.Feminino,
            Email = "atualizado@example.com",
            DataNascimento = new DateTime(1995, 5, 5),
            Nacionalidade = "Brasileira",
            Naturalidade = "Várzea Grande",
            Cpf = "98765432100"
        };

        _mockRepository.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingUser);

        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mockRepository.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _useCase.ExecuteAsync(userId, updateDto);

        // Assert
        result.Should().NotBeNull();
        result!.NomeCompleto.Should().Be(updateDto.NomeCompleto);
        result.Email.Should().Be(updateDto.Email);
        result.Cpf.Should().Be(updateDto.Cpf);

        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
        _mockRepository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_DeveRetornarNull_QuandoUsuarioNaoExiste()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var updateDto = new UserUpdateDto
        {
            NomeCompleto = "Nome Atualizado",
            Sexo = SexoUsuario.Feminino,
            Email = "atualizado@example.com",
            DataNascimento = new DateTime(1995, 5, 5),
            Nacionalidade = "Brasileira",
            Naturalidade = "Várzea Grande",
            Cpf = "98765432100"
        };

        _mockRepository.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _useCase.ExecuteAsync(userId, updateDto);

        // Assert
        result.Should().BeNull();

        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
        _mockRepository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}

