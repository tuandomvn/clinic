using Clinic.Services.Domain.Entities;

namespace Clinic.Services.Models.Appointment;

public class AppointmentResponse
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public int StaffId { get; set; }
    public string? PatientName { get; set; }
    public string? StaffName { get; set; }
    public DateTime ScheduledAt { get; set; }
    public int DurationMinutes { get; set; }
    public AppointmentStatus Status { get; set; }
    public string? Notes { get; set; }
    public string? Reason { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
