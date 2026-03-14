namespace Clinic.Services.Domain.Entities;

public class ServiceOrder
{
    public int Id { get; set; }
    public int HealthRecordId { get; set; }

    public string ServiceName { get; set; } = string.Empty;
    public int Quantity { get; set; } = 1;
    public decimal? UnitPrice { get; set; }
    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public HealthRecord HealthRecord { get; set; } = null!;
}
