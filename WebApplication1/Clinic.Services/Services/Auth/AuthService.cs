using Microsoft.EntityFrameworkCore;
using Clinic.Services.Data;
using Clinic.Services.Models.Auth;

namespace Clinic.Services.Services.Auth;

public sealed class AuthService : IAuthService
{
    private readonly ClinicDbContext _db;
    private readonly IPasswordHasher _hasher;
    private readonly IJwtTokenService _jwt;

    public AuthService(ClinicDbContext db, IPasswordHasher hasher, IJwtTokenService jwt)
    {
        _db = db;
        _hasher = hasher;
        _jwt = jwt;
    }

    public async Task<LoginResponse?> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            return null;

        var account = await _db.UserAccounts
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Username == request.Username && x.IsActive, ct);

        if (account == null || !_hasher.Verify(request.Password, account.PasswordHash))
            return null;

        var (token, expiresAtUtc) = _jwt.CreateAccessToken(account);

        await _db.UserAccounts.Where(x => x.Id == account.Id)
            .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.LastLoginAt, DateTime.UtcNow), ct);

        return new LoginResponse
        {
            AccessToken = token,
            ExpiresAtUtc = expiresAtUtc,
            StaffId = account.StaffId,
            Username = account.Username,
            Role = account.Role
        };
    }
}
