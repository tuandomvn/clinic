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
}

