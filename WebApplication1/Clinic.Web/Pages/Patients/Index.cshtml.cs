using Clinic.Services.Services.Patients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PatientEntity = Clinic.Services.Domain.Entities.Patient;

namespace Clinic.Web.Pages.Patients;

[Authorize]
public class IndexModel : PageModel
{
    private readonly IPatientService _patientService;

    public IndexModel(IPatientService patientService)
    {
        _patientService = patientService;
    }

    public IReadOnlyList<PatientEntity> Patients { get; private set; } = Array.Empty<PatientEntity>();

    public int CurrentPage { get; private set; }
    public int PageSize { get; private set; } = 10;
    public int TotalCount { get; private set; }
    public int TotalPages { get; private set; }

    public string SortBy { get; private set; } = "name";
    public string SortDir { get; private set; } = "asc";

    public async Task OnGetAsync(int pageNumber = 1, string? sortBy = "name", string? sortDir = "asc", CancellationToken ct = default)
    {
        CurrentPage = pageNumber <= 0 ? 1 : pageNumber;

        SortBy = string.IsNullOrWhiteSpace(sortBy) ? "name" : sortBy.ToLowerInvariant();
        SortDir = string.IsNullOrWhiteSpace(sortDir) ? "asc" : sortDir.ToLowerInvariant();
        var ascending = SortDir != "desc";

        var (items, total) = await _patientService.GetPagedAsync(CurrentPage, PageSize, SortBy, ascending, ct);
        Patients = items;
        TotalCount = total;
        TotalPages = (int)Math.Ceiling(total / (double)PageSize);
    }
}

