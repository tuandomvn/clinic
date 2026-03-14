namespace Clinic.Services.Domain.Entities;

public class SurgeryScheduleStaff
{
    public int SurgeryScheduleId { get; set; }
    public SurgerySchedule SurgerySchedule { get; set; } = null!;

    public int StaffId { get; set; }
    public Staff Staff { get; set; } = null!;

    /// <summary>Role of staff in the surgery team (e.g. "Surgeon", "Assistant", "Anesthetist").</summary>
    public string? TeamRole { get; set; }

    /// <summary>Individual scheduled start time for this staff member in the surgery.</summary>
    public DateTime ScheduledAt { get; set; }

    /// <summary>Duration in minutes this staff member is scheduled for.</summary>
    public int DurationMinutes { get; set; } = 30;
}
