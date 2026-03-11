namespace Clinic.Services.Domain.Entities;

public enum BarcodeType
{
    QRCode = 0,
    Code128 = 1,
    EAN13 = 2,
    UPC = 3
}

public class Patient
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? IdentityNumber { get; set; }
    public string? InsuranceNumber { get; set; }
    public string? BarcodeValue { get; set; }
    public BarcodeType BarcodeType { get; set; } = BarcodeType.Code128;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public ICollection<HealthRecord> HistoryRecords { get; set; } = new List<HealthRecord>();
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public ICollection<SurgerySchedule> SurgerySchedules { get; set; } = new List<SurgerySchedule>();
    public ICollection<Activity> Activities { get; set; } = new List<Activity>();
}
