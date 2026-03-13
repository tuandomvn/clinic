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

    public async Task<(IReadOnlyList<StaffEntity> Items, int FilteredCount, int TotalCount)> SearchPagedAsync(
        int skip, int take, string? search, string? sortBy, bool ascending, CancellationToken ct = default)
    {
        var query = _db.Staff.AsNoTracking().Include(s => s.UserAccount).AsQueryable();
        var totalCount = await query.CountAsync(ct);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(s =>
                s.FullName.ToLower().Contains(term) ||
                (s.Email != null && s.Email.ToLower().Contains(term)) ||
                (s.Specialization != null && s.Specialization.ToLower().Contains(term)) ||
                (s.Phone != null && s.Phone.Contains(term)));
        }

        query = sortBy switch
        {
            "specialization" => ascending ? query.OrderBy(s => s.Specialization) : query.OrderByDescending(s => s.Specialization),
            "staffType" => ascending ? query.OrderBy(s => s.StaffType) : query.OrderByDescending(s => s.StaffType),
            "phone" => ascending ? query.OrderBy(s => s.Phone) : query.OrderByDescending(s => s.Phone),
            "isActive" => ascending ? query.OrderBy(s => s.IsActive) : query.OrderByDescending(s => s.IsActive),
            _ => ascending ? query.OrderBy(s => s.FullName) : query.OrderByDescending(s => s.FullName)
        };

        var filteredCount = await query.CountAsync(ct);
        var items = await query.Skip(skip).Take(take).ToListAsync(ct);

        return (items, filteredCount, totalCount);
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

    public async Task<StaffEntity?> UpdateAvatarAsync(int staffId, string avatarPath, CancellationToken ct = default)
    {
        var existing = await _db.Staff.FindAsync([staffId], ct);
        if (existing is null) return null;

        existing.AvatarPath = avatarPath;
        existing.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync(ct);
        return existing;
    }
}

