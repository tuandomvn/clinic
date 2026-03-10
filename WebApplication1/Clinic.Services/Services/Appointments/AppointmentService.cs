using Microsoft.EntityFrameworkCore;
using Clinic.Services.Data;
using Clinic.Services.Domain.Entities;
using Clinic.Services.Models.Appointment;

namespace Clinic.Services.Services.Appointments;

public sealed class AppointmentService : IAppointmentService
{
    private const int DefaultSlotDurationMinutes = 30;
    private static readonly TimeOnly WorkDayStart = new(8, 0);
    private static readonly TimeOnly WorkDayEnd = new(17, 0);

    private readonly ClinicDbContext _db;

    public AppointmentService(ClinicDbContext db)
    {
        _db = db;
    }

    public async Task<AppointmentResponse?> CreateAsync(CreateAppointmentRequest request, CancellationToken ct = default)
    {
        var patientExists = await _db.Patients.AnyAsync(p => p.Id == request.PatientId, ct);
        if (!patientExists) return null;

        var staffExists = await _db.Staff.AnyAsync(s => s.Id == request.StaffId, ct);
        if (!staffExists) return null;

        var conflict = await HasDoctorTimeConflict(request.StaffId, request.ScheduledAt, request.DurationMinutes, null, ct);
        if (conflict) return null;

        var appointment = new Appointment
        {
            PatientId = request.PatientId,
            StaffId = request.StaffId,
            ScheduledAt = request.ScheduledAt,
            DurationMinutes = request.DurationMinutes,
            Notes = request.Notes,
            Reason = request.Reason,
            Status = AppointmentStatus.Scheduled
        };
        _db.Appointments.Add(appointment);
        await _db.SaveChangesAsync(ct);

        return await GetAppointmentResponseAsync(appointment.Id, ct);
    }

    public async Task<AppointmentResponse?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await GetAppointmentResponseAsync(id, ct);
    }

    public async Task<AppointmentResponse?> UpdateAsync(int id, UpdateAppointmentRequest request, CancellationToken ct = default)
    {
        var appointment = await _db.Appointments.FindAsync([id], ct);
        if (appointment == null) return null;

        if (appointment.Status == AppointmentStatus.Cancelled) return null;

        var newStart = request.ScheduledAt ?? appointment.ScheduledAt;
        var newDuration = request.DurationMinutes ?? appointment.DurationMinutes;
        var conflict = await HasDoctorTimeConflict(appointment.StaffId, newStart, newDuration, id, ct);
        if (conflict) return null;

        if (request.ScheduledAt.HasValue) appointment.ScheduledAt = request.ScheduledAt.Value;
        if (request.DurationMinutes.HasValue) appointment.DurationMinutes = request.DurationMinutes.Value;
        if (request.Notes != null) appointment.Notes = request.Notes;
        if (request.Reason != null) appointment.Reason = request.Reason;

        appointment.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);

        return await GetAppointmentResponseAsync(appointment.Id, ct);
    }

    public async Task<AppointmentResponse?> CancelAsync(int id, CancelAppointmentRequest? request, CancellationToken ct = default)
    {
        var appointment = await _db.Appointments.FindAsync([id], ct);
        if (appointment == null) return null;

        if (appointment.Status == AppointmentStatus.Cancelled) return null;

        appointment.Status = AppointmentStatus.Cancelled;
        if (!string.IsNullOrWhiteSpace(request?.Reason))
            appointment.Notes = (appointment.Notes ?? "") + (appointment.Notes != null ? " " : "") + "[Cancelled: " + request.Reason + "]";
        appointment.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);

        return await GetAppointmentResponseAsync(appointment.Id, ct);
    }

    public async Task<DoctorDayAvailabilityResponse?> GetDoctorDayAvailabilityAsync(int staffId, DateOnly date, CancellationToken ct = default)
    {
        var staff = await _db.Staff.AsNoTracking().FirstOrDefaultAsync(s => s.Id == staffId, ct);
        if (staff == null) return null;

        var dayStartUtc = date.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
        var workStart = date.ToDateTime(WorkDayStart, DateTimeKind.Utc);
        var workEnd = date.ToDateTime(WorkDayEnd, DateTimeKind.Utc);

        var booked = await _db.Appointments
            .AsNoTracking()
            .Where(a => a.StaffId == staffId && a.Status != AppointmentStatus.Cancelled
                && a.ScheduledAt < workEnd
                && a.ScheduledAt.AddMinutes(a.DurationMinutes) > workStart)
            .OrderBy(a => a.ScheduledAt)
            .Select(a => new { a.Id, a.ScheduledAt, a.DurationMinutes })
            .ToListAsync(ct);

        var bookedSlots = booked
            .Select(a => new TimeSlotDto
            {
                Start = a.ScheduledAt,
                End = a.ScheduledAt.AddMinutes(a.DurationMinutes),
                AppointmentId = a.Id
            })
            .ToList();

        var availableSlots = GetAvailableSlots(workStart, workEnd, bookedSlots, DefaultSlotDurationMinutes);

        return new DoctorDayAvailabilityResponse
        {
            StaffId = staffId,
            StaffName = staff.FullName,
            Date = date,
            BookedSlots = bookedSlots,
            AvailableSlots = availableSlots
        };
    }

    public async Task<IReadOnlyList<AppointmentResponse>> GetByDateAsync(DateOnly date, CancellationToken ct = default)
    {
        var dayStartUtc = date.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
        var dayEndUtc = dayStartUtc.AddDays(1);

        var items = await _db.Appointments
            .AsNoTracking()
            .Include(a => a.Patient)
            .Include(a => a.Staff)
            //.Where(a => a.ScheduledAt >= dayStartUtc && a.ScheduledAt < dayEndUtc) TODO
            .OrderBy(a => a.ScheduledAt)
            .ToListAsync(ct);

        return items.Select(a => new AppointmentResponse
        {
            Id = a.Id,
            PatientId = a.PatientId,
            StaffId = a.StaffId,
            PatientName = a.Patient?.FullName,
            StaffName = a.Staff?.FullName,
            ScheduledAt = a.ScheduledAt,
            DurationMinutes = a.DurationMinutes,
            Status = a.Status,
            Notes = a.Notes,
            Reason = a.Reason,
            CreatedAt = a.CreatedAt,
            UpdatedAt = a.UpdatedAt
        }).ToList();
    }

    private async Task<bool> HasDoctorTimeConflict(int staffId, DateTime start, int durationMinutes, int? excludeAppointmentId, CancellationToken ct)
    {
        var end = start.AddMinutes(durationMinutes);
        var query = _db.Appointments
            .Where(a => a.StaffId == staffId && a.Status != AppointmentStatus.Cancelled
                && a.ScheduledAt < end
                && a.ScheduledAt.AddMinutes(a.DurationMinutes) > start);
        if (excludeAppointmentId.HasValue)
            query = query.Where(a => a.Id != excludeAppointmentId.Value);
        return await query.AnyAsync(ct);
    }

    private static List<TimeSlotDto> GetAvailableSlots(DateTime workStart, DateTime workEnd, List<TimeSlotDto> bookedSlots, int slotDurationMinutes)
    {
        var available = new List<TimeSlotDto>();
        var slotStart = workStart;
        while (slotStart.AddMinutes(slotDurationMinutes) <= workEnd)
        {
            var slotEnd = slotStart.AddMinutes(slotDurationMinutes);
            var overlaps = bookedSlots.Any(b => b.Start < slotEnd && b.End > slotStart);
            if (!overlaps)
                available.Add(new TimeSlotDto { Start = slotStart, End = slotEnd });
            slotStart = slotEnd;
        }
        return available;
    }

    private async Task<AppointmentResponse?> GetAppointmentResponseAsync(int id, CancellationToken ct)
    {
        var a = await _db.Appointments
            .AsNoTracking()
            .Include(x => x.Patient)
            .Include(x => x.Staff)
            .FirstOrDefaultAsync(x => x.Id == id, ct);
        if (a == null) return null;
        return new AppointmentResponse
        {
            Id = a.Id,
            PatientId = a.PatientId,
            StaffId = a.StaffId,
            PatientName = a.Patient?.FullName,
            StaffName = a.Staff?.FullName,
            ScheduledAt = a.ScheduledAt,
            DurationMinutes = a.DurationMinutes,
            Status = a.Status,
            Notes = a.Notes,
            Reason = a.Reason,
            CreatedAt = a.CreatedAt,
            UpdatedAt = a.UpdatedAt
        };
    }
}
