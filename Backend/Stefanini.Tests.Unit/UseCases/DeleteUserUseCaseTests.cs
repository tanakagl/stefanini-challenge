using FluentAssertions;
using Moq;
using Stefanini.Application.UseCases;
using Stefanini.Domain.Entities;
using Stefanini.Domain.Enums;
using Stefanini.Domain.Interfaces;

namespace Stefanini.Tests.Unit.UseCases;

public class DeleteUserUseCaseTests
{
    private readonly Mock<IUserRepository> _mockRepository;
    private readonly DeleteUserUseCase _useCase;

    public DeleteUserUseCaseTests()
    {
        _mockRepository = new Mock<IUserRepository>();
        _useCase = new DeleteUserUseCase(_mockRepository.Object);
    }

    [Fact]
    public async Task ExecuteAsync_DeveRetornarTrue_QuandoUsuarioDeletadoComSucesso()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var existingUser = new User
        {
            Id = userId,
            NomeCompleto = "User to Delete",
            Sexo = SexoUsuario.Masculino,
            Email = "delete@example.com",
            DataNascimento = new DateTime(1990, 1, 1),
            Nacionalidade = "Brasileiro",
            Naturalidade = "São Paulo",
            Cpf = "12345678909",
            DataCriacao = DateTime.Now,
            DataUltimaAtualizacao = DateTime.Now
        };

        _mockRepository.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingUser);

        _mockRepository.Setup(r => r.DeleteAsync(existingUser, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _mockRepository.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _useCase.ExecuteAsync(userId);

        // Assert
        result.Should().BeTrue();

        _mockRepository.Verify(r => r.DeleteAsync(existingUser, It.IsAny<CancellationToken>()), Times.Once);
        _mockRepository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_DeveRetornarFalse_QuandoUsuarioNaoExiste()
    {
        // Arrange
        var userId = Guid.NewGuid();

        _mockRepository.Setup(r => r.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _useCase.ExecuteAsync(userId);

        // Assert
        result.Should().BeFalse();

        _mockRepository.Verify(r => r.DeleteAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
        _mockRepository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}

