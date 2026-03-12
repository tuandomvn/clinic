using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Clinic.Services.Services.Staffs;
using Clinic.Services.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using StaffEntity = Clinic.Services.Domain.Entities.Staff;

namespace Clinic.Web.Pages.Auth;

[Authorize]
public class ProfileModel : PageModel
{
    private readonly IStaffService _staffService;

    public ProfileModel(IStaffService staffService)
    {
        _staffService = staffService;
    }

    [BindProperty]
    public ProfileInput Input { get; set; } = new();

    public string? SuccessMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(CancellationToken ct)
    {
        var staffId = GetCurrentStaffId();
        if (staffId is null) return RedirectToPage("/Auth/Login");

        var staff = await _staffService.GetByIdAsync(staffId.Value, ct);
        if (staff is null) return RedirectToPage("/Index");

        Input = new ProfileInput
        {
            FullName = staff.FullName,
            Email = staff.Email,
            Phone = staff.Phone,
            Specialization = staff.Specialization,
            StaffType = staff.StaffType
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        var staffId = GetCurrentStaffId();
        if (staffId is null) return RedirectToPage("/Auth/Login");

        if (!ModelState.IsValid)
        {
            return Page();
        }

        var staff = new StaffEntity
        {
            Id = staffId.Value,
            FullName = Input.FullName,
            Email = Input.Email,
            Phone = Input.Phone,
            Specialization = Input.Specialization
        };

        var updated = await _staffService.UpdateAsync(staff, ct);
        if (updated is null)
        {
            ModelState.AddModelError(string.Empty, "Không thể cập nhật thông tin. Vui lòng thử lại.");
            return Page();
        }

        SuccessMessage = "Cập nhật thông tin cá nhân thành công.";

        Input.StaffType = updated.StaffType;

        return Page();
    }

    private int? GetCurrentStaffId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (claim is not null && int.TryParse(claim.Value, out var id))
            return id;
        return null;
    }

    public class ProfileInput
    {
        [Required(ErrorMessage = "Vui lòng nhập họ tên.")]
        [StringLength(200, ErrorMessage = "Họ tên tối đa 200 ký tự.")]
        public string FullName { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string? Phone { get; set; }

        public string? Specialization { get; set; }

        public StaffType StaffType { get; set; }
    }
}
