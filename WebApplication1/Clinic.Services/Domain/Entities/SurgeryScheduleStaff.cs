namespace Clinic.Services.Domain.Entities;

public class SurgeryScheduleStaff
{
    public int SurgeryScheduleId { get; set; }
    public SurgerySchedule SurgerySchedule { get; set; } = null!;

    public int StaffId { get; set; }
    public Staff Staff { get; set; } = null!;

    /// <summary>Role of staff in the surgery team (e.g. "Surgeon", "Assistant", "Anesthetist").</summary>
    public string? TeamRole { get; set; }
}
