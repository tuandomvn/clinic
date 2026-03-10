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
}

