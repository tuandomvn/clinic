using Clinic.Services.Domain.Entities;
using Clinic.Services.Services.Patients;
using Clinic.Services.Services.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Clinic.Web.Pages.Tasks;

[Authorize]
public class CreateModel : PageModel
{
    private readonly IReminderTaskService _taskService;
    private readonly IPatientService _patientService;

    public CreateModel(IReminderTaskService taskService, IPatientService patientService)
    {
        _taskService = taskService;
        _patientService = patientService;
    }

    public void OnGet()
    {
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

    /// <summary>Create a new reminder task.</summary>
    public async Task<IActionResult> OnPostAsync(
        int patientId,
        string taskType,
        string dueDate,
        string? description,
        CancellationToken ct)
    {
        if (patientId <= 0)
            return BadRequest(new { error = "Bệnh nhân không hợp lệ." });

        if (!Enum.TryParse<ReminderTaskType>(taskType, out var type))
            return BadRequest(new { error = "Loại task không hợp lệ." });


        if (!DateTime.TryParse(dueDate, out var due))
            return BadRequest(new { error = "Ngày hẹn không hợp lệ." });

        int createdBy = -1;
        var staffIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (staffIdClaim is not null && int.TryParse(staffIdClaim.Value, out var sid))
            createdBy = sid;

        var task = new ReminderTask
        {
            PatientId = patientId,
            TaskType = type,
            DueDate = due,
            Description = description ?? string.Empty,
            IsDone = false,
            CreatedBy = createdBy,
            CreatedAt = DateTime.Now
        };

        var created = await _taskService.CreateAsync(task, ct);
        return new JsonResult(new { success = true, id = created.Id });
    }
}
