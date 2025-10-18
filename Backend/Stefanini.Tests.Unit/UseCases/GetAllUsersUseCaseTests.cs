using FluentAssertions;
using Moq;
using Stefanini.Application.UseCases;
using Stefanini.Domain.Entities;
using Stefanini.Domain.Enums;
using Stefanini.Domain.Interfaces;

namespace Stefanini.Tests.Unit.UseCases;

public class GetAllUsersUseCaseTests
{
    private readonly Mock<IUserRepository> _mockRepository;
    private readonly GetAllUsersUseCase _useCase;

    public GetAllUsersUseCaseTests()
    {
        _mockRepository = new Mock<IUserRepository>();
        _useCase = new GetAllUsersUseCase(_mockRepository.Object);
    }

    [Fact]
    public async Task ExecuteAsync_DeveRetornarListaDeUsuarios_QuandoExistemUsuarios()
    {
        // Arrange
        var users = new List<User>
        {
            new User
            {
                Id = Guid.NewGuid(),
                NomeCompleto = "User 1",
                Sexo = SexoUsuario.Masculino,
                Email = "user1@example.com",
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
                NomeCompleto = "User 2",
                Sexo = SexoUsuario.Feminino,
                Email = "user2@example.com",
                DataNascimento = new DateTime(1992, 2, 2),
                Nacionalidade = "Brasileira",
                Naturalidade = "Rio de Janeiro",
                Cpf = "22222222222",
                DataCriacao = DateTime.Now,
                DataUltimaAtualizacao = DateTime.Now
            }
        };

        _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(users);

        // Act
        var result = await _useCase.ExecuteAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.First().NomeCompleto.Should().Be("User 1");
        result.Last().NomeCompleto.Should().Be("User 2");

        _mockRepository.Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_DeveRetornarListaVazia_QuandoNaoExistemUsuarios()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<User>());

        // Act
        var result = await _useCase.ExecuteAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();

        _mockRepository.Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}

