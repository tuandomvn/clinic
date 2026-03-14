namespace Clinic.Services.Domain.Entities;

public class ClinicalExam
{
    public int Id { get; set; }
    public int HealthRecordId { get; set; }

    // Sinh hiệu (Vital Signs)
    public decimal? Temperature { get; set; }       // Nhiệt độ (°C)
    public string? BloodPressure { get; set; }      // Huyết áp (mmHg), vd: "120/80"
    public int? HeartRate { get; set; }              // Mạch (lần/phút)
    public int? RespiratoryRate { get; set; }        // Nhịp thở (lần/phút)
    public decimal? SpO2 { get; set; }               // SpO2 (%)
    public decimal? Weight { get; set; }             // Cân nặng (kg)
    public decimal? Height { get; set; }             // Chiều cao (cm)

    // Khám tổng quát
    public string? GeneralCondition { get; set; }   // Toàn thân
    public string? SkinExam { get; set; }           // Da, niêm mạc
    public string? HeadNeckExam { get; set; }       // Đầu, cổ
    public string? ChestExam { get; set; }          // Ngực, phổi
    public string? AbdomenExam { get; set; }        // Bụng
    public string? ExtremitiesExam { get; set; }    // Chi, cơ xương khớp
    public string? NeurologyExam { get; set; }      // Thần kinh
    public string? OtherFindings { get; set; }      // Khám khác

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }

    public HealthRecord HealthRecord { get; set; } = null!;
}
