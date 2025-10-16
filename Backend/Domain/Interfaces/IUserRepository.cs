using Backend.Domain.Entities;
using Backend.Domain.Enums;

namespace Backend.Domain.Interfaces;

/// <summary>
/// Interface específica de User com queries customizadas
/// Herda de IRepository<User> para ter os métodos básicos
/// </summary>
public interface IUserRepository : IRepository<User>
{
    // Queries específicas de User
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<User?> GetByCpfAsync(string cpf, CancellationToken cancellationToken = default);
    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> CpfExistsAsync(string cpf, CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetByNacionalidadeAsync(string nacionalidade, CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> GetBySexoAsync(SexoUsuario sexo, CancellationToken cancellationToken = default);
}

