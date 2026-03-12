using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Clinic.Services.Services.Staffs;
using Clinic.Services.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using StaffEntity = Clinic.Services.Domain.Entities.Staff;

namespace Clinic.Web.Pages.Staff;

[Authorize]
public class EditModel : PageModel
{
    private readonly IStaffService _staffService;

    public EditModel(IStaffService staffService)
    {
        _staffService = staffService;
    }

    [BindProperty]
    public StaffInput Input { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken ct)
    {
        var staff = await _staffService.GetByIdAsync(id, ct);
        if (staff is null)
        {
            return RedirectToPage("/Staff/Index");
        }

        Input = new StaffInput
        {
            Id = staff.Id,
            FullName = staff.FullName,
            StaffType = staff.StaffType,
            Email = staff.Email,
            Phone = staff.Phone,
            Specialization = staff.Specialization,
            IsActive = staff.IsActive
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var staff = new StaffEntity
        {
            Id = Input.Id,
            FullName = Input.FullName,
            StaffType = Input.StaffType,
            Email = Input.Email,
            Phone = Input.Phone,
            Specialization = Input.Specialization,
            IsActive = Input.IsActive
        };

        var updated = await _staffService.UpdateAsync(staff, ct);
        if (updated is null)
        {
            return RedirectToPage("/Staff/Index");
        }

        return RedirectToPage("/Staff/Index");
    }

    public class StaffInput
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập họ tên.")]
        [StringLength(200, ErrorMessage = "Họ tên tối đa 200 ký tự.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng chọn chức vụ.")]
        public StaffType StaffType { get; set; }

        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string? Phone { get; set; }

        public string? Specialization { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
