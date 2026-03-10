namespace Clinic.Services.Domain.Entities;

public class Prescription
{
    public int Id { get; set; }

    public int HealthRecordId { get; set; }
    public HealthRecord HealthRecord { get; set; } = null!;

    public string MedicineName { get; set; } = string.Empty;
    public string? Dosage { get; set; }
    public string? Frequency { get; set; }
    public string? Duration { get; set; }
    public string? Instructions { get; set; }

    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
