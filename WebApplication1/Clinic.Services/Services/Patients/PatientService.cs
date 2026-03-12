using Clinic.Services.Data;
using Microsoft.EntityFrameworkCore;
using PatientEntity = Clinic.Services.Domain.Entities.Patient;

namespace Clinic.Services.Services.Patients;

public sealed class PatientService : IPatientService
{
    private readonly ClinicDbContext _db;

    public PatientService(ClinicDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<PatientEntity>> GetAllAsync(CancellationToken ct = default)
    {
        return await _db.Patients
            .AsNoTracking()
            .OrderBy(p => p.FullName)
            .ToListAsync(ct);
    }

    public async Task<(IReadOnlyList<PatientEntity> Items, int TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        string? sortBy,
        bool ascending,
        CancellationToken ct = default)
    {
        if (page <= 0) page = 1;
        if (pageSize <= 0) pageSize = 10;

        var query = _db.Patients.AsNoTracking();

        sortBy = string.IsNullOrWhiteSpace(sortBy) ? "name" : sortBy.ToLowerInvariant();
        query = sortBy switch
        {
            "phone" => ascending
                ? query.OrderBy(p => p.Phone)
                : query.OrderByDescending(p => p.Phone),
            "lastvisit" => ascending
                ? query.OrderBy(p => p.CreatedAt)
                : query.OrderByDescending(p => p.CreatedAt),
            _ => ascending
                ? query.OrderBy(p => p.FullName)
                : query.OrderByDescending(p => p.FullName)
        };

        var totalCount = await query.CountAsync(ct);
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, totalCount);
    }

    public async Task<PatientEntity?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _db.Patients
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id, ct);
    }

    public async Task<PatientEntity?> GetByIdWithDetailsAsync(int id, CancellationToken ct = default)
    {
        return await _db.Patients
            .AsNoTracking()
            .Include(p => p.Activities.OrderByDescending(a => a.CreatedDate))
                .ThenInclude(a => a.Images)
            .Include(p => p.Appointments.OrderByDescending(a => a.ScheduledAt))
                .ThenInclude(a => a.Staff)
            .FirstOrDefaultAsync(p => p.Id == id, ct);
    }

    public async Task<PatientEntity> CreateAsync(PatientEntity patient, CancellationToken ct = default)
    {
        patient.CreatedAt = DateTime.UtcNow;
        patient.IsActive = true;
        patient.BarcodeValue ??= Guid.NewGuid().ToString("N")[..12].ToUpperInvariant();

        _db.Patients.Add(patient);
        await _db.SaveChangesAsync(ct);

        return patient;
    }

    public async Task<PatientEntity?> UpdateAsync(PatientEntity patient, CancellationToken ct = default)
    {
        var existing = await _db.Patients.FindAsync([patient.Id], ct);
        if (existing is null) return null;

        existing.FullName = patient.FullName;
        existing.DateOfBirth = patient.DateOfBirth;
        existing.Gender = patient.Gender;
        existing.Phone = patient.Phone;
        existing.Email = patient.Email;
        existing.Address = patient.Address;
        existing.IdentityNumber = patient.IdentityNumber;
        existing.InsuranceNumber = patient.InsuranceNumber;
        existing.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync(ct);

        return existing;
    }
}

