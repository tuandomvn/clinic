using Clinic.Services.Data;
using Clinic.Services.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Web.Pages.Visits;

[Authorize]
public class HealthRecordModel : PageModel
{
    private readonly ClinicDbContext _dbContext;

    public HealthRecordModel(ClinicDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public HealthRecord Record { get; set; } = null!;

    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    public async Task<IActionResult> OnGetAsync(CancellationToken ct = default)
    {
        if (Id <= 0)
            return RedirectToPage("/Patients/Index");

        var record = await _dbContext.HistoryRecords
            .Include(r => r.Patient)
            .Include(r => r.Staff)
            .Include(r => r.Prescriptions)
            .Include(r => r.ClinicalExams)
            .Include(r => r.ServiceOrders)
            .FirstOrDefaultAsync(r => r.Id == Id, ct);

        if (record is null)
            return NotFound();

        Record = record;
        return Page();
    }

    public async Task<IActionResult> OnPostSaveAsync([FromBody] SaveRecordRequest request, CancellationToken ct = default)
    {
        if (Id <= 0)
            return BadRequest();

        var record = await _dbContext.HistoryRecords
            .FirstOrDefaultAsync(r => r.Id == Id, ct);

        if (record is null)
            return NotFound();

        record.Diagnosis = request.Diagnosis?.Trim();
        record.Symptoms = request.Symptoms?.Trim();
        record.Notes = request.Notes?.Trim();
        record.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(ct);

        return new JsonResult(new { success = true });
    }

    public async Task<IActionResult> OnPostSaveExamAsync([FromBody] SaveExamRequest request, CancellationToken ct = default)
    {
        if (Id <= 0)
            return BadRequest();

        var recordExists = await _dbContext.HistoryRecords
            .AnyAsync(r => r.Id == Id, ct);

        if (!recordExists)
            return NotFound();

        var exam = await _dbContext.ClinicalExams
            .FirstOrDefaultAsync(e => e.HealthRecordId == Id, ct);

        var now = DateTime.UtcNow;

        if (exam is null)
        {
            exam = new ClinicalExam { HealthRecordId = Id, CreatedAt = now };
            _dbContext.ClinicalExams.Add(exam);
        }

        exam.Temperature = request.Temperature;
        exam.BloodPressure = request.BloodPressure?.Trim();
        exam.HeartRate = request.HeartRate;
        exam.RespiratoryRate = request.RespiratoryRate;
        exam.SpO2 = request.SpO2;
        exam.Weight = request.Weight;
        exam.Height = request.Height;
        exam.GeneralCondition = request.GeneralCondition?.Trim();
        exam.SkinExam = request.SkinExam?.Trim();
        exam.HeadNeckExam = request.HeadNeckExam?.Trim();
        exam.ChestExam = request.ChestExam?.Trim();
        exam.AbdomenExam = request.AbdomenExam?.Trim();
        exam.ExtremitiesExam = request.ExtremitiesExam?.Trim();
        exam.NeurologyExam = request.NeurologyExam?.Trim();
        exam.OtherFindings = request.OtherFindings?.Trim();
        exam.UpdatedAt = now;

        await _dbContext.SaveChangesAsync(ct);

        return new JsonResult(new { success = true });
    }

    public record SaveRecordRequest(string? Diagnosis, string? Symptoms, string? Notes);

    public record SaveExamRequest(
        decimal? Temperature, string? BloodPressure, int? HeartRate, int? RespiratoryRate,
        decimal? SpO2, decimal? Weight, decimal? Height,
        string? GeneralCondition, string? SkinExam, string? HeadNeckExam,
        string? ChestExam, string? AbdomenExam, string? ExtremitiesExam,
        string? NeurologyExam, string? OtherFindings);

    public async Task<IActionResult> OnPostSavePrescriptionsAsync([FromBody] SavePrescriptionsRequest request, CancellationToken ct = default)
    {
        if (Id <= 0)
            return BadRequest();

        var recordExists = await _dbContext.HistoryRecords.AnyAsync(r => r.Id == Id, ct);
        if (!recordExists)
            return NotFound();

        var existing = await _dbContext.Prescriptions
            .Where(p => p.HealthRecordId == Id)
            .ToListAsync(ct);

        _dbContext.Prescriptions.RemoveRange(existing);

        var now = DateTime.UtcNow;
        foreach (var item in request.Items)
        {
            if (string.IsNullOrWhiteSpace(item.MedicineName))
                continue;

            _dbContext.Prescriptions.Add(new Prescription
            {
                HealthRecordId = Id,
                MedicineName = item.MedicineName.Trim(),
                Dosage = item.Dosage?.Trim(),
                Frequency = item.Frequency?.Trim(),
                Duration = item.Duration?.Trim(),
                Instructions = item.Instructions?.Trim(),
                IsActive = true,
                CreatedAt = now
            });
        }

        await _dbContext.SaveChangesAsync(ct);
        return new JsonResult(new { success = true });
    }

    public async Task<IActionResult> OnPostSaveServicesAsync([FromBody] SaveServicesRequest request, CancellationToken ct = default)
    {
        if (Id <= 0)
            return BadRequest();

        var recordExists = await _dbContext.HistoryRecords.AnyAsync(r => r.Id == Id, ct);
        if (!recordExists)
            return NotFound();

        var existing = await _dbContext.ServiceOrders
            .Where(s => s.HealthRecordId == Id)
            .ToListAsync(ct);

        _dbContext.ServiceOrders.RemoveRange(existing);

        var now = DateTime.UtcNow;
        foreach (var item in request.Items)
        {
            if (string.IsNullOrWhiteSpace(item.ServiceName))
                continue;

            _dbContext.ServiceOrders.Add(new ServiceOrder
            {
                HealthRecordId = Id,
                ServiceName = item.ServiceName.Trim(),
                Quantity = item.Quantity > 0 ? item.Quantity : 1,
                UnitPrice = item.UnitPrice,
                Notes = item.Notes?.Trim(),
                CreatedAt = now
            });
        }

        await _dbContext.SaveChangesAsync(ct);
        return new JsonResult(new { success = true });
    }

    public record SavePrescriptionsRequest(List<PrescriptionItem> Items);
    public record PrescriptionItem(string MedicineName, string? Dosage, string? Frequency, string? Duration, string? Instructions);

    public record SaveServicesRequest(List<ServiceItem> Items);
    public record ServiceItem(string ServiceName, int Quantity, decimal? UnitPrice, string? Notes);
}
