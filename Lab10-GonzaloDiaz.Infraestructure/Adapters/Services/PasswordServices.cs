using Lab10_GonzaloDiaz.Domain.Ports;
using Microsoft.AspNetCore.Identity;

namespace Lab10_GonzaloDiaz.Infraestructure.Adapters.Services;

public class PasswordHashService : IPasswordHasher
{
    private readonly PasswordHasher<object> _hasher = new();
    private readonly object _user = new();

    public string Hash(string password)
    {
        return _hasher.HashPassword(_user, password);
    }

    public bool Verify(string password, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            return false;
        }

        try
        {
            var result = _hasher.VerifyHashedPassword(
                _user,
                passwordHash,
                password);

            return result != PasswordVerificationResult.Failed;
        }
        catch (FormatException)
        {
            return false;
        }
    }
}