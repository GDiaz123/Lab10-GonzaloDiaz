using Lab10_GonzaloDiaz.Application.Dtos;
using Lab10_GonzaloDiaz.Domain.Ports;
using Lab10_GonzaloDiaz.Models;

namespace Lab10_GonzaloDiaz.Application.UserCase.AuthUseCase;

public class RegisterUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterUseCase(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<UserDto> Execute(RegisterDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Username))
        {
            throw new ArgumentException("El nombre de usuario es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(dto.Password) ||
            dto.Password.Length < 8)
        {
            throw new ArgumentException(
                "La contraseña debe tener al menos 8 caracteres.");
        }

        var username = dto.Username.Trim();

        var existingUser =
            await _userRepository.ObtenerPorUsernameAsync(username);

        if (existingUser != null)
        {
            throw new InvalidOperationException(
                "El nombre de usuario ya está registrado.");
        }

        var email = string.IsNullOrWhiteSpace(dto.Email)
            ? null
            : dto.Email.Trim();

        if (email != null)
        {
            var existingEmail =
                await _userRepository.ObtenerPorEmailAsync(email);

            if (existingEmail != null)
            {
                throw new InvalidOperationException(
                    "El correo ya está registrado.");
            }
        }

        var user = new User
        {
            UserId = Guid.NewGuid(),
            Username = username,
            PasswordHash = _passwordHasher.Hash(dto.Password),
            Email = email,
            CreatedAt = DateTime.SpecifyKind(
                DateTime.UtcNow,
                DateTimeKind.Unspecified)
        };

        await _userRepository.AddAndSaveAsync(user);

        return new UserDto
        {
            UserId = user.UserId,
            Username = user.Username,
            Email = user.Email,
            CreatedAt = user.CreatedAt
        };
    }
}