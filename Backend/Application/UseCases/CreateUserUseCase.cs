using Backend.Domain.Entities;
using Backend.Domain.Interfaces;

namespace Backend.Application.UseCases;

/// <summary>
/// Use Case para criar um novo usuário
/// Exemplo de como usar o Repository Pattern
/// </summary>
public class CreateUserUseCase
{
    private readonly IUserRepository _userRepository;

    public CreateUserUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> ExecuteAsync(User user, CancellationToken cancellationToken = default)
    {
        // Validações de negócio
        if (await _userRepository.EmailExistsAsync(user.Email, cancellationToken))
        {
            throw new InvalidOperationException("Email já cadastrado.");
        }

        if (await _userRepository.CpfExistsAsync(user.Cpf, cancellationToken))
        {
            throw new InvalidOperationException("CPF já cadastrado.");
        }

        // Adiciona o usuário
        await _userRepository.AddAsync(user, cancellationToken);
        
        // Salva no banco
        await _userRepository.SaveChangesAsync(cancellationToken);

        return user;
    }
}

