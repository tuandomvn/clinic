using Clinic.Services.Models.Surgery;

namespace Clinic.Services.Services.Surgeries;

public interface ISurgeryScheduleService
{
    Task<(SurgeryScheduleResponse? Result, string? Error)> CreateAsync(CreateSurgeryScheduleRequest request, CancellationToken ct = default);
    Task<SurgeryScheduleResponse?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<SurgeryScheduleResponse>> GetByDateAsync(DateOnly date, CancellationToken ct = default);
}
