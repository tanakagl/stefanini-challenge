using FluentAssertions;
using Stefanini.Application.DTOs;
using Stefanini.Application.Mappings;
using Stefanini.Domain.Entities;

namespace Stefanini.Tests.Unit.Mappings;

public class AddressMapperTests
{
    [Fact]
    public void ToEntity_DeveConverterAddressDtoParaAddress()
    {
        var dto = new AddressDto
        {
            Rua = "Av. Historiador Rubens de Mendonça",
            Numero = "1000",
            Complemento = "Apto 501",
            Bairro = "Centro Norte",
            Cidade = "Cuiabá",
            Estado = "MT",
            Cep = "78000000"
        };

        var address = dto.ToEntity();

        address.Should().NotBeNull();
        address.Rua.Should().Be(dto.Rua);
        address.Numero.Should().Be(dto.Numero);
        address.Complemento.Should().Be(dto.Complemento);
        address.Bairro.Should().Be(dto.Bairro);
        address.Cidade.Should().Be(dto.Cidade);
        address.Estado.Should().Be(dto.Estado);
        address.Cep.Should().Be(dto.Cep);
    }

    [Fact]
    public void ToDto_DeveConverterAddressParaAddressDto()
    {
        var address = new Address
        {
            Rua = "Rua Pedro Celestino",
            Numero = "500",
            Complemento = "Casa",
            Bairro = "Centro",
            Cidade = "Cuiabá",
            Estado = "MT",
            Cep = "78005000"
        };

        var dto = address.ToDto();

        dto.Should().NotBeNull();
        dto.Rua.Should().Be(address.Rua);
        dto.Numero.Should().Be(address.Numero);
        dto.Complemento.Should().Be(address.Complemento);
        dto.Bairro.Should().Be(address.Bairro);
        dto.Cidade.Should().Be(address.Cidade);
        dto.Estado.Should().Be(address.Estado);
        dto.Cep.Should().Be(address.Cep);
    }

    [Fact]
    public void ToEntity_DeveConverterComplementoNulo()
    {
        // Arrange
        var dto = new AddressDto
        {
            Rua = "Rua Teste",
            Numero = "100",
            Complemento = null,
            Bairro = "Bairro Teste",
            Cidade = "Cidade Teste",
            Estado = "SP",
            Cep = "12345678"
        };

        // Act
        var address = dto.ToEntity();

        // Assert
        address.Complemento.Should().BeNull();
    }
}

