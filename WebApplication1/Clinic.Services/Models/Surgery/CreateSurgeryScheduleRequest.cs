namespace Clinic.Services.Models.Surgery;

public sealed class CreateSurgeryScheduleRequest
{
    public int PatientId { get; set; }
    public DateTime ScheduledAt { get; set; }
    public int DurationMinutes { get; set; } = 120;
    public int? OperatingRoomId { get; set; }
    public string? SurgeryType { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }

    public List<TeamMemberRequest> TeamMembers { get; set; } = new();
}
