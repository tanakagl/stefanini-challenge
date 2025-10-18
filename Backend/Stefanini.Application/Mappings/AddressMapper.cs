using Stefanini.Application.DTOs;
using Stefanini.Domain.Entities;

namespace Stefanini.Application.Mappings;

public static class AddressMapper
{
    public static Address ToEntity(this AddressDto dto)
    {
        return new Address
        {
            Rua = dto.Rua,
            Numero = dto.Numero,
            Complemento = dto.Complemento,
            Bairro = dto.Bairro,
            Cidade = dto.Cidade,
            Estado = dto.Estado,
            Cep = dto.Cep.Replace("-", "")
        };
    }

    public static AddressDto ToDto(this Address address)
    {
        return new AddressDto
        {
            Rua = address.Rua,
            Numero = address.Numero,
            Complemento = address.Complemento,
            Bairro = address.Bairro,
            Cidade = address.Cidade,
            Estado = address.Estado,
            Cep = address.Cep
        };
    }
}

