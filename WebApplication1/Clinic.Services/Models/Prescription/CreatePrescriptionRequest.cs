namespace Clinic.Services.Models.Prescription;

public sealed class CreatePrescriptionRequest
{
    public int HealthRecordId { get; set; }
    public string MedicineName { get; set; } = string.Empty;
    public string? Dosage { get; set; }
    public string? Frequency { get; set; }
    public string? Duration { get; set; }
    public string? Instructions { get; set; }
}
