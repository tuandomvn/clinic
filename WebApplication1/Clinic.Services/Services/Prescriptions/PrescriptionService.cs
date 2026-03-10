using Microsoft.EntityFrameworkCore;
using Clinic.Services.Data;
using Clinic.Services.Domain.Entities;
using Clinic.Services.Models.Prescription;

namespace Clinic.Services.Services.Prescriptions;

public sealed class PrescriptionService : IPrescriptionService
{
    private readonly ClinicDbContext _db;

    public PrescriptionService(ClinicDbContext db)
    {
        _db = db;
    }

    public async Task<(PrescriptionResponse? Result, string? Error)> CreateAsync(CreatePrescriptionRequest request, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.MedicineName))
            return (null, "MedicineName is required.");

        var recordExists = await _db.HistoryRecords.AnyAsync(hr => hr.Id == request.HealthRecordId, ct);
        if (!recordExists)
            return (null, "HealthRecord not found.");

        var p = new Prescription
        {
            HealthRecordId = request.HealthRecordId,
            MedicineName = request.MedicineName.Trim(),
            Dosage = request.Dosage,
            Frequency = request.Frequency,
            Duration = request.Duration,
            Instructions = request.Instructions,
            IsActive = true
        };

        _db.Prescriptions.Add(p);
        await _db.SaveChangesAsync(ct);

        return (ToResponse(p), null);
    }

    public async Task<PrescriptionResponse?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var p = await _db.Prescriptions.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
        return p == null ? null : ToResponse(p);
    }

    public async Task<IReadOnlyList<PrescriptionResponse>> GetByHealthRecordIdAsync(int healthRecordId, CancellationToken ct = default)
    {
        var items = await _db.Prescriptions
            .AsNoTracking()
            .Where(x => x.HealthRecordId == healthRecordId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(ct);

        return items.Select(ToResponse).ToList();
    }

    public async Task<PrescriptionResponse?> UpdateAsync(int id, UpdatePrescriptionRequest request, CancellationToken ct = default)
    {
        var p = await _db.Prescriptions.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (p == null) return null;

        if (request.MedicineName != null) p.MedicineName = request.MedicineName.Trim();
        if (request.Dosage != null) p.Dosage = request.Dosage;
        if (request.Frequency != null) p.Frequency = request.Frequency;
        if (request.Duration != null) p.Duration = request.Duration;
        if (request.Instructions != null) p.Instructions = request.Instructions;
        if (request.IsActive.HasValue) p.IsActive = request.IsActive.Value;

        p.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync(ct);

        return ToResponse(p);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
    {
        var p = await _db.Prescriptions.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (p == null) return false;

        _db.Prescriptions.Remove(p);
        await _db.SaveChangesAsync(ct);
        return true;
    }

    private static PrescriptionResponse ToResponse(Prescription p) => new()
    {
        Id = p.Id,
        HealthRecordId = p.HealthRecordId,
        MedicineName = p.MedicineName,
        Dosage = p.Dosage,
        Frequency = p.Frequency,
        Duration = p.Duration,
        Instructions = p.Instructions,
        IsActive = p.IsActive,
        CreatedAt = p.CreatedAt,
        UpdatedAt = p.UpdatedAt
    };
}
