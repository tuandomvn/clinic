using System.Security.Claims;
using Clinic.Services.Domain.Entities;

namespace Clinic.Services.Services.Auth;

/// <summary>
/// JWT token service for creating access tokens
/// Since we're using cookie authentication, this generates a simple token for reference
/// </summary>
public sealed class JwtTokenService : IJwtTokenService
{
    private const int AccessTokenMinutes = 480; // 8 hours

    /// <summary>
    /// Creates an access token for the given user account
    /// </summary>
    public (string Token, DateTime ExpiresAtUtc) CreateAccessToken(UserAccount account)
    {
        var now = DateTime.UtcNow;
        var expires = now.AddMinutes(AccessTokenMinutes);

        // Simple token format: base64(userId:staffId:username:role:timestamp)
        var tokenData = $"{account.Id}:{account.StaffId}:{account.Username}:{account.Role}:{now.Ticks}";
        var token = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(tokenData));

        return (token, expires);
    }
}
