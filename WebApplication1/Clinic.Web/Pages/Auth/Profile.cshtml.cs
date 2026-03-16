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
    private readonly string _uploadPath;

    public ProfileModel(IStaffService staffService, IConfiguration configuration)
    {
        _staffService = staffService;
        _uploadPath = configuration["FileStorage:UploadPath"]
            ?? throw new InvalidOperationException("FileStorage:UploadPath is not configured.");
    }

    [BindProperty]
    public ProfileInput Input { get; set; } = new();

    public string? AvatarUrl { get; set; }

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

        AvatarUrl = staff.AvatarPath;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        var staffId = GetCurrentStaffId();
        if (staffId is null) return RedirectToPage("/Auth/Login");

        if (!ModelState.IsValid)
        {
            await LoadAvatarAsync(staffId.Value, ct);
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
            await LoadAvatarAsync(staffId.Value, ct);
            return Page();
        }

        SuccessMessage = "Cập nhật thông tin cá nhân thành công.";
        Input.StaffType = updated.StaffType;
        AvatarUrl = updated.AvatarPath;

        return Page();
    }

    public async Task<IActionResult> OnPostUploadAvatarAsync(IFormFile avatar, CancellationToken ct)
    {
        var staffId = GetCurrentStaffId();
        if (staffId is null) return RedirectToPage("/Auth/Login");

        if (avatar is null || avatar.Length == 0)
        {
            ModelState.AddModelError(string.Empty, "Vui lòng chọn file ảnh.");
            await ReloadInputAsync(staffId.Value, ct);
            return Page();
        }

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
        var ext = Path.GetExtension(avatar.FileName).ToLowerInvariant();
        if (!allowedExtensions.Contains(ext))
        {
            ModelState.AddModelError(string.Empty, "Chỉ chấp nhận file ảnh JPG, PNG hoặc WebP.");
            await ReloadInputAsync(staffId.Value, ct);
            return Page();
        }

        if (avatar.Length > 2 * 1024 * 1024)
        {
            ModelState.AddModelError(string.Empty, "File ảnh không được vượt quá 2MB.");
            await ReloadInputAsync(staffId.Value, ct);
            return Page();
        }

        var uploadsDir = Path.Combine(_uploadPath, "avatars");
        Directory.CreateDirectory(uploadsDir);

        var fileName = $"staff-{staffId}-{Guid.NewGuid():N}{ext}";
        var filePath = Path.Combine(uploadsDir, fileName);

        // Delete old avatar file if exists
        var currentStaff = await _staffService.GetByIdAsync(staffId.Value, ct);
        if (currentStaff?.AvatarPath is not null)
        {
            var oldFile = Path.Combine(_uploadPath, currentStaff.AvatarPath.TrimStart('/'));
            if (System.IO.File.Exists(oldFile))
            {
                System.IO.File.Delete(oldFile);
            }
        }

        await using var stream = new FileStream(filePath, FileMode.Create);
        await avatar.CopyToAsync(stream, ct);

        var avatarUrl = $"/avatars/{fileName}";
        await _staffService.UpdateAvatarAsync(staffId.Value, avatarUrl, ct);

        SuccessMessage = "Cập nhật ảnh đại diện thành công.";
        await ReloadInputAsync(staffId.Value, ct);
        return Page();
    }

    private async Task LoadAvatarAsync(int staffId, CancellationToken ct)
    {
        var staff = await _staffService.GetByIdAsync(staffId, ct);
        AvatarUrl = staff?.AvatarPath;
    }

    private async Task ReloadInputAsync(int staffId, CancellationToken ct)
    {
        var staff = await _staffService.GetByIdAsync(staffId, ct);
        if (staff is null) return;

        Input = new ProfileInput
        {
            FullName = staff.FullName,
            Email = staff.Email,
            Phone = staff.Phone,
            Specialization = staff.Specialization,
            StaffType = staff.StaffType
        };
        AvatarUrl = staff.AvatarPath;
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
