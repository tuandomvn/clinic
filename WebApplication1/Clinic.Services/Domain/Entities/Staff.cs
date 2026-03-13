namespace Clinic.Services.Domain.Entities;

public enum StaffType
{
    Doctor = 0,
    Nurse = 1,
    Technician = 2,
    Admin = 3
}

public class Staff
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public StaffType StaffType { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Specialization { get; set; }
    public string? AvatarPath { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public UserAccount? UserAccount { get; set; }

    public ICollection<HealthRecord> HistoryRecords { get; set; } = new List<HealthRecord>();
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public ICollection<SurgeryScheduleStaff> SurgeryAssignments { get; set; } = new List<SurgeryScheduleStaff>();
}
