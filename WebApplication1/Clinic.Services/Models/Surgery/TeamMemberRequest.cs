namespace Clinic.Services.Models.Surgery;

public sealed class TeamMemberRequest
{
    public int StaffId { get; set; }
    public string? TeamRole { get; set; }
}
