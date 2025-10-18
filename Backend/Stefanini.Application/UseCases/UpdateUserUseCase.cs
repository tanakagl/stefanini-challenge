using Stefanini.Application.DTOs;
using Stefanini.Application.Mappings;
using Stefanini.Domain.Interfaces;

namespace Stefanini.Application.UseCases;

public class UpdateUserUseCase(IUserRepository userRepository)
{
    private readonly IUserRepository _userRepository = userRepository;
    public async Task<UserResponseDto?> ExecuteAsync(Guid id, UserUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken);
        if (user is null)
        {
            return null;
        }
        
        user.UpdateFromDto(dto);
        await _userRepository.UpdateAsync(user, cancellationToken);
        await _userRepository.SaveChangesAsync(cancellationToken);
        return user.ToResponseDto();
    }
}

