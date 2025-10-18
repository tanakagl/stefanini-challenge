using Stefanini.Application.DTOs;
using Stefanini.Application.Mappings;
using Stefanini.Domain.Interfaces;

namespace Stefanini.Application.UseCases;

public class GetUserByNameUseCase(IUserRepository userRepository)
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<IEnumerable<UserResponseDto>> ExecuteAsync(string nome, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(nome))
        {
            return Enumerable.Empty<UserResponseDto>();
        }

        var nomeLower = nome.ToLower();
        var users = await _userRepository.FindAsync(
            u => u.NomeCompleto.ToLower().Contains(nomeLower), 
            cancellationToken
        );
        
        return users.ToResponseDtoList();
    }
}

