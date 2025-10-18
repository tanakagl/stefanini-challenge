using Stefanini.Application.DTOs;
using Stefanini.Domain.Entities;

namespace Stefanini.Application.Mappings;

public static class UserMapper
{
    public static User ToEntity(this UserCreateDto dto)
    {
        return new User
        {
            NomeCompleto = dto.NomeCompleto,
            Sexo = dto.Sexo,
            Email = dto.Email,
            DataNascimento = dto.DataNascimento,
            Nacionalidade = dto.Nacionalidade,
            Naturalidade = dto.Naturalidade,
            Cpf = dto.Cpf
        };
    }

    public static UserResponseDto ToResponseDto(this User user)
    {
        return new UserResponseDto
        {
            Id = user.Id,
            NomeCompleto = user.NomeCompleto,
            Sexo = user.Sexo,
            Email = user.Email,
            DataNascimento = user.DataNascimento,
            Nacionalidade = user.Nacionalidade,
            Naturalidade = user.Naturalidade,
            Cpf = user.Cpf,
            DataCriacao = user.DataCriacao,
            DataUltimaAtualizacao = user.DataUltimaAtualizacao
        };
    }

    public static IEnumerable<UserResponseDto> ToResponseDtoList(this IEnumerable<User> users)
    {
        return users.Select(u => u.ToResponseDto());
    }

    public static void UpdateFromDto(this User user, UserUpdateDto dto)
    {
        user.NomeCompleto = dto.NomeCompleto;
        user.Sexo = dto.Sexo;
        user.Email = dto.Email;
        user.DataNascimento = dto.DataNascimento;
        user.Nacionalidade = dto.Nacionalidade;
        user.Naturalidade = dto.Naturalidade;
        user.Cpf = dto.Cpf;
    }
}

