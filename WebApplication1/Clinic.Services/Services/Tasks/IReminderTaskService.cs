using TaskEntity = Clinic.Services.Domain.Entities.ReminderTask;

namespace Clinic.Services.Services.Tasks;

public interface IReminderTaskService
{
    /// <summary>Tìm kiếm + phân trang cho DataTables server-side.</summary>
    Task<(IReadOnlyList<TaskEntity> Items, int FilteredCount, int TotalCount)> SearchPagedAsync(
        int skip, int take, string? search, string? filterType, string? filterStatus,
        string? sortBy, bool ascending, CancellationToken ct = default);
}
