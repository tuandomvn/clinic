namespace Clinic.Services.Domain.Entities;

public class UserAccount
{
    public int Id { get; set; }

    public int StaffId { get; set; }
    public Staff Staff { get; set; } = null!;

    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// Application role (e.g. "Admin", "Doctor", "Nurse", ...). Used for authorization.
    /// </summary>
    public string Role { get; set; } = "User";

    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
}
