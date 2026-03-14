namespace Clinic.Services.Domain.Entities;

public class OperatingRoom
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Location { get; set; }
    public bool IsActive { get; set; } = true;
}
