using FluentAssertions;
using Stefanini.Application.DTOs;
using Stefanini.Domain.Enums;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Stefanini.Tests.Integration.Controllers;

public class UsersControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonOptions;

    public UsersControllerIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() }
        };
    }

    [Fact]
    public async Task GetAll_DeveRetornarOk_ComListaDeUsuarios()
    {
        var response = await _client.GetAsync("/api/v1/users");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var users = await response.Content.ReadFromJsonAsync<List<UserResponseDto>>(_jsonOptions);
        users.Should().NotBeNull();
    }

    [Fact]
    public async Task Create_DeveRetornarCreated_QuandoDadosValidos()
    {
        var newUser = new UserCreateDto
        {
            NomeCompleto = "Matheo Bonucia",
            Sexo = SexoUsuario.Masculino,
            Email = $"matheo.bonucia{Guid.NewGuid()}@gmail.com",
            DataNascimento = new DateTime(2001, 10, 01),
            Nacionalidade = "Brasileiro",
            Naturalidade = "Cuiabá",
            Cpf = GenerateRandomCpf(),
            Password = "SenhaForte123!"
        };

        var response = await _client.PostAsJsonAsync("/api/v1/users", newUser, _jsonOptions);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        
        var createdUser = await response.Content.ReadFromJsonAsync<UserResponseDto>(_jsonOptions);
        createdUser.Should().NotBeNull();
        createdUser!.NomeCompleto.Should().Be(newUser.NomeCompleto);
        createdUser.Email.Should().Be(newUser.Email);
    }

    [Fact]
    public async Task Create_DeveRetornarBadRequest_QuandoEmailDuplicado()
    {
        var email = $"duplicate{Guid.NewGuid()}@gmail.com";
        var user1 = new UserCreateDto
        {
            NomeCompleto = "User 1",
            Sexo = SexoUsuario.Masculino,
            Email = email,
            DataNascimento = new DateTime(1990, 1, 1),
            Nacionalidade = "Brasileiro",
            Naturalidade = "São Paulo",
            Cpf = GenerateRandomCpf(),
            Password = "SenhaForte123!"
        };

        var user2 = new UserCreateDto
        {
            NomeCompleto = "User 2",
            Sexo = SexoUsuario.Feminino,
            Email = email,
            DataNascimento = new DateTime(1992, 2, 2),
            Nacionalidade = "Brasileira",
            Naturalidade = "Rio de Janeiro",
            Cpf = GenerateRandomCpf(),
            Password = "SenhaForte123!"
        };

        await _client.PostAsJsonAsync("/api/v1/users", user1, _jsonOptions);
        var response = await _client.PostAsJsonAsync("/api/v1/users", user2, _jsonOptions);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Update_DeveRetornarOk_QuandoUsuarioExiste()
    {
        var createDto = new UserCreateDto
        {
            NomeCompleto = "Matheo Bonucia",
            Sexo = SexoUsuario.Masculino,
            Email = $"matheo{Guid.NewGuid()}@gmail.com",
            DataNascimento = new DateTime(2001, 10, 01),
            Nacionalidade = "Brasileiro",
            Naturalidade = "Cuiabá",
            Cpf = GenerateRandomCpf(),
            Password = "SenhaForte123!"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/v1/users", createDto, _jsonOptions);
        var createdUser = await createResponse.Content.ReadFromJsonAsync<UserResponseDto>(_jsonOptions);

        var updateDto = new UserUpdateDto
        {
            NomeCompleto = "Matheo Bonucia Silva",
            Sexo = SexoUsuario.Masculino,
            Email = createDto.Email,
            DataNascimento = new DateTime(2001, 10, 01),
            Nacionalidade = "Brasileiro",
            Naturalidade = "Várzea Grande",
            Cpf = createDto.Cpf
        };

        var response = await _client.PutAsJsonAsync($"/api/v1/users/{createdUser!.Id}", updateDto, _jsonOptions);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var updatedUser = await response.Content.ReadFromJsonAsync<UserResponseDto>(_jsonOptions);
        updatedUser.Should().NotBeNull();
        updatedUser!.NomeCompleto.Should().Be("Matheo Bonucia Silva");
    }

    [Fact]
    public async Task Update_DeveRetornarNotFound_QuandoUsuarioNaoExiste()
    {
        var updateDto = new UserUpdateDto
        {
            NomeCompleto = "Non Existent User",
            Sexo = SexoUsuario.Masculino,
            Email = "nonexistent@gmail.com",
            DataNascimento = new DateTime(1990, 1, 1),
            Nacionalidade = "Brasileiro",
            Naturalidade = "São Paulo",
            Cpf = "12345678909"
        };

        var nonExistentId = Guid.NewGuid();

        var response = await _client.PutAsJsonAsync($"/api/v1/users/{nonExistentId}", updateDto, _jsonOptions);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_DeveRetornarNoContent_QuandoUsuarioExiste()
    {
        var createDto = new UserCreateDto
        {
            NomeCompleto = "User To Delete",
            Sexo = SexoUsuario.Masculino,
            Email = $"delete{Guid.NewGuid()}@gmail.com",
            DataNascimento = new DateTime(1990, 1, 1),
            Nacionalidade = "Brasileiro",
            Naturalidade = "São Paulo",
            Cpf = GenerateRandomCpf(),
            Password = "SenhaForte123!"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/v1/users", createDto, _jsonOptions);
        var createdUser = await createResponse.Content.ReadFromJsonAsync<UserResponseDto>(_jsonOptions);

        var response = await _client.DeleteAsync($"/api/v1/users/{createdUser!.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Delete_DeveRetornarNotFound_QuandoUsuarioNaoExiste()
    {
        var nonExistentId = Guid.NewGuid();

        var response = await _client.DeleteAsync($"/api/v1/users/{nonExistentId}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetByName_DeveRetornarOk_ComUsuariosFiltrados()
    {
        var user1 = new UserCreateDto
        {
            NomeCompleto = "Matheo Bonucia",
            Sexo = SexoUsuario.Masculino,
            Email = $"matheo.search{Guid.NewGuid()}@gmail.com",
            DataNascimento = new DateTime(2001, 10, 01),
            Nacionalidade = "Brasileiro",
            Naturalidade = "Cuiabá",
            Cpf = GenerateRandomCpf(),
            Password = "SenhaForte123!"
        };

        await _client.PostAsJsonAsync("/api/v1/users", user1, _jsonOptions);

        var response = await _client.GetAsync("/api/v1/users/search?nome=Matheo");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var users = await response.Content.ReadFromJsonAsync<List<UserResponseDto>>(_jsonOptions);
        users.Should().NotBeNull();
        users.Should().NotBeEmpty();
        users!.Any(u => u.NomeCompleto.Contains("Matheo")).Should().BeTrue();
    }

    private string GenerateRandomCpf()
    {
        var random = new Random();
        var cpf = "";
        
        for (int i = 0; i < 9; i++)
        {
            cpf += random.Next(0, 10).ToString();
        }
        
        var multiplicador1 = new[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        var multiplicador2 = new[] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        
        var soma = 0;
        for (int i = 0; i < 9; i++)
        {
            soma += int.Parse(cpf[i].ToString()) * multiplicador1[i];
        }
        
        var resto = soma % 11;
        var digito1 = resto < 2 ? 0 : 11 - resto;
        cpf += digito1.ToString();
        
        soma = 0;
        for (int i = 0; i < 10; i++)
        {
            soma += int.Parse(cpf[i].ToString()) * multiplicador2[i];
        }
        
        resto = soma % 11;
        var digito2 = resto < 2 ? 0 : 11 - resto;
        cpf += digito2.ToString();
        
        return cpf;
    }
}

