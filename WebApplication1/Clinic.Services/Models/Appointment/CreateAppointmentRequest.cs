namespace Clinic.Services.Models.Appointment;

public class CreateAppointmentRequest
{
    public int PatientId { get; set; }
    public int StaffId { get; set; }
    public DateTime ScheduledAt { get; set; }
    public int DurationMinutes { get; set; } = 30;
    public string? Notes { get; set; }
    public string? Reason { get; set; }
}
