using Clinic.Services.Services.Staffs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Clinic.Web.Pages.Staff;

[Authorize]
public class IndexModel : PageModel
{
    private static readonly string[] ColumnMap = ["fullName", "specialization", "staffType", "phone", "isActive"];

    private readonly IStaffService _staffService;

    public IndexModel(IStaffService staffService)
    {
        _staffService = staffService;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnGetStaffListAsync(
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

        var (items, filteredCount, totalCount) = await _staffService.SearchPagedAsync(
            start, length, search, sortBy, ascending, ct);

        return new JsonResult(new
        {
            draw,
            recordsTotal = totalCount,
            recordsFiltered = filteredCount,
            data = items.Select(s => new
            {
                id = s.Id,
                fullName = s.FullName,
                email = s.Email ?? "-",
                specialization = s.Specialization ?? "-",
                staffType = s.StaffType.ToString(),
                phone = s.Phone ?? "-",
                isActive = s.IsActive,
                hasAccount = s.UserAccount is not null
            }).ToArray()
        });
    }
}

