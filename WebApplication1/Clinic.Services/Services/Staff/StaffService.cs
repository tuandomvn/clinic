using Clinic.Services.Data;
using StaffEntity = Clinic.Services.Domain.Entities.Staff;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Services.Services.Staffs;

public sealed class StaffService : IStaffService
{
    private readonly ClinicDbContext _db;

    public StaffService(ClinicDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<StaffEntity>> GetAllAsync(CancellationToken ct = default)
    {
        return await _db.Staff
            .AsNoTracking()
            .OrderBy(s => s.FullName)
            .ToListAsync(ct);
    }

    public async Task<StaffEntity?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _db.Staff
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id, ct);
    }

    public async Task<StaffEntity> CreateAsync(StaffEntity staff, CancellationToken ct = default)
    {
        staff.CreatedAt = DateTime.UtcNow;
        _db.Staff.Add(staff);
        await _db.SaveChangesAsync(ct);
        return staff;
    }

    public async Task<StaffEntity?> UpdateAsync(StaffEntity staff, CancellationToken ct = default)
    {
        var existing = await _db.Staff.FindAsync([staff.Id], ct);
        if (existing is null) return null;

        existing.FullName = staff.FullName;
        existing.Email = staff.Email;
        existing.Phone = staff.Phone;
        existing.Specialization = staff.Specialization;
        existing.StaffType = staff.StaffType;
        existing.IsActive = staff.IsActive;
        existing.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync(ct);
        return existing;
    }
}

