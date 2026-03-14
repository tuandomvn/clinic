using Clinic.Services.Services.Patients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clinic.Web.Pages.Operations;

[Authorize]
public class SurgeryBookingModel : PageModel
{
    private readonly IPatientService _patientService;

    public SurgeryBookingModel(IPatientService patientService)
    {
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
}
