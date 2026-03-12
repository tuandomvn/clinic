using Clinic.Services.Domain.Entities;
using Clinic.Services.Services.Patients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clinic.Web.Pages.Patients;

[Authorize]
public class DetailsModel : PageModel
{
    private readonly IPatientService _patientService;
    private readonly ILogger<DetailsModel> _logger;

    public DetailsModel(IPatientService patientService, ILogger<DetailsModel> logger)
    {
        _patientService = patientService;
        _logger = logger;
    }

    public Patient? Patient { get; set; }
    public Appointment? NextAppointment { get; set; }
    public IReadOnlyList<Activity> Activities { get; set; } = new List<Activity>();
    public IReadOnlyList<Appointment> VisitHistory { get; set; } = new List<Appointment>();

    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    public async Task<IActionResult> OnGetAsync(CancellationToken ct = default)
    {
        if (Id <= 0)
        {
            _logger.LogWarning("Invalid patient ID: {Id}", Id);
            return RedirectToPage("/Patients/Index");
        }

        try
        {
            Patient = await _patientService.GetByIdWithDetailsAsync(Id, ct);

            if (Patient == null)
            {
                _logger.LogWarning("Patient not found with ID: {Id}", Id);
                return NotFound();
            }

            // Lấy lịch hẹn kế tiếp (chưa diễn ra)
            var now = DateTime.UtcNow;
            NextAppointment = Patient.Appointments
                .Where(a => a.ScheduledAt > now && 
                            (a.Status == AppointmentStatus.Scheduled || 
                             a.Status == AppointmentStatus.CheckedIn))
                .OrderBy(a => a.ScheduledAt)
                .FirstOrDefault();

            // Lấy lịch sử khám (Appointments đã hoàn thành hoặc đang tiến hành)
            VisitHistory = Patient.Appointments
                .Where(a => a.Status == AppointmentStatus.Completed || 
                            a.Status == AppointmentStatus.InProgress)
                .OrderByDescending(a => a.ScheduledAt)
                .ToList();

            // Lấy Activities theo loại
            Activities = Patient.Activities
                .OrderByDescending(a => a.CreatedAt)
                .ToList();

            _logger.LogInformation("Patient details loaded successfully for ID: {Id}", Id);
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading patient details for ID: {Id}", Id);
            return StatusCode(500, "An error occurred while loading patient details");
        }
    }

    public string GetActivityTypeDisplay(ActivityType type)
    {
        return type switch
        {
            ActivityType.AppointmentCreated => "Tạo lịch hẹn",
            ActivityType.AppointmentCompleted => "Hoàn thành khám",
            ActivityType.HistoryRecordAdded => "Thêm kết quả khám",
            ActivityType.SurgeryScheduled => "Sắp xếp phẫu thuật",
            ActivityType.PatientRegistered => "Đăng ký bệnh nhân",
            ActivityType.General => "Khác",
            _ => "Không xác định"
        };
    }

    public string GetVisitTypeDisplay(Appointment appointment)
    {
        if (appointment.Status == AppointmentStatus.Completed || 
            appointment.Status == AppointmentStatus.InProgress)
        {
            return "Khám bệnh";
        }
        return "Khác";
    }

    public async Task<IActionResult> OnGetActivitiesAsync(int draw = 1, int start = 0, int length = 5, CancellationToken ct = default)
    {
        if (Id <= 0)
        {
            return BadRequest("Invalid patient ID");
        }

        try
        {
            // Load patient with details to ensure Activities are populated
            var patient = await _patientService.GetByIdWithDetailsAsync(Id, ct);
            if (patient == null)
            {
                return NotFound();
            }

            var activities = patient.Activities
                .OrderByDescending(a => a.CreatedAt)
                .ToList();

            var totalCount = activities.Count;
            var pagedActivities = activities.Skip(start).Take(length).ToList();

            var result = new
            {
                draw,
                recordsTotal = totalCount,
                recordsFiltered = totalCount,
                data = pagedActivities.Select(a => new
                {
                    description = a.Description,
                    staffName = a.Staff?.FullName ?? "-",
                    createdAt = a.CreatedAt.ToString("MM-dd HH:mm")
                }).ToArray()
            };

            return new JsonResult(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading activities for patient ID: {Id}", Id);
            return StatusCode(500, new { error = "An error occurred while loading activities" });
        }
    }
}
