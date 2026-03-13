using PatientEntity = Clinic.Services.Domain.Entities.Patient;

namespace Clinic.Services.Services.Patients;

public interface IPatientService
{
    Task<IReadOnlyList<PatientEntity>> GetAllAsync(CancellationToken ct = default);

    /// <summary>Lấy danh sách bệnh nhân có phân trang + sắp xếp.</summary>
    Task<(IReadOnlyList<PatientEntity> Items, int TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        string? sortBy,
        bool ascending,
        CancellationToken ct = default);

    /// <summary>Tìm kiếm + phân trang cho DataTables server-side.</summary>
    Task<(IReadOnlyList<PatientEntity> Items, int FilteredCount, int TotalCount)> SearchPagedAsync(
        int skip, int take, string? search, string? sortBy, bool ascending, CancellationToken ct = default);

    /// <summary>Lấy chi tiết bệnh nhân theo ID.</summary>
    Task<PatientEntity?> GetByIdAsync(int id, CancellationToken ct = default);

    /// <summary>Lấy chi tiết bệnh nhân kèm theo Activities và Appointments.</summary>
    Task<PatientEntity?> GetByIdWithDetailsAsync(int id, CancellationToken ct = default);

    /// <summary>Tạo bệnh nhân mới.</summary>
    Task<PatientEntity> CreateAsync(PatientEntity patient, CancellationToken ct = default);

    /// <summary>Cập nhật thông tin bệnh nhân.</summary>
    Task<PatientEntity?> UpdateAsync(PatientEntity patient, CancellationToken ct = default);
}

