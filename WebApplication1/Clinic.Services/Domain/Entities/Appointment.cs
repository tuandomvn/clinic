namespace Clinic.Services.Domain.Entities;

public enum AppointmentStatus
{
    Scheduled = 0,
    CheckedIn = 1,
    InProgress = 2,
    Completed = 3,

    //inactive status
    Cancelled = 4,
    NoShow = 5
}

public class Appointment
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public int StaffId { get; set; }
    public DateTime ScheduledAt { get; set; }
    public int DurationMinutes { get; set; } = 30;
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;
    public string? Notes { get; set; }
    public string? Reason { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }

    public Patient Patient { get; set; } = null!;
    public Staff Staff { get; set; } = null!;
}
