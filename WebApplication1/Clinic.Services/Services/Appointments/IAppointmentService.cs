using Clinic.Services.Models.Appointment;

namespace Clinic.Services.Services.Appointments;

public interface IAppointmentService
{
    Task<AppointmentResponse?> CreateAsync(CreateAppointmentRequest request, CancellationToken ct = default);
    Task<AppointmentResponse?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<AppointmentResponse?> UpdateAsync(int id, UpdateAppointmentRequest request, CancellationToken ct = default);
    Task<AppointmentResponse?> CancelAsync(int id, CancelAppointmentRequest? request, CancellationToken ct = default);
    Task<DoctorDayAvailabilityResponse?> GetDoctorDayAvailabilityAsync(int staffId, DateOnly date, CancellationToken ct = default);
}
