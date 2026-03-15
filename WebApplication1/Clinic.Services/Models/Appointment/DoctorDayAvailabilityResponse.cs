namespace Clinic.Services.Models.Appointment;

public class DoctorDayAvailabilityResponse
{
    public int StaffId { get; set; }
    public string? StaffName { get; set; }
    public DateOnly Date { get; set; }
    public IReadOnlyList<TimeSlotDto> BookedSlots { get; set; } = Array.Empty<TimeSlotDto>();
    public IReadOnlyList<TimeSlotDto> AvailableSlots { get; set; } = Array.Empty<TimeSlotDto>();
}

public class TimeSlotDto
{
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public int? AppointmentId { get; set; }

    // Thông tin bệnh nhân cho slot đã book
    public int? PatientId { get; set; }
    public string? PatientName { get; set; }
    public string? Reason { get; set; }
}
