using FluentAssertions;
using Stefanini.Application.DTOs;
using Stefanini.Application.Mappings;
using Stefanini.Domain.Entities;
using Stefanini.Domain.Enums;

namespace Stefanini.Tests.Unit.Mappings;

public class UserMapperTests
{
    [Fact]
    public void ToEntity_DeveConverterUserCreateDtoParaUser()
    {
        var dto = new UserCreateDto
        {
            NomeCompleto = "Matheo Bonucia",
            Sexo = SexoUsuario.Masculino,
            Email = "matheo.bonucia@example.com",
            DataNascimento = new DateTime(2001, 10, 01),
            Nacionalidade = "Brasileiro",
            Naturalidade = "Cuiabá",
            Cpf = "12345678909"
        };

        var user = dto.ToEntity();

        user.Should().NotBeNull();
        user.NomeCompleto.Should().Be(dto.NomeCompleto);
        user.Sexo.Should().Be(dto.Sexo);
        user.Email.Should().Be(dto.Email);
        user.DataNascimento.Should().Be(dto.DataNascimento);
        user.Nacionalidade.Should().Be(dto.Nacionalidade);
        user.Naturalidade.Should().Be(dto.Naturalidade);
        user.Cpf.Should().Be(dto.Cpf);
    }

    [Fact]
    public void ToResponseDto_DeveConverterUserParaUserResponseDto()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            NomeCompleto = "Matheo Bonucia",
            Sexo = SexoUsuario.Masculino,
            Email = "matheo@example.com",
            DataNascimento = new DateTime(2001, 10, 01),
            Nacionalidade = "Brasileiro",
            Naturalidade = "Cuiabá",
            Cpf = "98765432100",
            DataCriacao = DateTime.Now,
            DataUltimaAtualizacao = DateTime.Now
        };

        var dto = user.ToResponseDto();

        dto.Should().NotBeNull();
        dto.Id.Should().Be(user.Id);
        dto.NomeCompleto.Should().Be(user.NomeCompleto);
        dto.Sexo.Should().Be(user.Sexo);
        dto.Email.Should().Be(user.Email);
        dto.DataNascimento.Should().Be(user.DataNascimento);
        dto.Nacionalidade.Should().Be(user.Nacionalidade);
        dto.Naturalidade.Should().Be(user.Naturalidade);
        dto.Cpf.Should().Be(user.Cpf);
        dto.DataCriacao.Should().Be(user.DataCriacao);
        dto.DataUltimaAtualizacao.Should().Be(user.DataUltimaAtualizacao);
    }

    [Fact]
    public void ToResponseDtoList_DeveConverterListaDeUsers()
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

        // Act
        var dtos = users.ToResponseDtoList().ToList();

        // Assert
        dtos.Should().HaveCount(2);
        dtos[0].NomeCompleto.Should().Be("User 1");
        dtos[1].NomeCompleto.Should().Be("User 2");
    }

    [Fact]
    public void UpdateFromDto_DeveAtualizarUserComDadosDoDto()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            NomeCompleto = "Nome Original",
            Sexo = SexoUsuario.Masculino,
            Email = "original@example.com",
            DataNascimento = new DateTime(1990, 1, 1),
            Nacionalidade = "Brasileiro",
            Naturalidade = "Cuiabá",
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

        // Act
        user.UpdateFromDto(updateDto);

        // Assert
        user.NomeCompleto.Should().Be(updateDto.NomeCompleto);
        user.Sexo.Should().Be(updateDto.Sexo);
        user.Email.Should().Be(updateDto.Email);
        user.DataNascimento.Should().Be(updateDto.DataNascimento);
        user.Nacionalidade.Should().Be(updateDto.Nacionalidade);
        user.Naturalidade.Should().Be(updateDto.Naturalidade);
        user.Cpf.Should().Be(updateDto.Cpf);
        // ID e DataCriacao não devem mudar
        user.Id.Should().NotBeEmpty();
        user.DataCriacao.Should().BeBefore(DateTime.Now);
    }
}

