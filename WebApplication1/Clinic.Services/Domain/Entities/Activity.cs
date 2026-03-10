namespace Clinic.Services.Domain.Entities;

public enum ActivityType
{
    AppointmentCreated = 0,
    AppointmentCompleted = 1,
    HistoryRecordAdded = 2,
    SurgeryScheduled = 3,
    PatientRegistered = 4,
    General = 5
}

public class Activity
{
    public int Id { get; set; }
    public ActivityType ActivityType { get; set; }
    public string Description { get; set; } = string.Empty;
    public int? StaffId { get; set; }
    public int? PatientId { get; set; }
    public string? EntityType { get; set; }
    public int? EntityId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Staff? Staff { get; set; }
    public Patient? Patient { get; set; }
}
