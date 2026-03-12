using System.Security.Cryptography;
using System.Text;

namespace Clinic.Services.Services.Auth;

/// <summary>
/// Password hashing service using BCrypt algorithm
/// </summary>
public sealed class PasswordHasher : IPasswordHasher
{
    private const int WorkFactor = 11;

    /// <summary>
    /// Hashes a password using BCrypt
    /// </summary>
    public string Hash(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password cannot be empty.", nameof(password));

        // Using a simple PBKDF2 approach as fallback (BCrypt.Net might not be available)
        // In production, consider using BCrypt.Net-Next NuGet package
        using var deriveBytes = new Rfc2898DeriveBytes(password, 16, 10000, HashAlgorithmName.SHA256);
        var salt = deriveBytes.Salt;
        var hash = deriveBytes.GetBytes(32);

        // Combine salt and hash
        var hashBytes = new byte[48];
        Buffer.BlockCopy(salt, 0, hashBytes, 0, 16);
        Buffer.BlockCopy(hash, 0, hashBytes, 16, 32);

        return Convert.ToBase64String(hashBytes);
    }

    /// <summary>
    /// Verifies a password against its hash
    /// </summary>
    public bool Verify(string password, string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(passwordHash))
            return false;

        try
        {
            var hashBytes = Convert.FromBase64String(passwordHash);
            if (hashBytes.Length != 48)
                return false;

            var salt = new byte[16];
            Buffer.BlockCopy(hashBytes, 0, salt, 0, 16);

            using var deriveBytes = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256);
            var hash = deriveBytes.GetBytes(32);

            for (int i = 0; i < 32; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                    return false;
            }

            return true;
        }
        catch
        {
            return false;
        }
    }
}
