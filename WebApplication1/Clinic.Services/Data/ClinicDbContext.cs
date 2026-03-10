using Microsoft.EntityFrameworkCore;
using Clinic.Services.Domain.Entities;

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
    }
}
