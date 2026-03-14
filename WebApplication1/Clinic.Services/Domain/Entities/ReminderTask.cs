namespace Clinic.Services.Domain.Entities;

public enum ReminderTaskType
{
    FollowUp = 0,
    BirthdayGreeting = 1,
    VaccinationReminder = 2,
    PeriodicTest = 3,
    General = 4
}

public enum TaskPriority
{
    Low = 0,
    Medium = 1,
    High = 2
}

public class ReminderTask
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public ReminderTaskType TaskType { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public TaskPriority Priority { get; set; }
    public bool IsDone { get; set; }
    public int? DoneByStaffId { get; set; }
    public DateTime? DoneAt { get; set; }
    public int CreatedBy { get; set; } = -1;
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public Patient Patient { get; set; } = null!;
}
