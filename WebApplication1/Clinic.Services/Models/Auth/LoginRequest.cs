namespace Clinic.Services.Models.Auth;

public sealed class LoginRequest
{
    public string Username { get; set; } = "doctor1";
    public string Password { get; set; } = "Password@123";
}
