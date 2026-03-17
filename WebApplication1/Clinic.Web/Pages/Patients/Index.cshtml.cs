using Clinic.Services.Services.Patients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clinic.Web.Pages.Patients;

[Authorize]
public class IndexModel : PageModel
{
    private static readonly string[] ColumnMap = ["fullName", "phone", "dateOfBirth", "gender", "createdAt"];

    private readonly IPatientService _patientService;

    public IndexModel(IPatientService patientService)
    {
        _patientService = patientService;
    }

    [BindProperty(SupportsGet = true)]
    public string? Search { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnGetPatientListAsync(
        int draw = 1,
        int start = 0,
        int length = 10,
        string? search = null,
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

        var (items, filteredCount, totalCount) = await _patientService.SearchPagedAsync(
            start, length, search, sortBy, ascending, ct);

        return new JsonResult(new
        {
            draw,
            recordsTotal = totalCount,
            recordsFiltered = filteredCount,
            data = items.Select(p => new
            {
                id = p.Id,
                fullName = p.FullName,
                patientCode = $"PT-{p.Id:D5}",
                phone = p.Phone ?? "-",
                dateOfBirth = p.DateOfBirth.ToString("dd/MM/yyyy"),
                gender = p.Gender ?? "-",
                email = p.Email ?? "-",
                isActive = p.IsActive,
                createdAt = p.CreatedAt.ToString("dd/MM/yyyy")
            }).ToArray()
        });
    }
}

