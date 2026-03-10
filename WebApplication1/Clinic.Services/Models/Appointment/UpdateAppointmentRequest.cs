namespace Clinic.Services.Models.Appointment;

public class UpdateAppointmentRequest
{
    public DateTime? ScheduledAt { get; set; }
    public int? DurationMinutes { get; set; }
    public string? Notes { get; set; }
    public string? Reason { get; set; }
}
