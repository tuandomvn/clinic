using Clinic.Services.Domain.Entities;

namespace Clinic.Services.Services.Auth;

public interface IJwtTokenService
{
    (string Token, DateTime ExpiresAtUtc) CreateAccessToken(UserAccount account);
}
