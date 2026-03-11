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

    /// <summary>Lấy chi tiết bệnh nhân theo ID.</summary>
    Task<PatientEntity?> GetByIdAsync(int id, CancellationToken ct = default);

    /// <summary>Lấy chi tiết bệnh nhân kèm theo Activities và Appointments.</summary>
    Task<PatientEntity?> GetByIdWithDetailsAsync(int id, CancellationToken ct = default);
}

