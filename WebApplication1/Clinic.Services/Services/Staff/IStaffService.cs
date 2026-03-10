using StaffEntity = Clinic.Services.Domain.Entities.Staff;

namespace Clinic.Services.Services.Staffs;

public interface IStaffService
{
    Task<IReadOnlyList<StaffEntity>> GetAllAsync(CancellationToken ct = default);
}

