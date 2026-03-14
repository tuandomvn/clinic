using Clinic.Services.Domain.Entities;
using Clinic.Services.Data;
using Clinic.Services.Services.Patients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Clinic.Web.Pages.Patients;

[Authorize]
public class DetailsModel : PageModel
{
    private readonly IPatientService _patientService;
    private readonly ClinicDbContext _dbContext;
    private readonly ILogger<DetailsModel> _logger;
    private readonly string _uploadPath;

    public DetailsModel(IPatientService patientService, ClinicDbContext dbContext,
        ILogger<DetailsModel> logger, IConfiguration configuration)
    {
        _patientService = patientService;
        _dbContext = dbContext;
        _logger = logger;
        _uploadPath = configuration["FileStorage:UploadPath"]
            ?? throw new InvalidOperationException("FileStorage:UploadPath is not configured.");
    }

    public Patient? Patient { get; set; }
    public Appointment? NextAppointment { get; set; }
    public IReadOnlyList<Activity> Activities { get; set; } = new List<Activity>();

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

            // Build StaffId → (FullName, AvatarPath) dictionary
            var staffMap = await _dbContext.Staff
                .AsNoTracking()
                .Select(s => new { s.Id, s.FullName, s.AvatarPath })
                .ToDictionaryAsync(s => s.Id, s => new { s.FullName, s.AvatarPath }, ct);

            var totalCount = activities.Count;
            var pagedActivities = activities.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            var result = new
            {
                data = pagedActivities.Select(a =>
                {
                    var staff = a.CreatedBy > 0 && staffMap.TryGetValue(a.CreatedBy, out var s) ? s : null;
                    return new
                    {
                        id = a.Id,
                        contentText = a.ContentText,
                        createdBy = staff?.FullName ?? "Hệ thống",
                        avatarUrl = staff?.AvatarPath,
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
                    };
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

    public async Task<IActionResult> OnPostCreateActivityAsync(CancellationToken ct = default)
    {
        var patientIdStr = Request.Form["patientId"].ToString();
        var contentText = Request.Form["contentText"].ToString();

        if (!int.TryParse(patientIdStr, out var patientId) || patientId <= 0
            || string.IsNullOrWhiteSpace(contentText))
        {
            return BadRequest(new { error = "Nội dung không được để trống." });
        }

        try
        {
            // Get current staff info for CreatedBy and avatar
            int createdByStaffId = -1;
            string createdByName = "Hệ thống";
            string? avatarUrl = null;
            var staffIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (staffIdClaim is not null && int.TryParse(staffIdClaim.Value, out var currentStaffId))
            {
                var currentStaff = await _dbContext.Staff
                    .AsNoTracking()
                    .Where(s => s.Id == currentStaffId)
                    .Select(s => new { s.FullName, s.AvatarPath })
                    .FirstOrDefaultAsync(ct);
                if (currentStaff is not null)
                {
                    createdByStaffId = currentStaffId;
                    createdByName = currentStaff.FullName;
                    avatarUrl = currentStaff.AvatarPath;
                }
            }

            var activity = new Activity
            {
                ActivityType = ActivityType.Post,
                ContentText = contentText.Trim(),
                CreatedBy = createdByStaffId,
                PatientId = patientId,
                CreatedDate = DateTime.UtcNow
            };

            // Handle uploaded images
            var files = Request.Form.Files;
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp", ".gif" };
            var activityImagesDir = Path.Combine(_uploadPath, "activities");
            Directory.CreateDirectory(activityImagesDir);

            var savedImages = new List<ActivityImage>();

            foreach (var file in files)
            {
                if (file.Length <= 0 || file.Length > 5 * 1024 * 1024) continue;

                var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(ext)) continue;

                var fileName = $"act-{Guid.NewGuid():N}{ext}";
                var filePath = Path.Combine(activityImagesDir, fileName);

                await using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream, ct);

                var image = new ActivityImage
                {
                    ImageUrl = $"/activities/{fileName}",
                    Caption = file.FileName,
                    CreatedDate = DateTime.UtcNow
                };
                activity.Images.Add(image);
                savedImages.Add(image);
            }

            _dbContext.Activities.Add(activity);
            await _dbContext.SaveChangesAsync(ct);

            var result = new
            {
                id = activity.Id,
                contentText = activity.ContentText,
                createdBy = createdByName,
                avatarUrl,
                activityType = activity.ActivityType.ToString(),
                activityTypeDisplay = GetActivityTypeDisplay(activity.ActivityType),
                createdDate = activity.CreatedDate.ToString("yyyy-MM-dd HH:mm"),
                createdDateRelative = GetRelativeTime(activity.CreatedDate),
                images = savedImages.Select(img => new
                {
                    id = img.Id,
                    imageUrl = img.ImageUrl,
                    caption = img.Caption
                }).ToArray()
            };

            return new JsonResult(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating activity for patient ID: {PatientId}", patientId);
            return StatusCode(500, new { error = "Lỗi khi tạo bài viết." });
        }
    }

    public async Task<IActionResult> OnPostCreateVisitAsync(CancellationToken ct = default)
    {
        if (Id <= 0)
            return BadRequest();

        int staffId = 1; // default
        var staffIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (staffIdClaim is not null && int.TryParse(staffIdClaim.Value, out var sid))
            staffId = sid;

        var record = new HealthRecord
        {
            PatientId = Id,
            StaffId = staffId,
            VisitDate = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.HistoryRecords.Add(record);
        await _dbContext.SaveChangesAsync(ct);

        return RedirectToPage("/Visits/HealthRecord", new { id = record.Id });
    }

    public async Task<IActionResult> OnGetVisitHistoryAsync(int pageNumber = 1, int pageSize = 5, CancellationToken ct = default)
    {
        if (Id <= 0)
            return BadRequest();

        var query = _dbContext.HistoryRecords
            .AsNoTracking()
            .Where(r => r.PatientId == Id)
            .OrderByDescending(r => r.VisitDate);

        var totalCount = await query.CountAsync(ct);

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(r => new
            {
                r.Id,
                visitDate = r.VisitDate.ToString("yyyy-MM-dd HH:mm"),
                staffName = r.Staff.FullName,
                diagnosis = r.Diagnosis ?? "",
                symptoms = r.Symptoms ?? ""
            })
            .ToListAsync(ct);

        return new JsonResult(new
        {
            data = items,
            totalCount,
            pageNumber,
            pageSize,
            totalPages = (int)Math.Ceiling((double)totalCount / pageSize)
        });
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
