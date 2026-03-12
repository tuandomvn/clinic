using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Clinic.Services.Services.Patients;
using Clinic.Services.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Clinic.Web.Pages.Patients;

[Authorize]
public class CreateModel : PageModel
{
    private readonly IPatientService _patientService;

    public CreateModel(IPatientService patientService)
    {
        _patientService = patientService;
    }

    [BindProperty]
    public PatientInput Input { get; set; } = new();

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var patient = new Patient
        {
            FullName = Input.FullName,
            DateOfBirth = Input.DateOfBirth,
            Gender = Input.Gender,
            Phone = Input.Phone,
            Email = Input.Email,
            Address = Input.Address,
            IdentityNumber = Input.IdentityNumber,
            InsuranceNumber = Input.InsuranceNumber
        };

        var created = await _patientService.CreateAsync(patient, ct);

        return RedirectToPage("/Patients/Details", new { id = created.Id });
    }

    public class PatientInput
    {
        [Required(ErrorMessage = "Vui lòng nhập họ tên.")]
        [StringLength(200, ErrorMessage = "Họ tên tối đa 200 ký tự.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập ngày sinh.")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; } = DateTime.Today.AddYears(-30);

        public string? Gender { get; set; }

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string? Phone { get; set; }

        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string? Email { get; set; }

        public string? Address { get; set; }

        public string? IdentityNumber { get; set; }

        public string? InsuranceNumber { get; set; }
    }
}
