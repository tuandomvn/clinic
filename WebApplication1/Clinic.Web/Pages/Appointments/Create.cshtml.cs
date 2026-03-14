using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Clinic.Services.Services.Appointments;
using Clinic.Services.Services.Patients;
using Clinic.Services.Services.Staffs;
using Clinic.Services.Models.Appointment;

namespace Clinic.Web.Pages.Appointments;

[Authorize]
public class CreateModel : PageModel
{
    private readonly IAppointmentService _appointmentService;
    private readonly IPatientService _patientService;
    private readonly IStaffService _staffService;

    public CreateModel(
        IAppointmentService appointmentService,
        IPatientService patientService,
        IStaffService staffService)
    {
        _appointmentService = appointmentService;
        _patientService = patientService;
        _staffService = staffService;
    }

    public void OnGet()
    {
    }

    /// <summary>Search patients by name (autocomplete).</summary>
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

    /// <summary>Get all doctors/staff.</summary>
    public async Task<IActionResult> OnGetStaffListAsync(CancellationToken ct)
    {
        var staff = await _staffService.GetAllAsync(ct);
        var result = staff.Select(s => new
        {
            id = s.Id,
            fullName = s.FullName
        });
        return new JsonResult(result);
    }

    /// <summary>Get available slots for a doctor on a given date.</summary>
    public async Task<IActionResult> OnGetAvailabilityAsync(int staffId, string date, CancellationToken ct)
    {
        if (!DateOnly.TryParse(date, out var dateOnly))
            return BadRequest("Invalid date");

        var availability = await _appointmentService.GetDoctorDayAvailabilityAsync(staffId, dateOnly, ct);
        if (availability == null)
            return NotFound();

        var result = new
        {
            staffId = availability.StaffId,
            staffName = availability.StaffName,
            date = availability.Date.ToString("yyyy-MM-dd"),
            availableSlots = availability.AvailableSlots.Select(s => new
            {
                start = s.Start.ToString("HH:mm"),
                end = s.End.ToString("HH:mm")
            }),
            bookedSlots = availability.BookedSlots.Select(s => new
            {
                start = s.Start.ToString("HH:mm"),
                end = s.End.ToString("HH:mm")
            })
        };
        return new JsonResult(result);
    }

    /// <summary>Create appointment.</summary>
    public async Task<IActionResult> OnPostAsync(
        int patientId,
        int staffId,
        string date,
        string time,
        string? reason,
        CancellationToken ct)
    {
        if (!DateTime.TryParse($"{date} {time}", out var scheduledAt))
            return BadRequest(new { error = "Ngày giờ không hợp lệ." });

        var request = new CreateAppointmentRequest
        {
            PatientId = patientId,
            StaffId = staffId,
            ScheduledAt = scheduledAt,
            DurationMinutes = 30,
            Reason = reason
        };

        var result = await _appointmentService.CreateAsync(request, ct);
        if (result == null)
            return BadRequest(new { error = "Không thể tạo lịch hẹn. Có thể trùng lịch hoặc bệnh nhân/bác sĩ không tồn tại." });

        return new JsonResult(new { success = true, id = result.Id });
    }
}
