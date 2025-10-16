using Backend.Domain.Entities;
using Backend.Domain.Enums;
using Backend.Domain.Interfaces;
using Backend.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

/// <summary>
/// Implementação específica do Repository de User
/// Herda de Repository<User> para ter métodos básicos
/// Implementa IUserRepository para queries específicas
/// </summary>
public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }

    // Queries específicas de User
    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<User?> GetByCpfAsync(string cpf, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(u => u.Cpf == cpf, cancellationToken);
    }

    public async Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<bool> CpfExistsAsync(string cpf, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(u => u.Cpf == cpf, cancellationToken);
    }

    public async Task<IEnumerable<User>> GetByNacionalidadeAsync(
        string nacionalidade, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(u => u.Nacionalidade == nacionalidade)
            .OrderBy(u => u.NomeCompleto)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<User>> GetBySexoAsync(
        SexoUsuario sexo, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(u => u.Sexo == sexo)
            .OrderBy(u => u.NomeCompleto)
            .ToListAsync(cancellationToken);
    }
}

