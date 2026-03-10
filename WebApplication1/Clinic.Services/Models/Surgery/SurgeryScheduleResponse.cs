namespace Clinic.Services.Models.Surgery;

public sealed class SurgeryScheduleResponse
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public string? PatientName { get; set; }

    public DateTime ScheduledAt { get; set; }
    public int DurationMinutes { get; set; }
    public string? Room { get; set; }
    public string? SurgeryType { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }

    public List<TeamMemberResponse> TeamMembers { get; set; } = new();
}

public sealed class TeamMemberResponse
{
    public int StaffId { get; set; }
    public string? StaffName { get; set; }
    public string? TeamRole { get; set; }
}
