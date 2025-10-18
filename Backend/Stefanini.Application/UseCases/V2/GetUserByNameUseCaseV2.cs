using Stefanini.Application.DTOs.V2;
using Stefanini.Application.Mappings;
using Stefanini.Domain.Interfaces;

namespace Stefanini.Application.UseCases.V2;

public class GetUserByNameUseCaseV2(IUserRepository userRepository)
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<IEnumerable<UserResponseDtoV2>> ExecuteAsync(string nome, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(nome))
        {
            return Enumerable.Empty<UserResponseDtoV2>();
        }

        var nomeLower = nome.ToLower();
        var users = await _userRepository.FindAsync(
            u => u.NomeCompleto.ToLower().Contains(nomeLower), 
            cancellationToken
        );
        
        return users.ToResponseDtoV2List();
    }
}

