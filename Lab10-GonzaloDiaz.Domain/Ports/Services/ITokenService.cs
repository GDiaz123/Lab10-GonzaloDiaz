namespace Lab10_GonzaloDiaz.Domain.Ports;

public interface ITokenService
{
    string GenerateToken(
        Guid userId,
        string username,
        List<string> roles,
        DateTime expiresAt);
}