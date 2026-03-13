using TaskEntity = Clinic.Services.Domain.Entities.ReminderTask;

namespace Clinic.Services.Services.Tasks;

public interface IReminderTaskService
{
    /// <summary>Tìm kiếm + phân trang cho DataTables server-side.</summary>
    Task<(IReadOnlyList<TaskEntity> Items, int FilteredCount, int TotalCount)> SearchPagedAsync(
        int skip, int take, string? search, string? filterType, string? filterStatus,
        string? sortBy, bool ascending, CancellationToken ct = default);

    /// <summary>Đánh dấu hoàn thành các task theo danh sách Id.</summary>
    Task<int> MarkDoneAsync(IEnumerable<int> ids, int? staffId, CancellationToken ct = default);

    /// <summary>Đánh dấu chưa hoàn thành các task theo danh sách Id.</summary>
    Task<int> MarkUndoneAsync(IEnumerable<int> ids, CancellationToken ct = default);
}
