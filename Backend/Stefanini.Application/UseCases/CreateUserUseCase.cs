using Stefanini.Application.DTOs;
using Stefanini.Application.Mappings;
using Stefanini.Domain.Interfaces;

namespace Stefanini.Application.UseCases;

public class CreateUserUseCase(IUserRepository userRepository)
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<UserResponseDto> ExecuteAsync(UserCreateDto dto, CancellationToken cancellationToken = default)
    {
        if (await _userRepository.EmailExistsAsync(dto.Email, cancellationToken))
        {
            throw new InvalidOperationException("Email já cadastrado.");
        }

        if (await _userRepository.CpfExistsAsync(dto.Cpf, cancellationToken))
        {
            throw new InvalidOperationException("CPF já cadastrado.");
        }

        var user = dto.ToEntity();
        await _userRepository.AddAsync(user, cancellationToken);
        
        await _userRepository.SaveChangesAsync(cancellationToken);

        return user.ToResponseDto();
    }
}

