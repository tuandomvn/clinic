using Clinic.Services.Data;
using Clinic.Services.Domain.Entities;
using Clinic.Services.Services.Auth;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Web.Services;

public static class DatabaseSeeder
{
    /// <summary>
    /// Seeds initial admin user if no users exist
    /// </summary>
    public static async Task SeedAdminUserAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ClinicDbContext>();
        var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

        // Check if any users exist
        if (await db.UserAccounts.AnyAsync())
            return;

        // Check if any staff exist
        var staff = await db.Staff.FirstOrDefaultAsync();
        if (staff == null)
        {
            // Create a default admin staff member
            staff = new Staff
            {
                FullName = "Admin User",
                Email = "admin@clinic.local",
                Specialization = "Administrator",
                Phone = "0000000000"
            };
            db.Staff.Add(staff);
            await db.SaveChangesAsync();
        }

        // Create admin user account
        var adminAccount = new UserAccount
        {
            StaffId = staff.Id,
            Username = "admin",
            PasswordHash = hasher.Hash("admin123"), // Change this in production!
            Role = "Admin",
            IsActive = true
        };

        db.UserAccounts.Add(adminAccount);
        await db.SaveChangesAsync();
    }

    /// <summary>
    /// Create a new user account for a staff member
    /// </summary>
    public static async Task<bool> CreateUserAccountAsync(
        IServiceProvider serviceProvider,
        int staffId,
        string username,
        string password,
        string role = "User")
    {
        using var scope = serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ClinicDbContext>();
        var hasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

        // Check if username already exists
        if (await db.UserAccounts.AnyAsync(u => u.Username == username))
            return false;

        // Check if staff exists
        var staff = await db.Staff.FindAsync(staffId);
        if (staff == null)
            return false;

        var userAccount = new UserAccount
        {
            StaffId = staffId,
            Username = username,
            PasswordHash = hasher.Hash(password),
            Role = role,
            IsActive = true
        };

        db.UserAccounts.Add(userAccount);
        await db.SaveChangesAsync();
        return true;
    }
}
