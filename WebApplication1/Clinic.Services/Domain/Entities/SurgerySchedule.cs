namespace Clinic.Services.Domain.Entities;

public enum SurgeryStatus
{
    Scheduled = 0,
    InProgress = 1,
    Completed = 2,
    Cancelled = 3,
    Postponed = 4
}

public class SurgerySchedule
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public DateTime ScheduledAt { get; set; }
    public int DurationMinutes { get; set; } = 120;
    public string? Room { get; set; }
    public string? SurgeryType { get; set; }
    public string? Description { get; set; }
    public SurgeryStatus Status { get; set; } = SurgeryStatus.Scheduled;
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }

    public Patient Patient { get; set; } = null!;

    public ICollection<SurgeryScheduleStaff> TeamMembers { get; set; } = new List<SurgeryScheduleStaff>();
}
