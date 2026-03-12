using Clinic.Services.Domain.Entities;
using Clinic.Services.Data;
using Clinic.Services.Services.Patients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clinic.Web.Pages.Patients;

[Authorize]
public class DetailsModel : PageModel
{
    private readonly IPatientService _patientService;
    private readonly ClinicDbContext _dbContext;
    private readonly ILogger<DetailsModel> _logger;

    public DetailsModel(IPatientService patientService, ClinicDbContext dbContext, ILogger<DetailsModel> logger)
    {
        _patientService = patientService;
        _dbContext = dbContext;
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
                .Where(a => //a.ScheduledAt > now && TODO
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
                .OrderByDescending(a => a.CreatedDate)
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
            ActivityType.Post => "Bài viết",
            ActivityType.BirthdayGreeting => "Chúc mừng sinh nhật",
            ActivityType.AppointmentCreated => "Lịch hẹn",
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

    public async Task<IActionResult> OnGetActivitiesAsync(int pageNumber = 1, int pageSize = 10, CancellationToken ct = default)
    {
        if (Id <= 0)
        {
            return BadRequest("Invalid patient ID");
        }

        try
        {
            var patient = await _patientService.GetByIdWithDetailsAsync(Id, ct);
            if (patient == null)
            {
                return NotFound();
            }

            var activities = patient.Activities
                .OrderByDescending(a => a.CreatedDate)
                .ToList();

            var totalCount = activities.Count;
            var pagedActivities = activities.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            var result = new
            {
                data = pagedActivities.Select(a => new
                {
                    id = a.Id,
                    contentText = a.ContentText,
                    createdBy = a.CreatedBy ?? "system",
                    activityType = a.ActivityType.ToString(),
                    activityTypeDisplay = GetActivityTypeDisplay(a.ActivityType),
                    createdDate = a.CreatedDate.ToString("yyyy-MM-dd HH:mm"),
                    createdDateRelative = GetRelativeTime(a.CreatedDate),
                    images = (a.Images ?? []).Select(img => new
                    {
                        id = img.Id,
                        imageUrl = img.ImageUrl,
                        caption = img.Caption
                    }).ToArray()
                }).ToArray(),
                hasMore = (pageNumber * pageSize) < totalCount,
                totalCount,
                pageNumber
            };

            return new JsonResult(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading activities for patient ID: {Id}", Id);
            return StatusCode(500, new { error = "An error occurred while loading activities" });
        }
    }

    public async Task<IActionResult> OnPostCreateActivityAsync([FromBody] CreateActivityRequest request, CancellationToken ct = default)
    {
        if (request.PatientId <= 0 || string.IsNullOrWhiteSpace(request.ContentText))
        {
            return BadRequest(new { error = "Nội dung không được để trống." });
        }

        try
        {
            var activity = new Activity
            {
                ActivityType = ActivityType.Post,
                ContentText = request.ContentText.Trim(),
                CreatedBy = User.Identity?.Name ?? "Unknown",
                PatientId = request.PatientId,
                CreatedDate = DateTime.UtcNow
            };

            _dbContext.Activities.Add(activity);
            await _dbContext.SaveChangesAsync(ct);

            var result = new
            {
                id = activity.Id,
                contentText = activity.ContentText,
                createdBy = activity.CreatedBy,
                activityType = activity.ActivityType.ToString(),
                activityTypeDisplay = GetActivityTypeDisplay(activity.ActivityType),
                createdDate = activity.CreatedDate.ToString("yyyy-MM-dd HH:mm"),
                createdDateRelative = GetRelativeTime(activity.CreatedDate),
                images = Array.Empty<object>()
            };

            return new JsonResult(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating activity for patient ID: {PatientId}", request.PatientId);
            return StatusCode(500, new { error = "Lỗi khi tạo bài viết." });
        }
    }

    public class CreateActivityRequest
    {
        public int PatientId { get; set; }
        public string ContentText { get; set; } = string.Empty;
    }

    private static string GetRelativeTime(DateTime date)
    {
        var span = DateTime.UtcNow - date;
        if (span.TotalMinutes < 1) return "Vừa xong";
        if (span.TotalMinutes < 60) return $"{(int)span.TotalMinutes} phút trước";
        if (span.TotalHours < 24) return $"{(int)span.TotalHours} giờ trước";
        if (span.TotalDays < 30) return $"{(int)span.TotalDays} ngày trước";
        if (span.TotalDays < 365) return $"{(int)(span.TotalDays / 30)} tháng trước";
        return $"{(int)(span.TotalDays / 365)} năm trước";
    }
}
