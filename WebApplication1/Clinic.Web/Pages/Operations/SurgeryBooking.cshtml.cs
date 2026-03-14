using Clinic.Services.Data;
using Clinic.Services.Domain.Entities;
using Clinic.Services.Services.Patients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Web.Pages.Operations;

[Authorize]
public class SurgeryBookingModel : PageModel
{
    private readonly IPatientService _patientService;
    private readonly ClinicDbContext _db;

    public SurgeryBookingModel(IPatientService patientService, ClinicDbContext db)
    {
        _patientService = patientService;
        _db = db;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnGetOperatingRoomsAsync(CancellationToken ct)
    {
        var rooms = await _db.OperatingRooms
            .Where(r => r.IsActive)
            .OrderBy(r => r.Name)
            .Select(r => new { r.Id, r.Name, r.Location })
            .ToListAsync(ct);
        return new JsonResult(rooms);
    }

    public async Task<IActionResult> OnGetRoomSurgeriesAsync(int roomId, string date, CancellationToken ct)
    {
        if (!DateTime.TryParse(date, out var day))
            return new JsonResult(Array.Empty<object>());

        var dayStart = day.Date;
        var dayEnd = dayStart.AddDays(1);

        var surgeries = await _db.SurgerySchedules
            .Where(s => s.OperatingRoomId == roomId
                && s.ScheduledAt >= dayStart
                && s.ScheduledAt < dayEnd
                && s.Status != SurgeryStatus.Cancelled)
            .OrderBy(s => s.ScheduledAt)
            .Select(s => new
            {
                from = s.ScheduledAt.ToString("HH:mm"),
                to = s.ScheduledAt.AddMinutes(s.DurationMinutes).ToString("HH:mm"),
                patientName = s.Patient.FullName,
                reason = s.SurgeryType ?? ""
            })
            .ToListAsync(ct);

        return new JsonResult(surgeries);
    }

    public async Task<IActionResult> OnGetStaffAsync(CancellationToken ct)
    {
        var staff = await _db.Staff
            .Where(s => s.IsActive)
            .OrderBy(s => s.StaffType).ThenBy(s => s.FullName)
            .Select(s => new { s.Id, s.FullName, role = s.StaffType.ToString() })
            .ToListAsync(ct);
        return new JsonResult(staff);
    }

    public async Task<IActionResult> OnGetStaffSurgeriesAsync(int staffId, string date, CancellationToken ct)
    {
        if (!DateTime.TryParse(date, out var day))
            return new JsonResult(Array.Empty<object>());

        var dayStart = day.Date;
        var dayEnd = dayStart.AddDays(1);

        var slots = await _db.SurgeryScheduleStaff
            .Where(ss => ss.StaffId == staffId
                && ss.ScheduledAt >= dayStart
                && ss.ScheduledAt < dayEnd
                && ss.SurgerySchedule.Status != SurgeryStatus.Cancelled)
            .OrderBy(ss => ss.ScheduledAt)
            .Select(ss => new
            {
                from = ss.ScheduledAt.ToString("HH:mm"),
                to = ss.ScheduledAt.AddMinutes(ss.DurationMinutes).ToString("HH:mm"),
                patientName = ss.SurgerySchedule.Patient.FullName,
                reason = ss.SurgerySchedule.SurgeryType ?? ""
            })
            .ToListAsync(ct);

        return new JsonResult(slots);
    }

    public async Task<IActionResult> OnPostBookSurgeryAsync(
        int patientId, int roomId, string date, string timeFrom, string timeTo,
        int[] staffIds, string[] staffFroms, string[] staffTos, string? reason, CancellationToken ct)
    {
        if (patientId <= 0 || roomId <= 0 || staffIds.Length == 0)
            return new JsonResult(new { success = false, message = "Thiếu thông tin bắt buộc." });

        if (!DateTime.TryParse(date, out var day))
            return new JsonResult(new { success = false, message = "Ngày không hợp lệ." });

        var parts = timeFrom.Split(':');
        var scheduledAt = day.Date.AddHours(int.Parse(parts[0])).AddMinutes(int.Parse(parts[1]));

        var startMin = int.Parse(parts[0]) * 60 + int.Parse(parts[1]);
        var toParts = timeTo.Split(':');
        var endMin = int.Parse(toParts[0]) * 60 + int.Parse(toParts[1]);
        var duration = endMin - startMin;
        if (duration <= 0) duration = 30;

        // Create SurgerySchedule with OperatingRoomId FK
        var surgery = new SurgerySchedule
        {
            PatientId = patientId,
            OperatingRoomId = roomId,
            ScheduledAt = scheduledAt,
            DurationMinutes = duration,
            SurgeryType = reason,
            Status = SurgeryStatus.Scheduled,
            CreatedAt = DateTime.Now
        };
        _db.SurgerySchedules.Add(surgery);
        await _db.SaveChangesAsync(ct);

        // Add team members with individual schedules
        for (var i = 0; i < staffIds.Length; i++)
        {
            var staffId = staffIds[i];
            var staff = await _db.Staff.FindAsync([staffId], ct);

            // Parse per-staff time or fall back to room time
            var staffFrom = (staffFroms != null && i < staffFroms.Length) ? staffFroms[i] : timeFrom;
            var staffTo = (staffTos != null && i < staffTos.Length) ? staffTos[i] : timeTo;

            var sfParts = staffFrom.Split(':');
            var staffScheduledAt = day.Date.AddHours(int.Parse(sfParts[0])).AddMinutes(int.Parse(sfParts[1]));
            var sfStartMin = int.Parse(sfParts[0]) * 60 + int.Parse(sfParts[1]);
            var stParts = staffTo.Split(':');
            var sfEndMin = int.Parse(stParts[0]) * 60 + int.Parse(stParts[1]);
            var staffDuration = sfEndMin - sfStartMin;
            if (staffDuration <= 0) staffDuration = 30;

            _db.SurgeryScheduleStaff.Add(new SurgeryScheduleStaff
            {
                SurgeryScheduleId = surgery.Id,
                StaffId = staffId,
                TeamRole = staff?.StaffType.ToString(),
                ScheduledAt = staffScheduledAt,
                DurationMinutes = staffDuration
            });
        }
        await _db.SaveChangesAsync(ct);

        return new JsonResult(new { success = true, surgeryId = surgery.Id });
    }

    /// <summary>Search patients by name/phone (autocomplete).</summary>
    public async Task<IActionResult> OnGetSearchPatientsAsync(string term, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(term))
            return new JsonResult(Array.Empty<object>());

        var (items, _, _) = await _patientService.SearchPagedAsync(0, 10, term, "FullName", true, ct);
        var result = items.Select(p => new
        {
            id = p.Id,
            fullName = p.FullName,
            phone = p.Phone ?? "",
            dateOfBirth = p.DateOfBirth.ToString("dd/MM/yyyy")
        });
        return new JsonResult(result);
    }
}
