using Microsoft.EntityFrameworkCore;
using Clinic.Services.Data;
using Clinic.Services.Domain.Entities;
using Clinic.Services.Models.Surgery;

namespace Clinic.Services.Services.Surgeries;

public sealed class SurgeryScheduleService : ISurgeryScheduleService
{
    private readonly ClinicDbContext _db;

    public SurgeryScheduleService(ClinicDbContext db)
    {
        _db = db;
    }

    public async Task<(SurgeryScheduleResponse? Result, string? Error)> CreateAsync(CreateSurgeryScheduleRequest request, CancellationToken ct = default)
    {
        if (request.DurationMinutes <= 0)
            return (null, "DurationMinutes must be > 0.");

        if (request.TeamMembers == null || request.TeamMembers.Count == 0)
            return (null, "TeamMembers is required.");

        var distinctStaffIds = request.TeamMembers.Select(x => x.StaffId).Where(x => x > 0).Distinct().ToList();
        if (distinctStaffIds.Count != request.TeamMembers.Count)
            return (null, "TeamMembers contains duplicate or invalid StaffId.");

        var patientExists = await _db.Patients.AnyAsync(p => p.Id == request.PatientId, ct);
        if (!patientExists)
            return (null, "Patient not found.");

        var staff = await _db.Staff
            .AsNoTracking()
            .Where(s => distinctStaffIds.Contains(s.Id) && s.IsActive)
            .Select(s => new { s.Id, s.FullName })
            .ToListAsync(ct);

        if (staff.Count != distinctStaffIds.Count)
            return (null, "One or more staff not found or inactive.");

        var start = request.ScheduledAt;
        var end = request.ScheduledAt.AddMinutes(request.DurationMinutes);

        var conflicts = await GetConflictingStaffIdsAsync(distinctStaffIds, start, end, ct);
        if (conflicts.Count > 0)
            return (null, "One or more staff are not available in this time range.");

        var surgery = new SurgerySchedule
        {
            PatientId = request.PatientId,
            ScheduledAt = request.ScheduledAt,
            DurationMinutes = request.DurationMinutes,
            Room = request.Room,
            SurgeryType = request.SurgeryType,
            Description = request.Description,
            Notes = request.Notes,
            Status = SurgeryStatus.Scheduled
        };

        foreach (var tm in request.TeamMembers)
        {
            surgery.TeamMembers.Add(new SurgeryScheduleStaff
            {
                StaffId = tm.StaffId,
                TeamRole = string.IsNullOrWhiteSpace(tm.TeamRole) ? null : tm.TeamRole.Trim()
            });
        }

        _db.SurgerySchedules.Add(surgery);
        await _db.SaveChangesAsync(ct);

        var created = await GetResponseAsync(surgery.Id, ct);
        return (created, null);
    }

    public async Task<SurgeryScheduleResponse?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await GetResponseAsync(id, ct);
    }

    public async Task<IReadOnlyList<SurgeryScheduleResponse>> GetByDateAsync(DateOnly date, CancellationToken ct = default)
    {
        var dayStartUtc = date.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
        var dayEndUtc = dayStartUtc.AddDays(1);

        var items = await _db.SurgerySchedules
            .AsNoTracking()
            .Where(x => x.ScheduledAt >= dayStartUtc && x.ScheduledAt < dayEndUtc)
            .Include(x => x.Patient)
            .Include(x => x.TeamMembers)
                .ThenInclude(tm => tm.Staff)
            .OrderBy(x => x.ScheduledAt)
            .ToListAsync(ct);

        return items.Select(s => MapToResponse(s)).ToList();
    }

    private async Task<SurgeryScheduleResponse?> GetResponseAsync(int id, CancellationToken ct)
    {
        var s = await _db.SurgerySchedules
            .AsNoTracking()
            .Include(x => x.Patient)
            .Include(x => x.TeamMembers)
                .ThenInclude(tm => tm.Staff)
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        return s == null ? null : MapToResponse(s);
    }

    private static SurgeryScheduleResponse MapToResponse(SurgerySchedule s) => new()
    {
        Id = s.Id,
        PatientId = s.PatientId,
        PatientName = s.Patient?.FullName,
        ScheduledAt = s.ScheduledAt,
        DurationMinutes = s.DurationMinutes,
        Room = s.Room,
        SurgeryType = s.SurgeryType,
        Description = s.Description,
        Notes = s.Notes,
        TeamMembers = s.TeamMembers
            .OrderBy(x => x.StaffId)
            .Select(x => new TeamMemberResponse
            {
                StaffId = x.StaffId,
                StaffName = x.Staff?.FullName,
                TeamRole = x.TeamRole
            })
            .ToList()
    };

    private async Task<List<int>> GetConflictingStaffIdsAsync(List<int> staffIds, DateTime start, DateTime end, CancellationToken ct)
    {
        var appointmentConflicts = await _db.Appointments
            .AsNoTracking()
            .Where(a => staffIds.Contains(a.StaffId)
                && a.Status != AppointmentStatus.Cancelled
                && a.ScheduledAt < end
                && a.ScheduledAt.AddMinutes(a.DurationMinutes) > start)
            .Select(a => a.StaffId)
            .Distinct()
            .ToListAsync(ct);

        var surgeryConflicts = await _db.SurgeryScheduleStaff
            .AsNoTracking()
            .Where(tm => staffIds.Contains(tm.StaffId)
                && tm.SurgerySchedule.Status != SurgeryStatus.Cancelled
                && tm.SurgerySchedule.ScheduledAt < end
                && tm.SurgerySchedule.ScheduledAt.AddMinutes(tm.SurgerySchedule.DurationMinutes) > start)
            .Select(tm => tm.StaffId)
            .Distinct()
            .ToListAsync(ct);

        return appointmentConflicts.Union(surgeryConflicts).Distinct().ToList();
    }
}
