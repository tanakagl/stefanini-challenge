using FluentAssertions;
using Moq;
using Stefanini.Application.UseCases;
using Stefanini.Domain.Entities;
using Stefanini.Domain.Enums;
using Stefanini.Domain.Interfaces;
using System.Linq.Expressions;

namespace Stefanini.Tests.Unit.UseCases;

public class GetUserByNameUseCaseTests
{
    private readonly Mock<IUserRepository> _mockRepository;
    private readonly GetUserByNameUseCase _useCase;

    public GetUserByNameUseCaseTests()
    {
        _mockRepository = new Mock<IUserRepository>();
        _useCase = new GetUserByNameUseCase(_mockRepository.Object);
    }

    [Fact]
    public async Task ExecuteAsync_DeveRetornarUsuarios_QuandoEncontrarPorNome()
    {
        // Arrange
        var users = new List<User>
        {
            new User
            {
                Id = Guid.NewGuid(),
                NomeCompleto = "João Silva",
                Sexo = SexoUsuario.Masculino,
                Email = "joao@example.com",
                DataNascimento = new DateTime(1990, 1, 1),
                Nacionalidade = "Brasileiro",
                Naturalidade = "São Paulo",
                Cpf = "11111111111",
                DataCriacao = DateTime.Now,
                DataUltimaAtualizacao = DateTime.Now
            },
            new User
            {
                Id = Guid.NewGuid(),
                NomeCompleto = "João Santos",
                Sexo = SexoUsuario.Masculino,
                Email = "joaosantos@example.com",
                DataNascimento = new DateTime(1992, 2, 2),
                Nacionalidade = "Brasileiro",
                Naturalidade = "Rio de Janeiro",
                Cpf = "22222222222",
                DataCriacao = DateTime.Now,
                DataUltimaAtualizacao = DateTime.Now
            }
        };

        _mockRepository.Setup(r => r.FindAsync(
            It.IsAny<Expression<Func<User, bool>>>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(users);

        // Act
        var result = await _useCase.ExecuteAsync("João");

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.All(u => u.NomeCompleto.Contains("João")).Should().BeTrue();

        _mockRepository.Verify(r => r.FindAsync(
            It.IsAny<Expression<Func<User, bool>>>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_DeveRetornarListaVazia_QuandoNomeForVazio()
    {
        // Act
        var result = await _useCase.ExecuteAsync("");

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();

        _mockRepository.Verify(r => r.FindAsync(
            It.IsAny<Expression<Func<User, bool>>>(),
            It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_DeveRetornarListaVazia_QuandoNomeForNull()
    {
        // Act
        var result = await _useCase.ExecuteAsync(null!);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();

        _mockRepository.Verify(r => r.FindAsync(
            It.IsAny<Expression<Func<User, bool>>>(),
            It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_DeveRetornarListaVazia_QuandoNenhumUsuarioEncontrado()
    {
        // Arrange
        _mockRepository.Setup(r => r.FindAsync(
            It.IsAny<Expression<Func<User, bool>>>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<User>());

        // Act
        var result = await _useCase.ExecuteAsync("NomeInexistente");

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}

