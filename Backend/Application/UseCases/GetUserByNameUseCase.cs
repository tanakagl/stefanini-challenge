using Backend.Application.DTOs;
using Backend.Application.Mappings;
using Backend.Domain.Interfaces;

namespace Backend.Application.UseCases;

public class GetUserByNameUseCase(IUserRepository userRepository)
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<IEnumerable<UserResponseDto>> ExecuteAsync(string nome, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(nome))
        {
            return [];
        }

        var nomeLower = nome.ToLower();
        var users = await _userRepository.FindAsync(
            u => u.NomeCompleto.ToLower().Contains(nomeLower), 
            cancellationToken
        );
        
        return users.ToResponseDtoList();
    }
}

