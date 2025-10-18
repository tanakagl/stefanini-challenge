using Stefanini.Application.DTOs.V2;
using Stefanini.Application.Mappings;
using Stefanini.Domain.Interfaces;

namespace Stefanini.Application.UseCases.V2;

public class CreateUserUseCaseV2(IUserRepository userRepository)
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<UserResponseDtoV2> ExecuteAsync(UserCreateDtoV2 dto, CancellationToken cancellationToken = default)
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
        
        // Hash da senha
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        
        await _userRepository.AddAsync(user, cancellationToken);
        await _userRepository.SaveChangesAsync(cancellationToken);

        return user.ToResponseDtoV2();
    }
}

