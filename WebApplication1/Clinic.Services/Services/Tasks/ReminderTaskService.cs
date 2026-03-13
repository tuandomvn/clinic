using Clinic.Services.Data;
using Clinic.Services.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using TaskEntity = Clinic.Services.Domain.Entities.ReminderTask;

namespace Clinic.Services.Services.Tasks;

public sealed class ReminderTaskService : IReminderTaskService
{
    private readonly ClinicDbContext _db;

    public ReminderTaskService(ClinicDbContext db)
    {
        _db = db;
    }

    public async Task<(IReadOnlyList<TaskEntity> Items, int FilteredCount, int TotalCount)> SearchPagedAsync(
        int skip, int take, string? search, string? filterType, string? filterStatus,
        string? sortBy, bool ascending, CancellationToken ct = default)
    {
        var query = _db.ReminderTasks
            .AsNoTracking()
            .Include(t => t.Patient)
            .AsQueryable();

        var totalCount = await query.CountAsync(ct);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(t =>
                t.Patient.FullName.ToLower().Contains(term) ||
                (t.Patient.Phone != null && t.Patient.Phone.Contains(term)) ||
                t.Description.ToLower().Contains(term));
        }

        if (!string.IsNullOrWhiteSpace(filterType) && Enum.TryParse<ReminderTaskType>(filterType, out var taskType))
        {
            query = query.Where(t => t.TaskType == taskType);
        }

        if (!string.IsNullOrWhiteSpace(filterStatus))
        {
            query = filterStatus switch
            {
                "done" => query.Where(t => t.IsDone),
                "pending" => query.Where(t => !t.IsDone),
                _ => query
            };
        }

        query = sortBy switch
        {
            "patientName" => ascending ? query.OrderBy(t => t.Patient.FullName) : query.OrderByDescending(t => t.Patient.FullName),
            "taskType" => ascending ? query.OrderBy(t => t.TaskType) : query.OrderByDescending(t => t.TaskType),
            "dueDate" => ascending ? query.OrderBy(t => t.DueDate) : query.OrderByDescending(t => t.DueDate),
            "priority" => ascending ? query.OrderBy(t => t.Priority) : query.OrderByDescending(t => t.Priority),
            _ => ascending ? query.OrderBy(t => t.DueDate) : query.OrderByDescending(t => t.DueDate)
        };

        var filteredCount = await query.CountAsync(ct);
        var items = await query.Skip(skip).Take(take).ToListAsync(ct);

        return (items, filteredCount, totalCount);
    }

    public async Task<int> MarkDoneAsync(IEnumerable<int> ids, int? staffId, CancellationToken ct = default)
    {
        var idList = ids.ToList();
        var tasks = await _db.ReminderTasks
            .Where(t => idList.Contains(t.Id) && !t.IsDone)
            .ToListAsync(ct);

        if (tasks.Count == 0)
            return 0;

        var now = DateTime.UtcNow;
        foreach (var task in tasks)
        {
            task.IsDone = true;
            task.DoneAt = now;
            task.DoneByStaffId = staffId;
        }

        await _db.SaveChangesAsync(ct);
        return tasks.Count;
    }

    public async Task<int> MarkUndoneAsync(IEnumerable<int> ids, CancellationToken ct = default)
    {
        var idList = ids.ToList();
        var tasks = await _db.ReminderTasks
            .Where(t => idList.Contains(t.Id) && t.IsDone)
            .ToListAsync(ct);

        if (tasks.Count == 0)
            return 0;

        foreach (var task in tasks)
        {
            task.IsDone = false;
            task.DoneAt = null;
            task.DoneByStaffId = null;
        }

        await _db.SaveChangesAsync(ct);
        return tasks.Count;
    }
}
