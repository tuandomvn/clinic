using Microsoft.EntityFrameworkCore;
using Clinic.Services.Domain.Entities;
using System.Linq;

namespace Clinic.Services.Data;

public class ClinicDbContext : DbContext
{
    public ClinicDbContext(DbContextOptions<ClinicDbContext> options)
        : base(options)
    {
    }

    public DbSet<Staff> Staff { get; set; }
    public DbSet<UserAccount> UserAccounts { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<HealthRecord> HistoryRecords { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<SurgerySchedule> SurgerySchedules { get; set; }
    public DbSet<SurgeryScheduleStaff> SurgeryScheduleStaff { get; set; }
    public DbSet<Activity> Activities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Staff>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.FullName).HasMaxLength(200).IsRequired();
            e.Property(x => x.Email).HasMaxLength(200);
            e.Property(x => x.Phone).HasMaxLength(50);
            e.Property(x => x.Specialization).HasMaxLength(200);
            e.HasIndex(x => x.Email);
        });

        modelBuilder.Entity<UserAccount>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Username).HasMaxLength(100).IsRequired();
            e.Property(x => x.PasswordHash).HasMaxLength(500).IsRequired();
            e.Property(x => x.Role).HasMaxLength(50).IsRequired();
            e.HasIndex(x => x.Username).IsUnique();
            e.HasIndex(x => x.StaffId).IsUnique();

            e.HasOne(x => x.Staff)
                .WithOne(s => s.UserAccount)
                .HasForeignKey<UserAccount>(x => x.StaffId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Patient>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.FullName).HasMaxLength(200).IsRequired();
            e.Property(x => x.Gender).HasMaxLength(20);
            e.Property(x => x.Phone).HasMaxLength(50);
            e.Property(x => x.Email).HasMaxLength(200);
            e.Property(x => x.Address).HasMaxLength(500);
            e.Property(x => x.IdentityNumber).HasMaxLength(50);
            e.Property(x => x.InsuranceNumber).HasMaxLength(100);
            e.HasIndex(x => x.Phone);
        });

        modelBuilder.Entity<HealthRecord>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Diagnosis).HasMaxLength(500);
            e.Property(x => x.Symptoms).HasMaxLength(1000);
            e.Property(x => x.Notes).HasMaxLength(2000);
            e.HasOne(x => x.Patient).WithMany(p => p.HistoryRecords).HasForeignKey(x => x.PatientId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Staff).WithMany(s => s.HistoryRecords).HasForeignKey(x => x.StaffId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Prescription>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.MedicineName).HasMaxLength(200).IsRequired();
            e.Property(x => x.Dosage).HasMaxLength(100);
            e.Property(x => x.Frequency).HasMaxLength(100);
            e.Property(x => x.Duration).HasMaxLength(100);
            e.Property(x => x.Instructions).HasMaxLength(1000);
            e.HasIndex(x => x.HealthRecordId);
            e.HasOne(x => x.HealthRecord)
                .WithMany(hr => hr.Prescriptions)
                .HasForeignKey(x => x.HealthRecordId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Appointment>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Notes).HasMaxLength(500);
            e.Property(x => x.Reason).HasMaxLength(300);
            e.HasOne(x => x.Patient).WithMany(p => p.Appointments).HasForeignKey(x => x.PatientId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Staff).WithMany(s => s.Appointments).HasForeignKey(x => x.StaffId).OnDelete(DeleteBehavior.Restrict);
            e.HasIndex(x => x.ScheduledAt);
        });

        modelBuilder.Entity<SurgerySchedule>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Room).HasMaxLength(50);
            e.Property(x => x.SurgeryType).HasMaxLength(200);
            e.Property(x => x.Description).HasMaxLength(1000);
            e.Property(x => x.Notes).HasMaxLength(500);
            e.HasOne(x => x.Patient).WithMany(p => p.SurgerySchedules).HasForeignKey(x => x.PatientId).OnDelete(DeleteBehavior.Restrict);
            e.HasIndex(x => x.ScheduledAt);
        });

        modelBuilder.Entity<SurgeryScheduleStaff>(e =>
        {
            e.HasKey(x => new { x.SurgeryScheduleId, x.StaffId });
            e.Property(x => x.TeamRole).HasMaxLength(100);

            e.HasOne(x => x.SurgerySchedule)
                .WithMany(s => s.TeamMembers)
                .HasForeignKey(x => x.SurgeryScheduleId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(x => x.Staff)
                .WithMany(s => s.SurgeryAssignments)
                .HasForeignKey(x => x.StaffId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Activity>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Description).HasMaxLength(500).IsRequired();
            e.Property(x => x.EntityType).HasMaxLength(100);
            e.HasOne(x => x.Staff).WithMany(s => s.Activities).HasForeignKey(x => x.StaffId).OnDelete(DeleteBehavior.SetNull);
            e.HasOne(x => x.Patient).WithMany(p => p.Activities).HasForeignKey(x => x.PatientId).OnDelete(DeleteBehavior.SetNull);
            e.HasIndex(x => x.CreatedAt);
        });

        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        var utc = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        // Seed Staff
        var staff = new[]
        {
            new Staff { Id = 1, FullName = "Nguyễn Văn Bác sĩ", StaffType = StaffType.Doctor, Email = "doctor1@clinic.com", Phone = "0901234567", Specialization = "Nội tổng quát", IsActive = true, CreatedAt = utc },
            new Staff { Id = 2, FullName = "Trần Thị Y tá", StaffType = StaffType.Nurse, Email = "nurse1@clinic.com", Phone = "0901234568", IsActive = true, CreatedAt = utc },
            new Staff { Id = 3, FullName = "Lê Văn Phẫu thuật", StaffType = StaffType.Doctor, Email = "doctor2@clinic.com", Phone = "0901234569", Specialization = "Ngoại khoa", IsActive = true, CreatedAt = utc },
            new Staff { Id = 4, FullName = "Phạm Thị Điều dưỡng", StaffType = StaffType.Nurse, Email = "nurse2@clinic.com", Phone = "0901234570", IsActive = true, CreatedAt = utc },
        };
        modelBuilder.Entity<Staff>().HasData(staff);

        // Seed UserAccounts (linked to Staff)
        var userAccounts = new[]
        {
            new UserAccount { Id = 1, StaffId = 1, Username = "doctor1", PasswordHash = HashPassword("Password@123"), Role = "Doctor", IsActive = true, CreatedAt = utc },
            new UserAccount { Id = 2, StaffId = 2, Username = "nurse1", PasswordHash = HashPassword("Password@123"), Role = "Nurse", IsActive = true, CreatedAt = utc },
            new UserAccount { Id = 3, StaffId = 3, Username = "doctor2", PasswordHash = HashPassword("Password@123"), Role = "Doctor", IsActive = true, CreatedAt = utc },
            new UserAccount { Id = 4, StaffId = 4, Username = "nurse2", PasswordHash = HashPassword("Password@123"), Role = "Nurse", IsActive = true, CreatedAt = utc },
        };
        modelBuilder.Entity<UserAccount>().HasData(userAccounts);

        var basePatients = new[]
        {
            new Patient { Id = 1, FullName = "Hoàng Minh Anh", DateOfBirth = new DateTime(1985, 5, 15), Gender = "Nam", Phone = "0912345678", Email = "hoangminh@email.com", Address = "123 Đường ABC, Quận 1, TP.HCM", IsActive = true, CreatedAt = utc },
            new Patient { Id = 2, FullName = "Nguyễn Thị Bình", DateOfBirth = new DateTime(1990, 8, 20), Gender = "Nữ", Phone = "0912345679", Address = "456 Đường XYZ, Quận 3, TP.HCM", IsActive = true, CreatedAt = utc },
            new Patient { Id = 3, FullName = "Trần Văn Cường", DateOfBirth = new DateTime(1978, 3, 10), Gender = "Nam", Phone = "0912345680", IdentityNumber = "012345678901", IsActive = true, CreatedAt = utc },
        };

        var demoPatients = Enumerable.Range(4, 97)
            .Select(i => new Patient
            {
                Id = i,
                FullName = $"Bệnh nhân demo {i}",
                DateOfBirth = new DateTime(1990, 1, 1).AddDays(i),
                IdentityNumber = $"ID{i:000000000}",
                BarcodeValue = $"BC{i:000000000}",
                BarcodeType = BarcodeType.Code128,
                Gender = i % 2 == 0 ? "Nam" : "Nữ",
                Phone = $"09{i:0000000}",
                Email = $"dominht{i}@gmail.com",
                Address = $"Địa chỉ demo {i}",
                IsActive = true,
                CreatedAt = utc
            })
            .ToArray();

        var patients = basePatients.Concat(demoPatients).ToArray();
        modelBuilder.Entity<Patient>().HasData(patients);

        var historyRecords = new[]
        {
            new HealthRecord { Id = 1, PatientId = 1, StaffId = 1, VisitDate = new DateTime(2024, 2, 10, 8, 0, 0, DateTimeKind.Utc), Diagnosis = "Cảm cúm nhẹ", Symptoms = "Sốt, đau họng", Notes = "Nghỉ ngơi, uống nhiều nước", CreatedAt = utc },
            new HealthRecord { Id = 2, PatientId = 2, StaffId = 1, VisitDate = new DateTime(2024, 2, 15, 9, 30, 0, DateTimeKind.Utc), Diagnosis = "Khám sức khỏe định kỳ", Symptoms = "Không", Notes = "Kết quả tốt", CreatedAt = utc },
        };
        modelBuilder.Entity<HealthRecord>().HasData(historyRecords);

        var prescriptions = new[]
        {
            new Prescription { Id = 1, HealthRecordId = 1, MedicineName = "Paracetamol 500mg", Dosage = "1 viên", Frequency = "3 lần/ngày", Duration = "5 ngày", Instructions = "Uống sau ăn", IsActive = true, CreatedAt = utc },
        };
        modelBuilder.Entity<Prescription>().HasData(prescriptions);

        var appointments = new[]
        {
            new Appointment { Id = 1, PatientId = 1, StaffId = 1, ScheduledAt = new DateTime(2024, 3, 20, 8, 0, 0, DateTimeKind.Utc), DurationMinutes = 30, Status = AppointmentStatus.Scheduled, Reason = "Tái khám", CreatedAt = utc },
            new Appointment { Id = 2, PatientId = 2, StaffId = 1, ScheduledAt = new DateTime(2024, 3, 21, 9, 0, 0, DateTimeKind.Utc), DurationMinutes = 30, Status = AppointmentStatus.Scheduled, Reason = "Khám tổng quát", CreatedAt = utc },
        };
        modelBuilder.Entity<Appointment>().HasData(appointments);

        var surgerySchedules = new[]
        {
            new SurgerySchedule { Id = 1, PatientId = 3, ScheduledAt = new DateTime(2024, 3, 25, 7, 0, 0, DateTimeKind.Utc), DurationMinutes = 120, Room = "Phòng 101", SurgeryType = "Phẫu thuật ruột thừa", Description = "Nội soi cắt ruột thừa", Status = SurgeryStatus.Scheduled, CreatedAt = new DateTime(2024, 3, 5, 0, 0, 0, DateTimeKind.Utc) },
        };
        modelBuilder.Entity<SurgerySchedule>().HasData(surgerySchedules);

        var surgeryTeam = new[]
        {
            new SurgeryScheduleStaff { SurgeryScheduleId = 1, StaffId = 3, TeamRole = "Surgeon" },
        };
        modelBuilder.Entity<SurgeryScheduleStaff>().HasData(surgeryTeam);

        var activities = new List<Activity>();

        // Base activities
        var baseActivities = new[]
        {
            new Activity { Id = 1, ActivityType = ActivityType.PatientRegistered, Description = "Đăng ký bệnh nhân mới: Hoàng Minh Anh", PatientId = 1, EntityType = "Patient", EntityId = 1, CreatedAt = utc },
            new Activity { Id = 2, ActivityType = ActivityType.AppointmentCreated, Description = "Tạo lịch hẹn tái khám", StaffId = 1, PatientId = 1, EntityType = "Appointment", EntityId = 1, CreatedAt = utc },
            new Activity { Id = 3, ActivityType = ActivityType.SurgeryScheduled, Description = "Lên lịch phẫu thuật ruột thừa", StaffId = 3, PatientId = 3, EntityType = "SurgerySchedule", EntityId = 1, CreatedAt = new DateTime(2024, 3, 5, 0, 0, 0, DateTimeKind.Utc) },
        };
        activities.AddRange(baseActivities);

        // Generate activities for each patient (base + demo)
        var patientIds = Enumerable.Range(1, 100).ToList(); // 100 patients
        var activityDescriptions = new[]
        {
            "Tạo lịch hẹn khám bệnh",
            "Hoàn thành khám bệnh",
            "Thêm kết quả khám lâm sàng",
            "Cập nhật thông tin bệnh nhân",
            "Gửi tin nhắn nhắc lịch khám",
            "Hủy lịch hẹn",
            "Rescheduled lịch khám",
            "Thêm kết quả xét nghiệm",
            "Tái khám theo hướng dẫn",
            "Kết thúc điều trị",
            "Cập nhật thuốc điều trị",
            "Ghi chú điều trị thêm",
            "Chuyển bệnh nhân chuyên khoa",
            "Cập nhật tiền sử bệnh",
            "Thêm ảnh chẩn đoán hình ảnh",
            "Yêu cầu tư vấn chuyên khoa",
            "Hoàn thành đơn thuốc",
            "Gửi phiếu khám về nhà",
            "Lên kế hoạch điều trị mới",
            "Kiểm tra tình trạng sức khỏe định kỳ",
            "Ghi nhận tăng cân/giảm cân",
            "Cập nhật tiền sử dị ứng",
            "Thêm ghi chú y bác sĩ",
            "Yêu cầu kiểm tra lại",
            "Sắp xếp nhập viện",
            "Xuất viện và theo dõi",
            "Gửi lời mời khám định kỳ",
            "Cập nhật liên hệ khẩn cấp",
            "Ghi nhận phản ứng thuốc",
            "Thêm ghi chú dinh dưỡng",
        };

        var activityTypes = new[] 
        { 
            ActivityType.AppointmentCreated, 
            ActivityType.AppointmentCompleted, 
            ActivityType.HistoryRecordAdded, 
            ActivityType.General 
        };

        var staffIds = new[] { 1, 2, 3, 4 };
        int activityId = 4; // Start after base activities (1,2,3)

        foreach (var patientId in patientIds)
        {
            var random = new Random(patientId); // Use patientId as seed for consistent results
            var activitiesPerPatient = random.Next(35, 45); // 35-45 activities per patient

            for (int i = 0; i < activitiesPerPatient; i++)
            {
                var activityType = activityTypes[random.Next(activityTypes.Length)];
                var staffId = random.NextDouble() > 0.3 ? (int?)staffIds[random.Next(staffIds.Length)] : null;
                var description = activityDescriptions[random.Next(activityDescriptions.Length)];

                // Generate date from past (spread across months)
                var daysAgo = random.Next(1, 300);
                var activityDate = utc.AddDays(-daysAgo);

                activities.Add(new Activity
                {
                    Id = activityId++,
                    ActivityType = activityType,
                    Description = description,
                    StaffId = staffId,
                    PatientId = patientId,
                    EntityType = activityType switch
                    {
                        ActivityType.AppointmentCreated => "Appointment",
                        ActivityType.AppointmentCompleted => "Appointment",
                        ActivityType.HistoryRecordAdded => "HealthRecord",
                        _ => "General"
                    },
                    EntityId = random.Next(1, 10),
                    CreatedAt = activityDate
                });
            }
        }

        modelBuilder.Entity<Activity>().HasData(activities);
    }

    /// <summary>
    /// Simple PBKDF2-SHA256 password hashing for seeding
    /// </summary>
    private static string HashPassword(string password)
    {
        using var deriveBytes = new System.Security.Cryptography.Rfc2898DeriveBytes(
            password, 16, 10000, System.Security.Cryptography.HashAlgorithmName.SHA256);
        var salt = deriveBytes.Salt;
        var hash = deriveBytes.GetBytes(32);

        // Combine salt and hash
        var hashBytes = new byte[48];
        System.Buffer.BlockCopy(salt, 0, hashBytes, 0, 16);
        System.Buffer.BlockCopy(hash, 0, hashBytes, 16, 32);

        return Convert.ToBase64String(hashBytes);
    }
}
