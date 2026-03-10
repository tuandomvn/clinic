using Clinic.Services.Models.Prescription;

namespace Clinic.Services.Services.Prescriptions;

public interface IPrescriptionService
{
    Task<(PrescriptionResponse? Result, string? Error)> CreateAsync(CreatePrescriptionRequest request, CancellationToken ct = default);
    Task<PrescriptionResponse?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<PrescriptionResponse>> GetByHealthRecordIdAsync(int healthRecordId, CancellationToken ct = default);
    Task<PrescriptionResponse?> UpdateAsync(int id, UpdatePrescriptionRequest request, CancellationToken ct = default);
    Task<bool> DeleteAsync(int id, CancellationToken ct = default);
}
