namespace Clinic.Services.Domain.Entities;

public enum ActivityType
{
    Post = 0,
    BirthdayGreeting = 1,
    AppointmentCreated = 2,
    HistoryRecordAdded = 3,
    SurgeryScheduled = 4,
    PatientRegistered = 5,
    General = 6
}

public class Activity
{
    public int Id { get; set; }
    public ActivityType ActivityType { get; set; }
    public string ContentText { get; set; } = string.Empty;
    public int CreatedBy { get; set; } = -1; //Staff create, -1 if system generated
    public int? PatientId { get; set; }
    public int? RelatedEncounterId { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public Patient? Patient { get; set; }
    public HealthRecord? RelatedEncounter { get; set; }
    public ICollection<ActivityImage> Images { get; set; } = new List<ActivityImage>();
}

public class ActivityImage
{
    public int Id { get; set; }
    public int ActivityId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string? Caption { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public Activity Activity { get; set; } = null!;
}
