using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Clinic.Services.Migrations
{
    /// <inheritdoc />
    public partial class SeedSampleData1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FullName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DateOfBirth = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Gender = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Phone = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Address = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdentityNumber = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InsuranceNumber = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Staff",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FullName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StaffType = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Phone = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Specialization = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staff", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SurgerySchedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    ScheduledAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DurationMinutes = table.Column<int>(type: "int", nullable: false),
                    Room = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SurgeryType = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurgerySchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SurgerySchedules_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ActivityType = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StaffId = table.Column<int>(type: "int", nullable: true),
                    PatientId = table.Column<int>(type: "int", nullable: true),
                    EntityType = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Activities_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Activities_Staff_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Staff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    StaffId = table.Column<int>(type: "int", nullable: false),
                    ScheduledAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DurationMinutes = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Reason = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Appointments_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Appointments_Staff_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Staff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "HistoryRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    StaffId = table.Column<int>(type: "int", nullable: false),
                    VisitDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Diagnosis = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Symptoms = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Notes = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoryRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoryRecords_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HistoryRecords_Staff_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Staff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StaffId = table.Column<int>(type: "int", nullable: false),
                    Username = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PasswordHash = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Role = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastLoginAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAccounts_Staff_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Staff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SurgeryScheduleStaff",
                columns: table => new
                {
                    SurgeryScheduleId = table.Column<int>(type: "int", nullable: false),
                    StaffId = table.Column<int>(type: "int", nullable: false),
                    TeamRole = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurgeryScheduleStaff", x => new { x.SurgeryScheduleId, x.StaffId });
                    table.ForeignKey(
                        name: "FK_SurgeryScheduleStaff_Staff_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Staff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SurgeryScheduleStaff_SurgerySchedules_SurgeryScheduleId",
                        column: x => x.SurgeryScheduleId,
                        principalTable: "SurgerySchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Prescriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    HealthRecordId = table.Column<int>(type: "int", nullable: false),
                    MedicineName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Dosage = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Frequency = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Duration = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Instructions = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prescriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prescriptions_HistoryRecords_HealthRecordId",
                        column: x => x.HealthRecordId,
                        principalTable: "HistoryRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Patients",
                columns: new[] { "Id", "Address", "CreatedAt", "DateOfBirth", "Email", "FullName", "Gender", "IdentityNumber", "InsuranceNumber", "IsActive", "Phone", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "123 Đường ABC, Quận 1, TP.HCM", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1985, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "hoangminh@email.com", "Hoàng Minh Anh", "Nam", null, null, true, "0912345678", null },
                    { 2, "456 Đường XYZ, Quận 3, TP.HCM", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Nguyễn Thị Bình", "Nữ", null, null, true, "0912345679", null },
                    { 3, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1978, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Trần Văn Cường", "Nam", "012345678901", null, true, "0912345680", null },
                    { 4, "Địa chỉ demo 4", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 4", "Nam", null, null, true, "090000004", null },
                    { 5, "Địa chỉ demo 5", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 5", "Nữ", null, null, true, "090000005", null },
                    { 6, "Địa chỉ demo 6", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 6", "Nam", null, null, true, "090000006", null },
                    { 7, "Địa chỉ demo 7", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 7", "Nữ", null, null, true, "090000007", null },
                    { 8, "Địa chỉ demo 8", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 8", "Nam", null, null, true, "090000008", null },
                    { 9, "Địa chỉ demo 9", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 9", "Nữ", null, null, true, "090000009", null },
                    { 10, "Địa chỉ demo 10", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 1, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 10", "Nam", null, null, true, "090000010", null },
                    { 11, "Địa chỉ demo 11", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 11", "Nữ", null, null, true, "090000011", null },
                    { 12, "Địa chỉ demo 12", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 1, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 12", "Nam", null, null, true, "090000012", null },
                    { 13, "Địa chỉ demo 13", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 13", "Nữ", null, null, true, "090000013", null },
                    { 14, "Địa chỉ demo 14", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 14", "Nam", null, null, true, "090000014", null },
                    { 15, "Địa chỉ demo 15", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 1, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 15", "Nữ", null, null, true, "090000015", null },
                    { 16, "Địa chỉ demo 16", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 1, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 16", "Nam", null, null, true, "090000016", null },
                    { 17, "Địa chỉ demo 17", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 1, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 17", "Nữ", null, null, true, "090000017", null },
                    { 18, "Địa chỉ demo 18", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 1, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 18", "Nam", null, null, true, "090000018", null },
                    { 19, "Địa chỉ demo 19", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 19", "Nữ", null, null, true, "090000019", null },
                    { 20, "Địa chỉ demo 20", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 20", "Nam", null, null, true, "090000020", null },
                    { 21, "Địa chỉ demo 21", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 21", "Nữ", null, null, true, "090000021", null },
                    { 22, "Địa chỉ demo 22", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 1, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 22", "Nam", null, null, true, "090000022", null },
                    { 23, "Địa chỉ demo 23", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 1, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 23", "Nữ", null, null, true, "090000023", null },
                    { 24, "Địa chỉ demo 24", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 1, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 24", "Nam", null, null, true, "090000024", null },
                    { 25, "Địa chỉ demo 25", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 1, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 25", "Nữ", null, null, true, "090000025", null },
                    { 26, "Địa chỉ demo 26", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 1, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 26", "Nam", null, null, true, "090000026", null },
                    { 27, "Địa chỉ demo 27", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 1, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 27", "Nữ", null, null, true, "090000027", null },
                    { 28, "Địa chỉ demo 28", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 1, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 28", "Nam", null, null, true, "090000028", null },
                    { 29, "Địa chỉ demo 29", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 1, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 29", "Nữ", null, null, true, "090000029", null },
                    { 30, "Địa chỉ demo 30", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 30", "Nam", null, null, true, "090000030", null },
                    { 31, "Địa chỉ demo 31", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 31", "Nữ", null, null, true, "090000031", null },
                    { 32, "Địa chỉ demo 32", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 32", "Nam", null, null, true, "090000032", null },
                    { 33, "Địa chỉ demo 33", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 2, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 33", "Nữ", null, null, true, "090000033", null },
                    { 34, "Địa chỉ demo 34", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 2, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 34", "Nam", null, null, true, "090000034", null },
                    { 35, "Địa chỉ demo 35", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 35", "Nữ", null, null, true, "090000035", null },
                    { 36, "Địa chỉ demo 36", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 2, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 36", "Nam", null, null, true, "090000036", null },
                    { 37, "Địa chỉ demo 37", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 2, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 37", "Nữ", null, null, true, "090000037", null },
                    { 38, "Địa chỉ demo 38", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 2, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 38", "Nam", null, null, true, "090000038", null },
                    { 39, "Địa chỉ demo 39", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 2, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 39", "Nữ", null, null, true, "090000039", null },
                    { 40, "Địa chỉ demo 40", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 40", "Nam", null, null, true, "090000040", null },
                    { 41, "Địa chỉ demo 41", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 2, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 41", "Nữ", null, null, true, "090000041", null },
                    { 42, "Địa chỉ demo 42", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 2, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 42", "Nam", null, null, true, "090000042", null },
                    { 43, "Địa chỉ demo 43", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 2, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 43", "Nữ", null, null, true, "090000043", null },
                    { 44, "Địa chỉ demo 44", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 2, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 44", "Nam", null, null, true, "090000044", null },
                    { 45, "Địa chỉ demo 45", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 45", "Nữ", null, null, true, "090000045", null },
                    { 46, "Địa chỉ demo 46", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 2, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 46", "Nam", null, null, true, "090000046", null },
                    { 47, "Địa chỉ demo 47", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 47", "Nữ", null, null, true, "090000047", null },
                    { 48, "Địa chỉ demo 48", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 2, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 48", "Nam", null, null, true, "090000048", null },
                    { 49, "Địa chỉ demo 49", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 2, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 49", "Nữ", null, null, true, "090000049", null },
                    { 50, "Địa chỉ demo 50", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 50", "Nam", null, null, true, "090000050", null },
                    { 51, "Địa chỉ demo 51", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 2, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 51", "Nữ", null, null, true, "090000051", null },
                    { 52, "Địa chỉ demo 52", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 2, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 52", "Nam", null, null, true, "090000052", null },
                    { 53, "Địa chỉ demo 53", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 2, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 53", "Nữ", null, null, true, "090000053", null },
                    { 54, "Địa chỉ demo 54", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 2, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 54", "Nam", null, null, true, "090000054", null },
                    { 55, "Địa chỉ demo 55", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 2, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 55", "Nữ", null, null, true, "090000055", null },
                    { 56, "Địa chỉ demo 56", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 2, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 56", "Nam", null, null, true, "090000056", null },
                    { 57, "Địa chỉ demo 57", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 2, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 57", "Nữ", null, null, true, "090000057", null },
                    { 58, "Địa chỉ demo 58", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 2, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 58", "Nam", null, null, true, "090000058", null },
                    { 59, "Địa chỉ demo 59", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 59", "Nữ", null, null, true, "090000059", null },
                    { 60, "Địa chỉ demo 60", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 60", "Nam", null, null, true, "090000060", null },
                    { 61, "Địa chỉ demo 61", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 61", "Nữ", null, null, true, "090000061", null },
                    { 62, "Địa chỉ demo 62", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 62", "Nam", null, null, true, "090000062", null },
                    { 63, "Địa chỉ demo 63", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 63", "Nữ", null, null, true, "090000063", null },
                    { 64, "Địa chỉ demo 64", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 64", "Nam", null, null, true, "090000064", null },
                    { 65, "Địa chỉ demo 65", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 65", "Nữ", null, null, true, "090000065", null },
                    { 66, "Địa chỉ demo 66", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 66", "Nam", null, null, true, "090000066", null },
                    { 67, "Địa chỉ demo 67", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 67", "Nữ", null, null, true, "090000067", null },
                    { 68, "Địa chỉ demo 68", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 68", "Nam", null, null, true, "090000068", null },
                    { 69, "Địa chỉ demo 69", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 69", "Nữ", null, null, true, "090000069", null },
                    { 70, "Địa chỉ demo 70", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 70", "Nam", null, null, true, "090000070", null },
                    { 71, "Địa chỉ demo 71", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 71", "Nữ", null, null, true, "090000071", null },
                    { 72, "Địa chỉ demo 72", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 72", "Nam", null, null, true, "090000072", null },
                    { 73, "Địa chỉ demo 73", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 73", "Nữ", null, null, true, "090000073", null },
                    { 74, "Địa chỉ demo 74", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 74", "Nam", null, null, true, "090000074", null },
                    { 75, "Địa chỉ demo 75", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 75", "Nữ", null, null, true, "090000075", null },
                    { 76, "Địa chỉ demo 76", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 76", "Nam", null, null, true, "090000076", null },
                    { 77, "Địa chỉ demo 77", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 77", "Nữ", null, null, true, "090000077", null },
                    { 78, "Địa chỉ demo 78", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 78", "Nam", null, null, true, "090000078", null },
                    { 79, "Địa chỉ demo 79", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 79", "Nữ", null, null, true, "090000079", null },
                    { 80, "Địa chỉ demo 80", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 80", "Nam", null, null, true, "090000080", null },
                    { 81, "Địa chỉ demo 81", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 81", "Nữ", null, null, true, "090000081", null },
                    { 82, "Địa chỉ demo 82", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 82", "Nam", null, null, true, "090000082", null },
                    { 83, "Địa chỉ demo 83", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 83", "Nữ", null, null, true, "090000083", null },
                    { 84, "Địa chỉ demo 84", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 84", "Nam", null, null, true, "090000084", null },
                    { 85, "Địa chỉ demo 85", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 85", "Nữ", null, null, true, "090000085", null },
                    { 86, "Địa chỉ demo 86", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 86", "Nam", null, null, true, "090000086", null },
                    { 87, "Địa chỉ demo 87", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 87", "Nữ", null, null, true, "090000087", null },
                    { 88, "Địa chỉ demo 88", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 88", "Nam", null, null, true, "090000088", null },
                    { 89, "Địa chỉ demo 89", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 89", "Nữ", null, null, true, "090000089", null },
                    { 90, "Địa chỉ demo 90", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 90", "Nam", null, null, true, "090000090", null },
                    { 91, "Địa chỉ demo 91", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 4, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 91", "Nữ", null, null, true, "090000091", null },
                    { 92, "Địa chỉ demo 92", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 4, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 92", "Nam", null, null, true, "090000092", null },
                    { 93, "Địa chỉ demo 93", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 93", "Nữ", null, null, true, "090000093", null },
                    { 94, "Địa chỉ demo 94", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 94", "Nam", null, null, true, "090000094", null },
                    { 95, "Địa chỉ demo 95", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 4, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 95", "Nữ", null, null, true, "090000095", null },
                    { 96, "Địa chỉ demo 96", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 96", "Nam", null, null, true, "090000096", null },
                    { 97, "Địa chỉ demo 97", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 97", "Nữ", null, null, true, "090000097", null },
                    { 98, "Địa chỉ demo 98", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 98", "Nam", null, null, true, "090000098", null },
                    { 99, "Địa chỉ demo 99", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 99", "Nữ", null, null, true, "090000099", null },
                    { 100, "Địa chỉ demo 100", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 4, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bệnh nhân demo 100", "Nam", null, null, true, "090000100", null }
                });

            migrationBuilder.InsertData(
                table: "Staff",
                columns: new[] { "Id", "CreatedAt", "Email", "FullName", "IsActive", "Phone", "Specialization", "StaffType", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "doctor1@clinic.com", "Nguyễn Văn Bác sĩ", true, "0901234567", "Nội tổng quát", 0, null },
                    { 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "nurse1@clinic.com", "Trần Thị Y tá", true, "0901234568", null, 1, null },
                    { 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "doctor2@clinic.com", "Lê Văn Phẫu thuật", true, "0901234569", "Ngoại khoa", 0, null },
                    { 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "nurse2@clinic.com", "Phạm Thị Điều dưỡng", true, "0901234570", null, 1, null }
                });

            migrationBuilder.InsertData(
                table: "Activities",
                columns: new[] { "Id", "ActivityType", "CreatedAt", "Description", "EntityId", "EntityType", "PatientId", "StaffId" },
                values: new object[,]
                {
                    { 1, 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Đăng ký bệnh nhân mới: Hoàng Minh Anh", 1, "Patient", 1, null },
                    { 2, 0, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Tạo lịch hẹn tái khám", 1, "Appointment", 1, 1 },
                    { 3, 3, new DateTime(2024, 3, 5, 0, 0, 0, 0, DateTimeKind.Utc), "Lên lịch phẫu thuật ruột thừa", 1, "SurgerySchedule", 3, 3 }
                });

            migrationBuilder.InsertData(
                table: "Appointments",
                columns: new[] { "Id", "CreatedAt", "DurationMinutes", "Notes", "PatientId", "Reason", "ScheduledAt", "StaffId", "Status", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 30, null, 1, "Tái khám", new DateTime(2024, 3, 20, 8, 0, 0, 0, DateTimeKind.Utc), 1, 0, null },
                    { 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 30, null, 2, "Khám tổng quát", new DateTime(2024, 3, 21, 9, 0, 0, 0, DateTimeKind.Utc), 1, 0, null }
                });

            migrationBuilder.InsertData(
                table: "HistoryRecords",
                columns: new[] { "Id", "CreatedAt", "Diagnosis", "Notes", "PatientId", "StaffId", "Symptoms", "UpdatedAt", "VisitDate" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Cảm cúm nhẹ", "Nghỉ ngơi, uống nhiều nước", 1, 1, "Sốt, đau họng", null, new DateTime(2024, 2, 10, 8, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Khám sức khỏe định kỳ", "Kết quả tốt", 2, 1, "Không", null, new DateTime(2024, 2, 15, 9, 30, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "SurgerySchedules",
                columns: new[] { "Id", "CreatedAt", "Description", "DurationMinutes", "Notes", "PatientId", "Room", "ScheduledAt", "Status", "SurgeryType", "UpdatedAt" },
                values: new object[] { 1, new DateTime(2024, 3, 5, 0, 0, 0, 0, DateTimeKind.Utc), "Nội soi cắt ruột thừa", 120, null, 3, "Phòng 101", new DateTime(2024, 3, 25, 7, 0, 0, 0, DateTimeKind.Utc), 0, "Phẫu thuật ruột thừa", null });

            migrationBuilder.InsertData(
                table: "Prescriptions",
                columns: new[] { "Id", "CreatedAt", "Dosage", "Duration", "Frequency", "HealthRecordId", "Instructions", "IsActive", "MedicineName", "UpdatedAt" },
                values: new object[] { 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "1 viên", "5 ngày", "3 lần/ngày", 1, "Uống sau ăn", true, "Paracetamol 500mg", null });

            migrationBuilder.InsertData(
                table: "SurgeryScheduleStaff",
                columns: new[] { "StaffId", "SurgeryScheduleId", "TeamRole" },
                values: new object[] { 3, 1, "Surgeon" });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_CreatedAt",
                table: "Activities",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_PatientId",
                table: "Activities",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_StaffId",
                table: "Activities",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_PatientId",
                table: "Appointments",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_ScheduledAt",
                table: "Appointments",
                column: "ScheduledAt");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_StaffId",
                table: "Appointments",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoryRecords_PatientId",
                table: "HistoryRecords",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoryRecords_StaffId",
                table: "HistoryRecords",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_Phone",
                table: "Patients",
                column: "Phone");

            migrationBuilder.CreateIndex(
                name: "IX_Prescriptions_HealthRecordId",
                table: "Prescriptions",
                column: "HealthRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_Email",
                table: "Staff",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_SurgerySchedules_PatientId",
                table: "SurgerySchedules",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_SurgerySchedules_ScheduledAt",
                table: "SurgerySchedules",
                column: "ScheduledAt");

            migrationBuilder.CreateIndex(
                name: "IX_SurgeryScheduleStaff_StaffId",
                table: "SurgeryScheduleStaff",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_StaffId",
                table: "UserAccounts",
                column: "StaffId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_Username",
                table: "UserAccounts",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "Prescriptions");

            migrationBuilder.DropTable(
                name: "SurgeryScheduleStaff");

            migrationBuilder.DropTable(
                name: "UserAccounts");

            migrationBuilder.DropTable(
                name: "HistoryRecords");

            migrationBuilder.DropTable(
                name: "SurgerySchedules");

            migrationBuilder.DropTable(
                name: "Staff");

            migrationBuilder.DropTable(
                name: "Patients");
        }
    }
}
