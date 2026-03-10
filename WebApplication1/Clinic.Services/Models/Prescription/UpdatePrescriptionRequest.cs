namespace Clinic.Services.Models.Prescription;

public sealed class UpdatePrescriptionRequest
{
    public string? MedicineName { get; set; }
    public string? Dosage { get; set; }
    public string? Frequency { get; set; }
    public string? Duration { get; set; }
    public string? Instructions { get; set; }
    public bool? IsActive { get; set; }
}
