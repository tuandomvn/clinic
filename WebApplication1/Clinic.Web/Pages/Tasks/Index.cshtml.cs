using Clinic.Services.Domain.Entities;
using Clinic.Services.Data;
using Clinic.Services.Services.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Clinic.Web.Pages.Tasks;

[Authorize]
public class IndexModel : PageModel
{
    private static readonly string[] ColumnMap = ["", "patientName", "taskType", "description", "dueDate", "priority"];

    private readonly IReminderTaskService _taskService;
    private readonly ClinicDbContext _dbContext;

    public IndexModel(IReminderTaskService taskService, ClinicDbContext dbContext)
    {
        _taskService = taskService;
        _dbContext = dbContext;
    }

    public int TodayPendingCount { get; set; }

    public async Task OnGetAsync(CancellationToken ct)
    {
        TodayPendingCount = await _dbContext.ReminderTasks
            .CountAsync(t => t.DueDate.Date == DateTime.UtcNow.Date && !t.IsDone, ct);
    }

    public async Task<IActionResult> OnGetTaskListAsync(
        int draw = 1,
        int start = 0,
        int length = 10,
        string? search = null,
        string? filterType = null,
        string? filterStatus = null,
        CancellationToken ct = default)
    {
        var orderColumn = HttpContext.Request.Query["order[0][column]"].ToString();
        var orderDir = HttpContext.Request.Query["order[0][dir]"].ToString();
        bool ascending = !orderDir.Equals("desc", StringComparison.OrdinalIgnoreCase);

        string? sortBy = null;
        if (int.TryParse(orderColumn, out int colIndex) && colIndex >= 0 && colIndex < ColumnMap.Length)
        {
            sortBy = ColumnMap[colIndex];
        }

        var (items, filteredCount, totalCount) = await _taskService.SearchPagedAsync(
            start, length, search, filterType, filterStatus, sortBy, ascending, ct);

        // Build StaffId → FullName map for DoneBy display
        var staffIds = items
            .Where(t => t.DoneByStaffId.HasValue && t.DoneByStaffId.Value > 0)
            .Select(t => t.DoneByStaffId!.Value)
            .Distinct()
            .ToList();
        var staffMap = staffIds.Count > 0
            ? await _dbContext.Staff.AsNoTracking()
                .Where(s => staffIds.Contains(s.Id))
                .ToDictionaryAsync(s => s.Id, s => s.FullName, ct)
            : new Dictionary<int, string>();

        var today = DateTime.UtcNow.Date;

        return new JsonResult(new
        {
            draw,
            recordsTotal = totalCount,
            recordsFiltered = filteredCount,
            data = items.Select(t => new
            {
                id = t.Id,
                patientId = t.PatientId,
                patientName = t.Patient.FullName,
                patientPhone = t.Patient.Phone ?? "-",
                taskType = t.TaskType.ToString(),
                taskTypeDisplay = GetTaskTypeDisplay(t.TaskType),
                description = t.Description,
                dueDate = t.DueDate.ToString("dd/MM/yyyy"),
                isOverdue = t.DueDate.Date <= today && !t.IsDone,
                isToday = t.DueDate.Date == today,
                priority = t.Priority.ToString(),
                priorityDisplay = t.Priority switch
                {
                    TaskPriority.High => "Cao",
                    TaskPriority.Medium => "TB",
                    _ => "Thấp"
                },
                isDone = t.IsDone,
                doneBy = t.DoneByStaffId.HasValue && t.DoneByStaffId.Value > 0
                    ? staffMap.GetValueOrDefault(t.DoneByStaffId.Value, "—")
                    : "—"
            }).ToArray()
        });
    }

    public async Task<IActionResult> OnPostMarkDoneAsync([FromBody] MarkDoneRequest request, CancellationToken ct = default)
    {
        if (request.Ids is not { Length: > 0 })
            return BadRequest(new { error = "Chưa chọn task nào." });

        var ids = request.Ids.ToList();
        var tasks = await _dbContext.ReminderTasks
            .Where(t => ids.Contains(t.Id) && !t.IsDone)
            .ToListAsync(ct);

        if (tasks.Count == 0)
            return new JsonResult(new { success = true, updated = 0 });

        int? staffId = null;
        var staffIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (staffIdClaim is not null && int.TryParse(staffIdClaim.Value, out var sid))
            staffId = sid;

        var now = DateTime.UtcNow;
        foreach (var task in tasks)
        {
            task.IsDone = true;
            task.DoneAt = now;
            task.DoneByStaffId = staffId;
        }

        await _dbContext.SaveChangesAsync(ct);

        return new JsonResult(new { success = true, updated = tasks.Count });
    }

    public record MarkDoneRequest(int[] Ids);

    private static string GetTaskTypeDisplay(ReminderTaskType type) => type switch
    {
        ReminderTaskType.FollowUp => "Follow-up sau mổ",
        ReminderTaskType.BirthdayGreeting => "Chúc mừng sinh nhật",
        ReminderTaskType.VaccinationReminder => "Nhắc lịch tiêm chủng",
        ReminderTaskType.PeriodicTest => "Xét nghiệm định kỳ",
        ReminderTaskType.General => "Khác",
        _ => "Không xác định"
    };
}
