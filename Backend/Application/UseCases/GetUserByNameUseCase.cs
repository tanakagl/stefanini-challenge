using Backend.Domain.Entities;
using Backend.Domain.Interfaces;

namespace Backend.Application.UseCases;

public class GetUserByNameUseCase(IUserRepository userRepository)
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<IEnumerable<User>> ExecuteAsync(string nome, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(nome))
        {
            return [];
        }

        return await _userRepository.FindAsync(
            u => u.NomeCompleto.Contains(nome, StringComparison.CurrentCultureIgnoreCase), 
            cancellationToken
        );
    }
}

