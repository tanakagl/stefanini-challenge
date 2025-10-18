using Stefanini.Application.DTOs.V2;
using Stefanini.Domain.Entities;

namespace Stefanini.Application.Mappings;

public static class UserMapperV2
{
    public static User ToEntity(this UserCreateDtoV2 dto)
    {
        return new User
        {
            NomeCompleto = dto.NomeCompleto,
            Sexo = dto.Sexo,
            Email = dto.Email,
            DataNascimento = dto.DataNascimento,
            Nacionalidade = dto.Nacionalidade,
            Naturalidade = dto.Naturalidade,
            Cpf = dto.Cpf,
            Endereco = dto.Endereco.ToEntity()
        };
    }

    public static UserResponseDtoV2 ToResponseDtoV2(this User user)
    {
        return new UserResponseDtoV2
        {
            Id = user.Id,
            NomeCompleto = user.NomeCompleto,
            Sexo = user.Sexo,
            Email = user.Email,
            DataNascimento = user.DataNascimento,
            Nacionalidade = user.Nacionalidade,
            Naturalidade = user.Naturalidade,
            Cpf = user.Cpf,
            Endereco = user.Endereco?.ToDto(),
            DataCriacao = user.DataCriacao,
            DataUltimaAtualizacao = user.DataUltimaAtualizacao
        };
    }

    public static IEnumerable<UserResponseDtoV2> ToResponseDtoV2List(this IEnumerable<User> users)
    {
        return users.Select(u => u.ToResponseDtoV2());
    }

    public static void UpdateFromDtoV2(this User user, UserUpdateDtoV2 dto)
    {
        user.NomeCompleto = dto.NomeCompleto;
        user.Sexo = dto.Sexo;
        user.Email = dto.Email;
        user.DataNascimento = dto.DataNascimento;
        user.Nacionalidade = dto.Nacionalidade;
        user.Naturalidade = dto.Naturalidade;
        user.Cpf = dto.Cpf;
        user.Endereco = dto.Endereco.ToEntity();
    }
}

