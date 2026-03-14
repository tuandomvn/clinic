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
    public DbSet<ActivityImage> ActivityImages { get; set; }
    public DbSet<ReminderTask> ReminderTasks { get; set; }
    public DbSet<ClinicalExam> ClinicalExams { get; set; }
    public DbSet<ServiceOrder> ServiceOrders { get; set; }
    public DbSet<OperatingRoom> OperatingRooms { get; set; }

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
            e.Property(x => x.AvatarPath).HasMaxLength(500);
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
            e.Property(x => x.SurgeryType).HasMaxLength(200);
            e.Property(x => x.Description).HasMaxLength(1000);
            e.Property(x => x.Notes).HasMaxLength(500);
            e.HasOne(x => x.Patient).WithMany(p => p.SurgerySchedules).HasForeignKey(x => x.PatientId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.OperatingRoom).WithMany(r => r.SurgerySchedules).HasForeignKey(x => x.OperatingRoomId).OnDelete(DeleteBehavior.SetNull);
            e.HasIndex(x => x.ScheduledAt);
            e.HasIndex(x => x.OperatingRoomId);
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
            e.Property(x => x.ContentText).HasMaxLength(2000).IsRequired();
            e.HasOne(x => x.Patient).WithMany(p => p.Activities).HasForeignKey(x => x.PatientId).OnDelete(DeleteBehavior.SetNull);
            e.HasOne(x => x.RelatedEncounter).WithMany().HasForeignKey(x => x.RelatedEncounterId).OnDelete(DeleteBehavior.SetNull);
            e.HasIndex(x => x.CreatedDate);
        });

        modelBuilder.Entity<ActivityImage>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.ImageUrl).HasMaxLength(500).IsRequired();
            e.Property(x => x.Caption).HasMaxLength(300);
            e.HasOne(x => x.Activity).WithMany(a => a.Images).HasForeignKey(x => x.ActivityId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ReminderTask>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Description).HasMaxLength(500).IsRequired();
            e.HasOne(x => x.Patient).WithMany().HasForeignKey(x => x.PatientId).OnDelete(DeleteBehavior.Cascade);
            e.HasIndex(x => x.DueDate);
            e.HasIndex(x => x.IsDone);
        });

        modelBuilder.Entity<ClinicalExam>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.BloodPressure).HasMaxLength(20);
            e.Property(x => x.Temperature).HasPrecision(4, 1);
            e.Property(x => x.SpO2).HasPrecision(4, 1);
            e.Property(x => x.Weight).HasPrecision(5, 1);
            e.Property(x => x.Height).HasPrecision(5, 1);
            e.Property(x => x.GeneralCondition).HasMaxLength(1000);
            e.Property(x => x.SkinExam).HasMaxLength(1000);
            e.Property(x => x.HeadNeckExam).HasMaxLength(1000);
            e.Property(x => x.ChestExam).HasMaxLength(1000);
            e.Property(x => x.AbdomenExam).HasMaxLength(1000);
            e.Property(x => x.ExtremitiesExam).HasMaxLength(1000);
            e.Property(x => x.NeurologyExam).HasMaxLength(1000);
            e.Property(x => x.OtherFindings).HasMaxLength(2000);
            e.HasOne(x => x.HealthRecord)
                .WithMany(hr => hr.ClinicalExams)
                .HasForeignKey(x => x.HealthRecordId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasIndex(x => x.HealthRecordId);
        });

        modelBuilder.Entity<ServiceOrder>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.ServiceName).HasMaxLength(300).IsRequired();
            e.Property(x => x.UnitPrice).HasPrecision(18, 2);
            e.Property(x => x.Notes).HasMaxLength(1000);
            e.HasOne(x => x.HealthRecord)
                .WithMany(hr => hr.ServiceOrders)
                .HasForeignKey(x => x.HealthRecordId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasIndex(x => x.HealthRecordId);
        });

        modelBuilder.Entity<OperatingRoom>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).HasMaxLength(100).IsRequired();
            e.Property(x => x.Location).HasMaxLength(200);
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
            new SurgerySchedule { Id = 1, PatientId = 3, OperatingRoomId = 1, ScheduledAt = new DateTime(2024, 3, 25, 7, 0, 0, DateTimeKind.Utc), DurationMinutes = 120, SurgeryType = "Phẫu thuật ruột thừa", Description = "Nội soi cắt ruột thừa", Status = SurgeryStatus.Scheduled, CreatedAt = new DateTime(2024, 3, 5, 0, 0, 0, DateTimeKind.Utc) },
        };
        modelBuilder.Entity<SurgerySchedule>().HasData(surgerySchedules);

        var surgeryTeam = new[]
        {
            new SurgeryScheduleStaff { SurgeryScheduleId = 1, StaffId = 3, TeamRole = "Surgeon", ScheduledAt = new DateTime(2024, 3, 25, 7, 0, 0, DateTimeKind.Utc), DurationMinutes = 120 },
        };
        modelBuilder.Entity<SurgeryScheduleStaff>().HasData(surgeryTeam);

        var operatingRooms = new[]
        {
            new OperatingRoom { Id = 1, Name = "Phòng mổ 1", Location = "Tầng 2", IsActive = true },
            new OperatingRoom { Id = 2, Name = "Phòng mổ 2", Location = "Tầng 2", IsActive = true },
            new OperatingRoom { Id = 3, Name = "Phòng mổ 3", Location = "Tầng 3", IsActive = true },
        };
        modelBuilder.Entity<OperatingRoom>().HasData(operatingRooms);

        var activities = new List<Activity>();
        var activityImages = new List<ActivityImage>();

        var patientIds = Enumerable.Range(1, 100).ToList();
        var staffIds = new[] { 1, 2, 3, 4 };
        var allTypes = new[]
        {
            ActivityType.Post,
            ActivityType.BirthdayGreeting,
            ActivityType.AppointmentCreated,
            ActivityType.HistoryRecordAdded,
            ActivityType.SurgeryScheduled,
            ActivityType.PatientRegistered,
            ActivityType.General
        };

        var contentByType = new Dictionary<ActivityType, string[]>
        {
            [ActivityType.Post] = [
                "Hôm nay bệnh nhân đã hồi phục tốt sau phẫu thuật.",
                "Kết quả xét nghiệm máu bình thường, không có dấu hiệu bất thường.",
                "Bệnh nhân phản hồi tích cực sau đợt điều trị.",
                "Cập nhật tình trạng sức khỏe: huyết áp ổn định.",
                "Ghi nhận tăng cân 2kg sau 1 tháng điều trị.",
                "Vết thương đã lành tốt, không có dấu hiệu nhiễm trùng.",
                "Kết quả chụp X-quang cho thấy xương đã liền.",
                "Bệnh nhân đã hoàn thành liệu trình vật lý trị liệu.",
                "Ghi nhận phản ứng dị ứng nhẹ với thuốc mới.",
                "Bệnh nhân cần theo dõi thêm sau khi dùng thuốc mới."
            ],
            [ActivityType.BirthdayGreeting] = [
                "Đã gửi chúc mừng sinh nhật tới bệnh nhân!",
                "Hệ thống đã gửi lời chúc sinh nhật tự động.",
                "Đã gửi tin nhắn chúc mừng sinh nhật.",
                "Chúc bệnh nhân sinh nhật vui vẻ, sức khỏe dồi dào!",
                "Đã gửi thiệp chúc mừng sinh nhật qua email."
            ],
            [ActivityType.AppointmentCreated] = [
                "Đã book lịch tái khám vào tuần tới.",
                "Đã đặt lịch khám tổng quát.",
                "Đã book lịch xét nghiệm định kỳ.",
                "Đã sắp xếp lịch hẹn kiểm tra sức khỏe.",
                "Đã đặt lịch tư vấn chuyên khoa.",
                "Đã book lịch tái khám vào ngày 20/3.",
                "Đã đặt lịch khám mắt cho bệnh nhân."
            ],
            [ActivityType.HistoryRecordAdded] = [
                "Đã thêm kết quả khám lâm sàng vào hồ sơ.",
                "Cập nhật kết quả xét nghiệm máu.",
                "Đã ghi nhận chẩn đoán mới vào hồ sơ bệnh án.",
                "Thêm kết quả siêu âm vào hồ sơ.",
                "Đã cập nhật tiền sử bệnh lý.",
                "Ghi nhận kết quả đo huyết áp.",
                "Đã thêm ghi chú điều trị vào hồ sơ."
            ],
            [ActivityType.SurgeryScheduled] = [
                "Đã lên lịch phẫu thuật cắt ruột thừa.",
                "Đã xếp lịch mổ nội soi.",
                "Đã đặt lịch phẫu thuật chỉnh hình.",
                "Đã sắp xếp ca mổ tim.",
                "Đã lên lịch tiểu phẫu.",
                "Đã xếp lịch phẫu thuật mắt."
            ],
            [ActivityType.PatientRegistered] = [
                "Đăng ký bệnh nhân mới thành công. Hồ sơ đầy đủ.",
                "Bệnh nhân đã được tiếp nhận và tạo hồ sơ.",
                "Hoàn tất đăng ký bệnh nhân, đã cấp mã barcode.",
                "Đã tạo hồ sơ bệnh nhân mới trong hệ thống.",
                "Tiếp nhận bệnh nhân chuyển viện, đã tạo hồ sơ."
            ],
            [ActivityType.General] = [
                "Cập nhật thông tin liên hệ bệnh nhân.",
                "Gửi tin nhắn nhắc lịch khám.",
                "Cập nhật thông tin bảo hiểm y tế.",
                "Ghi chú điều trị thêm.",
                "Yêu cầu kiểm tra lại kết quả.",
                "Chuyển bệnh nhân sang chuyên khoa khác.",
                "Cập nhật liên hệ khẩn cấp."
            ]
        };

        var imageCaptions = new[]
        {
            "Ảnh chẩn đoán", "Kết quả X-quang", "Ảnh siêu âm", "Ảnh vết thương",
            "Kết quả xét nghiệm", "Ảnh trước điều trị", "Ảnh sau điều trị",
            "Ảnh CT scan", "Ảnh MRI", "Ảnh nội soi"
        };

        int activityId = 1;
        int imageId = 1;

        foreach (var patientId in patientIds)
        {
            var random = new Random(patientId * 31);
            var activitiesPerPatient = random.Next(38, 43); // ~40 per patient

            for (int i = 0; i < activitiesPerPatient; i++)
            {
                var activityType = allTypes[random.Next(allTypes.Length)];
                var texts = contentByType[activityType];
                var contentText = texts[random.Next(texts.Length)];

                var createdBy = activityType switch
                {
                    ActivityType.BirthdayGreeting => -1,
                    ActivityType.PatientRegistered => -1,
                    _ => staffIds[random.Next(staffIds.Length)]
                };

                var daysAgo = random.Next(1, 365);
                var activityDate = utc.AddDays(-daysAgo).AddHours(random.Next(7, 18)).AddMinutes(random.Next(0, 60));
                var currentActivityId = activityId++;

                activities.Add(new Activity
                {
                    Id = currentActivityId,
                    ActivityType = activityType,
                    ContentText = contentText,
                    CreatedBy = createdBy,
                    PatientId = patientId,
                    CreatedDate = activityDate
                });

                // Add images: Post ~50%, HistoryRecordAdded ~40%, SurgeryScheduled ~30%, others ~10%
                var imageChance = activityType switch
                {
                    ActivityType.Post => 0.5,
                    ActivityType.HistoryRecordAdded => 0.4,
                    ActivityType.SurgeryScheduled => 0.3,
                    _ => 0.1
                };

                if (random.NextDouble() < imageChance)
                {
                    var imageCount = random.Next(1, 5); // 1-4 images
                    for (int j = 0; j < imageCount; j++)
                    {
                        activityImages.Add(new ActivityImage
                        {
                            Id = imageId++,
                            ActivityId = currentActivityId,
                            ImageUrl = $"{imageId - 1}.jpg",
                            Caption = random.NextDouble() > 0.4 ? imageCaptions[random.Next(imageCaptions.Length)] : null,
                            CreatedDate = activityDate
                        });
                    }
                }
            }
        }

        modelBuilder.Entity<Activity>().HasData(activities);
        modelBuilder.Entity<ActivityImage>().HasData(activityImages);

        // Seed ReminderTasks (~100)
        var reminderTasks = new List<ReminderTask>();
        var taskDescriptions = new Dictionary<ReminderTaskType, string[]>
        {
            [ReminderTaskType.FollowUp] = [
                "Tái khám sau phẫu thuật ruột thừa (7 ngày)",
                "Kiểm tra vết mổ sau 14 ngày - Phẫu thuật gối",
                "Tái khám sau mổ sỏi thận (1 tháng)",
                "Tái khám sau phẫu thuật mắt (2 tuần)",
                "Kiểm tra hồi phục sau nội soi dạ dày",
                "Tái khám sau điều trị viêm phổi",
                "Kiểm tra sau phẫu thuật tim (1 tháng)",
                "Follow-up sau cắt amidan (10 ngày)"
            ],
            [ReminderTaskType.BirthdayGreeting] = [
                "Gửi lời chúc sinh nhật",
                "Chúc mừng sinh nhật bệnh nhân",
                "Gửi thiệp chúc mừng sinh nhật qua email",
                "Gọi điện chúc mừng sinh nhật"
            ],
            [ReminderTaskType.VaccinationReminder] = [
                "Tiêm nhắc lại mũi 3 - Viêm gan B",
                "Tiêm phòng cúm mùa hàng năm",
                "Tiêm vaccine COVID-19 mũi tăng cường",
                "Tiêm phòng uốn ván nhắc lại",
                "Tiêm vaccine phế cầu khuẩn",
                "Tiêm nhắc lại sởi - quai bị - rubella"
            ],
            [ReminderTaskType.PeriodicTest] = [
                "Xét nghiệm máu định kỳ 3 tháng (tiểu đường)",
                "Siêu âm gan 6 tháng (viêm gan B mạn)",
                "Xét nghiệm chức năng tuyến giáp 6 tháng",
                "Nội soi đại tràng định kỳ (1 năm)",
                "Xét nghiệm mỡ máu định kỳ 6 tháng",
                "Chụp X-quang phổi định kỳ hàng năm",
                "Xét nghiệm HbA1c 3 tháng",
                "Đo mật độ xương định kỳ"
            ],
            [ReminderTaskType.General] = [
                "Nhắc bệnh nhân uống thuốc đúng liều",
                "Cập nhật thông tin bảo hiểm y tế",
                "Gửi kết quả xét nghiệm qua email",
                "Liên hệ bệnh nhân xác nhận lịch hẹn",
                "Nhắc bệnh nhân mang theo hồ sơ cũ"
            ]
        };

        var taskRandom = new Random(42);
        var allTaskTypes = Enum.GetValues<ReminderTaskType>();
        var priorities = Enum.GetValues<TaskPriority>();

        for (int i = 1; i <= 100; i++)
        {
            var taskType = allTaskTypes[taskRandom.Next(allTaskTypes.Length)];
            var descs = taskDescriptions[taskType];
            var desc = descs[taskRandom.Next(descs.Length)];
            var pid = taskRandom.Next(1, 51); // patients 1-50
            var daysOffset = taskRandom.Next(-10, 15); // some overdue, some today, some future
            var dueDate = utc.AddDays(daysOffset).Date;
            var priority = taskType switch
            {
                ReminderTaskType.FollowUp => TaskPriority.High,
                ReminderTaskType.PeriodicTest => priorities[taskRandom.Next(1, 3)], // Medium or High
                ReminderTaskType.BirthdayGreeting => TaskPriority.Low,
                _ => priorities[taskRandom.Next(priorities.Length)]
            };
            var isDone = daysOffset < -3 ? taskRandom.NextDouble() > 0.3 : // older → more likely done
                         daysOffset < 0 ? taskRandom.NextDouble() > 0.6 :
                         taskRandom.NextDouble() > 0.85;
            var doneByStaffId = isDone ? staffIds[taskRandom.Next(staffIds.Length)] : (int?)null;
            var doneAt = isDone ? dueDate.AddHours(taskRandom.Next(8, 17)) : (DateTime?)null;
            var createdBy = taskType == ReminderTaskType.BirthdayGreeting ? -1 : staffIds[taskRandom.Next(staffIds.Length)];

            reminderTasks.Add(new ReminderTask
            {
                Id = i,
                PatientId = pid,
                TaskType = taskType,
                Description = desc,
                DueDate = dueDate,
                Priority = priority,
                IsDone = isDone,
                DoneByStaffId = doneByStaffId,
                DoneAt = doneAt,
                CreatedBy = createdBy,
                CreatedAt = dueDate.AddDays(-taskRandom.Next(3, 14))
            });
        }

        modelBuilder.Entity<ReminderTask>().HasData(reminderTasks);
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
