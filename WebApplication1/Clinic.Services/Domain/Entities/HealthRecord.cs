namespace Clinic.Services.Domain.Entities;

public class HealthRecord
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public int StaffId { get; set; }
    public DateTime VisitDate { get; set; }
    public string? Diagnosis { get; set; }
    public string? Symptoms { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public Patient Patient { get; set; } = null!;
    public Staff Staff { get; set; } = null!;

    public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
}
