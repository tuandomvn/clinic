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
    /// Seeds approximately 100 appointments for testing purposes
    /// </summary>
    public static async Task SeedAppointmentsAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ClinicDbContext>();

        // Check if appointments already exist
        if (await db.Appointments.AnyAsync())
            return;

        // Get available patients and staff
        var patients = await db.Patients.ToListAsync();
        var staff = await db.Staff.ToListAsync();

        if (patients.Count == 0 || staff.Count == 0)
            return; // No data to seed with

        var appointments = new List<Appointment>();
        var appointmentStatuses = new[] 
        { 
            AppointmentStatus.Scheduled,
            AppointmentStatus.CheckedIn,
            AppointmentStatus.InProgress,
            AppointmentStatus.Completed,
            AppointmentStatus.NoShow
        };

        var reasons = new[]
        {
            "Khám tổng quát",
            "Kiểm tra sức khỏe định kỳ",
            "Tái khám theo đơn",
            "Đau đầu",
            "Cảm cúm",
            "Kiểm tra mắt",
            "Kiểm tra răng",
            "Theo dõi bệnh mãn tính",
            "Chấn thương",
            "Bệnh tim mạch"
        };

        var random = new Random(42); // Seed for reproducible results
        var baseDate = DateTime.UtcNow.AddDays(-30);

        for (int i = 0; i < 100; i++)
        {
            var patient = patients[random.Next(patients.Count)];
            var doctor = staff[random.Next(staff.Count)];
            var status = appointmentStatuses[random.Next(appointmentStatuses.Length)];
            var reason = reasons[random.Next(reasons.Length)];

            // Distribute appointments across the next 60 days
            var daysOffset = random.Next(0, 60);
            var hoursOffset = random.Next(8, 17); // Between 8am and 5pm
            var minutesOffset = random.Next(0, 60);

            var appointmentTime = baseDate
                .AddDays(daysOffset)
                .AddHours(hoursOffset)
                .AddMinutes(minutesOffset);

            var appointment = new Appointment
            {
                PatientId = patient.Id,
                StaffId = doctor.Id,
                ScheduledAt = appointmentTime,
                Status = status,
                Reason = reason,
                Notes = $"Appointment note {i + 1}",
                DurationMinutes = 30,
                CreatedAt = appointmentTime.AddDays(-random.Next(1, 10))
            };

            appointments.Add(appointment);
        }

        db.Appointments.AddRange(appointments);
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
