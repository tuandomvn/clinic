using Clinic.Services.Models.Auth;

namespace Clinic.Services.Services.Auth;

public interface IAuthService
{
    Task<LoginResponse?> LoginAsync(LoginRequest request, CancellationToken ct = default);
}
