using Lab10_GonzaloDiaz.Application.Dtos;
using Lab10_GonzaloDiaz.Domain.Ports;

namespace Lab10_GonzaloDiaz.Application.UserCase.AuthUseCase;

public class LoginUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public LoginUseCase(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        ITokenService tokenService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<LoginResponseDto> Execute(LoginDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Username) ||
            string.IsNullOrWhiteSpace(dto.Password))
        {
            throw new UnauthorizedAccessException(
                "Usuario o contraseña incorrectos.");
        }

        var user = await _userRepository
            .ObtenerPorUsernameAsync(dto.Username.Trim());

        if (user == null ||
            !_passwordHasher.Verify(dto.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException(
                "Usuario o contraseña incorrectos.");
        }

        var roles = await _userRepository
            .ObtenerRolesAsync(user.UserId);

        var expiresAt = DateTime.UtcNow.AddHours(1);

        var token = _tokenService.GenerateToken(
            user.UserId,
            user.Username,
            roles,
            expiresAt);

        return new LoginResponseDto
        {
            AccessToken = token,
            ExpiresAt = expiresAt,
            Username = user.Username,
            Roles = roles
        };
    }
}