using StaffEntity = Clinic.Services.Domain.Entities.Staff;
using Clinic.Services.Services.Staffs;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clinic.Web.Pages.Staff;

public class IndexModel : PageModel
{
    private readonly IStaffService _staffService;

    public IndexModel(IStaffService staffService)
    {
        _staffService = staffService;
    }

    public IReadOnlyList<StaffEntity> Staffs { get; private set; } = Array.Empty<StaffEntity>();

    public async Task OnGetAsync(CancellationToken ct)
    {
        Staffs = await _staffService.GetAllAsync(ct);
    }
}

