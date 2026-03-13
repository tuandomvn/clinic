using StaffEntity = Clinic.Services.Domain.Entities.Staff;

namespace Clinic.Services.Services.Staffs;

public interface IStaffService
{
    Task<IReadOnlyList<StaffEntity>> GetAllAsync(CancellationToken ct = default);
    Task<(IReadOnlyList<StaffEntity> Items, int FilteredCount, int TotalCount)> SearchPagedAsync(
        int skip, int take, string? search, string? sortBy, bool ascending, CancellationToken ct = default);
    Task<StaffEntity?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<StaffEntity> CreateAsync(StaffEntity staff, CancellationToken ct = default);
    Task<StaffEntity?> UpdateAsync(StaffEntity staff, CancellationToken ct = default);
}

