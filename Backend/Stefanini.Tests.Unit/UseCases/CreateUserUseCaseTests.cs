using FluentAssertions;
using Moq;
using Stefanini.Application.DTOs;
using Stefanini.Application.UseCases;
using Stefanini.Domain.Entities;
using Stefanini.Domain.Enums;
using Stefanini.Domain.Interfaces;

namespace Stefanini.Tests.Unit.UseCases;

public class CreateUserUseCaseTests
{
    private readonly Mock<IUserRepository> _mockRepository;
    private readonly CreateUserUseCase _useCase;

    public CreateUserUseCaseTests()
    {
        _mockRepository = new Mock<IUserRepository>();
        _useCase = new CreateUserUseCase(_mockRepository.Object);
    }

    [Fact]
    public async Task ExecuteAsync_DeveRetornarUserResponseDto_QuandoUsuarioCriadoComSucesso()
    {
        // Arrange
        var dto = new UserCreateDto
        {
            NomeCompleto = "João Silva",
            Sexo = SexoUsuario.Masculino,
            Email = "joao@example.com",
            DataNascimento = new DateTime(1990, 5, 15),
            Nacionalidade = "Brasileiro",
            Naturalidade = "São Paulo",
            Cpf = "12345678909"
        };

        _mockRepository.Setup(r => r.EmailExistsAsync(dto.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        
        _mockRepository.Setup(r => r.CpfExistsAsync(dto.Cpf, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _mockRepository.Setup(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .Returns((User u, CancellationToken ct) => Task.FromResult(u));

        _mockRepository.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _useCase.ExecuteAsync(dto);

        // Assert
        result.Should().NotBeNull();
        result.NomeCompleto.Should().Be(dto.NomeCompleto);
        result.Email.Should().Be(dto.Email);
        result.Cpf.Should().Be(dto.Cpf);

        _mockRepository.Verify(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
        _mockRepository.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_DeveLancarException_QuandoEmailJaExiste()
    {
        // Arrange
        var dto = new UserCreateDto
        {
            NomeCompleto = "João Silva",
            Sexo = SexoUsuario.Masculino,
            Email = "joao@example.com",
            DataNascimento = new DateTime(1990, 5, 15),
            Nacionalidade = "Brasileiro",
            Naturalidade = "São Paulo",
            Cpf = "12345678909"
        };

        _mockRepository.Setup(r => r.EmailExistsAsync(dto.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true); // Email já existe

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _useCase.ExecuteAsync(dto));

        _mockRepository.Verify(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_DeveLancarException_QuandoCpfJaExiste()
    {
        // Arrange
        var dto = new UserCreateDto
        {
            NomeCompleto = "João Silva",
            Sexo = SexoUsuario.Masculino,
            Email = "joao@example.com",
            DataNascimento = new DateTime(1990, 5, 15),
            Nacionalidade = "Brasileiro",
            Naturalidade = "São Paulo",
            Cpf = "12345678909"
        };

        _mockRepository.Setup(r => r.EmailExistsAsync(dto.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        
        _mockRepository.Setup(r => r.CpfExistsAsync(dto.Cpf, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true); // CPF já existe

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _useCase.ExecuteAsync(dto));

        _mockRepository.Verify(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}

