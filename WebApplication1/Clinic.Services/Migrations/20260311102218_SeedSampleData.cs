using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Clinic.Services.Migrations
{
    /// <inheritdoc />
    public partial class SeedSampleData : Migration
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
                    BarcodeValue = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BarcodeType = table.Column<int>(type: "int", nullable: false),
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
                columns: new[] { "Id", "Address", "BarcodeType", "BarcodeValue", "CreatedAt", "DateOfBirth", "Email", "FullName", "Gender", "IdentityNumber", "InsuranceNumber", "IsActive", "Phone", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "123 Đường ABC, Quận 1, TP.HCM", 1, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1985, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "hoangminh@email.com", "Hoàng Minh Anh", "Nam", null, null, true, "0912345678", null },
                    { 2, "456 Đường XYZ, Quận 3, TP.HCM", 1, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Nguyễn Thị Bình", "Nữ", null, null, true, "0912345679", null },
                    { 3, null, 1, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1978, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Trần Văn Cường", "Nam", "012345678901", null, true, "0912345680", null },
                    { 4, "Địa chỉ demo 4", 1, "BC000000004", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht4@gmail.com", "Bệnh nhân demo 4", "Nam", "ID000000004", null, true, "090000004", null },
                    { 5, "Địa chỉ demo 5", 1, "BC000000005", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht5@gmail.com", "Bệnh nhân demo 5", "Nữ", "ID000000005", null, true, "090000005", null },
                    { 6, "Địa chỉ demo 6", 1, "BC000000006", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht6@gmail.com", "Bệnh nhân demo 6", "Nam", "ID000000006", null, true, "090000006", null },
                    { 7, "Địa chỉ demo 7", 1, "BC000000007", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht7@gmail.com", "Bệnh nhân demo 7", "Nữ", "ID000000007", null, true, "090000007", null },
                    { 8, "Địa chỉ demo 8", 1, "BC000000008", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht8@gmail.com", "Bệnh nhân demo 8", "Nam", "ID000000008", null, true, "090000008", null },
                    { 9, "Địa chỉ demo 9", 1, "BC000000009", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht9@gmail.com", "Bệnh nhân demo 9", "Nữ", "ID000000009", null, true, "090000009", null },
                    { 10, "Địa chỉ demo 10", 1, "BC000000010", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 1, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht10@gmail.com", "Bệnh nhân demo 10", "Nam", "ID000000010", null, true, "090000010", null },
                    { 11, "Địa chỉ demo 11", 1, "BC000000011", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht11@gmail.com", "Bệnh nhân demo 11", "Nữ", "ID000000011", null, true, "090000011", null },
                    { 12, "Địa chỉ demo 12", 1, "BC000000012", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 1, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht12@gmail.com", "Bệnh nhân demo 12", "Nam", "ID000000012", null, true, "090000012", null },
                    { 13, "Địa chỉ demo 13", 1, "BC000000013", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht13@gmail.com", "Bệnh nhân demo 13", "Nữ", "ID000000013", null, true, "090000013", null },
                    { 14, "Địa chỉ demo 14", 1, "BC000000014", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht14@gmail.com", "Bệnh nhân demo 14", "Nam", "ID000000014", null, true, "090000014", null },
                    { 15, "Địa chỉ demo 15", 1, "BC000000015", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 1, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht15@gmail.com", "Bệnh nhân demo 15", "Nữ", "ID000000015", null, true, "090000015", null },
                    { 16, "Địa chỉ demo 16", 1, "BC000000016", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 1, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht16@gmail.com", "Bệnh nhân demo 16", "Nam", "ID000000016", null, true, "090000016", null },
                    { 17, "Địa chỉ demo 17", 1, "BC000000017", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 1, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht17@gmail.com", "Bệnh nhân demo 17", "Nữ", "ID000000017", null, true, "090000017", null },
                    { 18, "Địa chỉ demo 18", 1, "BC000000018", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 1, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht18@gmail.com", "Bệnh nhân demo 18", "Nam", "ID000000018", null, true, "090000018", null },
                    { 19, "Địa chỉ demo 19", 1, "BC000000019", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht19@gmail.com", "Bệnh nhân demo 19", "Nữ", "ID000000019", null, true, "090000019", null },
                    { 20, "Địa chỉ demo 20", 1, "BC000000020", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 1, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht20@gmail.com", "Bệnh nhân demo 20", "Nam", "ID000000020", null, true, "090000020", null },
                    { 21, "Địa chỉ demo 21", 1, "BC000000021", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 1, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht21@gmail.com", "Bệnh nhân demo 21", "Nữ", "ID000000021", null, true, "090000021", null },
                    { 22, "Địa chỉ demo 22", 1, "BC000000022", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 1, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht22@gmail.com", "Bệnh nhân demo 22", "Nam", "ID000000022", null, true, "090000022", null },
                    { 23, "Địa chỉ demo 23", 1, "BC000000023", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 1, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht23@gmail.com", "Bệnh nhân demo 23", "Nữ", "ID000000023", null, true, "090000023", null },
                    { 24, "Địa chỉ demo 24", 1, "BC000000024", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 1, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht24@gmail.com", "Bệnh nhân demo 24", "Nam", "ID000000024", null, true, "090000024", null },
                    { 25, "Địa chỉ demo 25", 1, "BC000000025", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 1, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht25@gmail.com", "Bệnh nhân demo 25", "Nữ", "ID000000025", null, true, "090000025", null },
                    { 26, "Địa chỉ demo 26", 1, "BC000000026", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 1, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht26@gmail.com", "Bệnh nhân demo 26", "Nam", "ID000000026", null, true, "090000026", null },
                    { 27, "Địa chỉ demo 27", 1, "BC000000027", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 1, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht27@gmail.com", "Bệnh nhân demo 27", "Nữ", "ID000000027", null, true, "090000027", null },
                    { 28, "Địa chỉ demo 28", 1, "BC000000028", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 1, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht28@gmail.com", "Bệnh nhân demo 28", "Nam", "ID000000028", null, true, "090000028", null },
                    { 29, "Địa chỉ demo 29", 1, "BC000000029", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 1, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht29@gmail.com", "Bệnh nhân demo 29", "Nữ", "ID000000029", null, true, "090000029", null },
                    { 30, "Địa chỉ demo 30", 1, "BC000000030", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht30@gmail.com", "Bệnh nhân demo 30", "Nam", "ID000000030", null, true, "090000030", null },
                    { 31, "Địa chỉ demo 31", 1, "BC000000031", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht31@gmail.com", "Bệnh nhân demo 31", "Nữ", "ID000000031", null, true, "090000031", null },
                    { 32, "Địa chỉ demo 32", 1, "BC000000032", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht32@gmail.com", "Bệnh nhân demo 32", "Nam", "ID000000032", null, true, "090000032", null },
                    { 33, "Địa chỉ demo 33", 1, "BC000000033", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 2, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht33@gmail.com", "Bệnh nhân demo 33", "Nữ", "ID000000033", null, true, "090000033", null },
                    { 34, "Địa chỉ demo 34", 1, "BC000000034", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 2, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht34@gmail.com", "Bệnh nhân demo 34", "Nam", "ID000000034", null, true, "090000034", null },
                    { 35, "Địa chỉ demo 35", 1, "BC000000035", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht35@gmail.com", "Bệnh nhân demo 35", "Nữ", "ID000000035", null, true, "090000035", null },
                    { 36, "Địa chỉ demo 36", 1, "BC000000036", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 2, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht36@gmail.com", "Bệnh nhân demo 36", "Nam", "ID000000036", null, true, "090000036", null },
                    { 37, "Địa chỉ demo 37", 1, "BC000000037", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 2, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht37@gmail.com", "Bệnh nhân demo 37", "Nữ", "ID000000037", null, true, "090000037", null },
                    { 38, "Địa chỉ demo 38", 1, "BC000000038", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 2, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht38@gmail.com", "Bệnh nhân demo 38", "Nam", "ID000000038", null, true, "090000038", null },
                    { 39, "Địa chỉ demo 39", 1, "BC000000039", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 2, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht39@gmail.com", "Bệnh nhân demo 39", "Nữ", "ID000000039", null, true, "090000039", null },
                    { 40, "Địa chỉ demo 40", 1, "BC000000040", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht40@gmail.com", "Bệnh nhân demo 40", "Nam", "ID000000040", null, true, "090000040", null },
                    { 41, "Địa chỉ demo 41", 1, "BC000000041", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 2, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht41@gmail.com", "Bệnh nhân demo 41", "Nữ", "ID000000041", null, true, "090000041", null },
                    { 42, "Địa chỉ demo 42", 1, "BC000000042", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 2, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht42@gmail.com", "Bệnh nhân demo 42", "Nam", "ID000000042", null, true, "090000042", null },
                    { 43, "Địa chỉ demo 43", 1, "BC000000043", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 2, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht43@gmail.com", "Bệnh nhân demo 43", "Nữ", "ID000000043", null, true, "090000043", null },
                    { 44, "Địa chỉ demo 44", 1, "BC000000044", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 2, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht44@gmail.com", "Bệnh nhân demo 44", "Nam", "ID000000044", null, true, "090000044", null },
                    { 45, "Địa chỉ demo 45", 1, "BC000000045", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht45@gmail.com", "Bệnh nhân demo 45", "Nữ", "ID000000045", null, true, "090000045", null },
                    { 46, "Địa chỉ demo 46", 1, "BC000000046", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 2, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht46@gmail.com", "Bệnh nhân demo 46", "Nam", "ID000000046", null, true, "090000046", null },
                    { 47, "Địa chỉ demo 47", 1, "BC000000047", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 2, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht47@gmail.com", "Bệnh nhân demo 47", "Nữ", "ID000000047", null, true, "090000047", null },
                    { 48, "Địa chỉ demo 48", 1, "BC000000048", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 2, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht48@gmail.com", "Bệnh nhân demo 48", "Nam", "ID000000048", null, true, "090000048", null },
                    { 49, "Địa chỉ demo 49", 1, "BC000000049", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 2, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht49@gmail.com", "Bệnh nhân demo 49", "Nữ", "ID000000049", null, true, "090000049", null },
                    { 50, "Địa chỉ demo 50", 1, "BC000000050", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht50@gmail.com", "Bệnh nhân demo 50", "Nam", "ID000000050", null, true, "090000050", null },
                    { 51, "Địa chỉ demo 51", 1, "BC000000051", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 2, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht51@gmail.com", "Bệnh nhân demo 51", "Nữ", "ID000000051", null, true, "090000051", null },
                    { 52, "Địa chỉ demo 52", 1, "BC000000052", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 2, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht52@gmail.com", "Bệnh nhân demo 52", "Nam", "ID000000052", null, true, "090000052", null },
                    { 53, "Địa chỉ demo 53", 1, "BC000000053", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 2, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht53@gmail.com", "Bệnh nhân demo 53", "Nữ", "ID000000053", null, true, "090000053", null },
                    { 54, "Địa chỉ demo 54", 1, "BC000000054", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 2, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht54@gmail.com", "Bệnh nhân demo 54", "Nam", "ID000000054", null, true, "090000054", null },
                    { 55, "Địa chỉ demo 55", 1, "BC000000055", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 2, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht55@gmail.com", "Bệnh nhân demo 55", "Nữ", "ID000000055", null, true, "090000055", null },
                    { 56, "Địa chỉ demo 56", 1, "BC000000056", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 2, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht56@gmail.com", "Bệnh nhân demo 56", "Nam", "ID000000056", null, true, "090000056", null },
                    { 57, "Địa chỉ demo 57", 1, "BC000000057", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 2, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht57@gmail.com", "Bệnh nhân demo 57", "Nữ", "ID000000057", null, true, "090000057", null },
                    { 58, "Địa chỉ demo 58", 1, "BC000000058", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 2, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht58@gmail.com", "Bệnh nhân demo 58", "Nam", "ID000000058", null, true, "090000058", null },
                    { 59, "Địa chỉ demo 59", 1, "BC000000059", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht59@gmail.com", "Bệnh nhân demo 59", "Nữ", "ID000000059", null, true, "090000059", null },
                    { 60, "Địa chỉ demo 60", 1, "BC000000060", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht60@gmail.com", "Bệnh nhân demo 60", "Nam", "ID000000060", null, true, "090000060", null },
                    { 61, "Địa chỉ demo 61", 1, "BC000000061", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht61@gmail.com", "Bệnh nhân demo 61", "Nữ", "ID000000061", null, true, "090000061", null },
                    { 62, "Địa chỉ demo 62", 1, "BC000000062", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht62@gmail.com", "Bệnh nhân demo 62", "Nam", "ID000000062", null, true, "090000062", null },
                    { 63, "Địa chỉ demo 63", 1, "BC000000063", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht63@gmail.com", "Bệnh nhân demo 63", "Nữ", "ID000000063", null, true, "090000063", null },
                    { 64, "Địa chỉ demo 64", 1, "BC000000064", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht64@gmail.com", "Bệnh nhân demo 64", "Nam", "ID000000064", null, true, "090000064", null },
                    { 65, "Địa chỉ demo 65", 1, "BC000000065", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht65@gmail.com", "Bệnh nhân demo 65", "Nữ", "ID000000065", null, true, "090000065", null },
                    { 66, "Địa chỉ demo 66", 1, "BC000000066", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht66@gmail.com", "Bệnh nhân demo 66", "Nam", "ID000000066", null, true, "090000066", null },
                    { 67, "Địa chỉ demo 67", 1, "BC000000067", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht67@gmail.com", "Bệnh nhân demo 67", "Nữ", "ID000000067", null, true, "090000067", null },
                    { 68, "Địa chỉ demo 68", 1, "BC000000068", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht68@gmail.com", "Bệnh nhân demo 68", "Nam", "ID000000068", null, true, "090000068", null },
                    { 69, "Địa chỉ demo 69", 1, "BC000000069", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht69@gmail.com", "Bệnh nhân demo 69", "Nữ", "ID000000069", null, true, "090000069", null },
                    { 70, "Địa chỉ demo 70", 1, "BC000000070", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht70@gmail.com", "Bệnh nhân demo 70", "Nam", "ID000000070", null, true, "090000070", null },
                    { 71, "Địa chỉ demo 71", 1, "BC000000071", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht71@gmail.com", "Bệnh nhân demo 71", "Nữ", "ID000000071", null, true, "090000071", null },
                    { 72, "Địa chỉ demo 72", 1, "BC000000072", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht72@gmail.com", "Bệnh nhân demo 72", "Nam", "ID000000072", null, true, "090000072", null },
                    { 73, "Địa chỉ demo 73", 1, "BC000000073", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht73@gmail.com", "Bệnh nhân demo 73", "Nữ", "ID000000073", null, true, "090000073", null },
                    { 74, "Địa chỉ demo 74", 1, "BC000000074", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht74@gmail.com", "Bệnh nhân demo 74", "Nam", "ID000000074", null, true, "090000074", null },
                    { 75, "Địa chỉ demo 75", 1, "BC000000075", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht75@gmail.com", "Bệnh nhân demo 75", "Nữ", "ID000000075", null, true, "090000075", null },
                    { 76, "Địa chỉ demo 76", 1, "BC000000076", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht76@gmail.com", "Bệnh nhân demo 76", "Nam", "ID000000076", null, true, "090000076", null },
                    { 77, "Địa chỉ demo 77", 1, "BC000000077", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht77@gmail.com", "Bệnh nhân demo 77", "Nữ", "ID000000077", null, true, "090000077", null },
                    { 78, "Địa chỉ demo 78", 1, "BC000000078", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht78@gmail.com", "Bệnh nhân demo 78", "Nam", "ID000000078", null, true, "090000078", null },
                    { 79, "Địa chỉ demo 79", 1, "BC000000079", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht79@gmail.com", "Bệnh nhân demo 79", "Nữ", "ID000000079", null, true, "090000079", null },
                    { 80, "Địa chỉ demo 80", 1, "BC000000080", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht80@gmail.com", "Bệnh nhân demo 80", "Nam", "ID000000080", null, true, "090000080", null },
                    { 81, "Địa chỉ demo 81", 1, "BC000000081", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht81@gmail.com", "Bệnh nhân demo 81", "Nữ", "ID000000081", null, true, "090000081", null },
                    { 82, "Địa chỉ demo 82", 1, "BC000000082", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht82@gmail.com", "Bệnh nhân demo 82", "Nam", "ID000000082", null, true, "090000082", null },
                    { 83, "Địa chỉ demo 83", 1, "BC000000083", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht83@gmail.com", "Bệnh nhân demo 83", "Nữ", "ID000000083", null, true, "090000083", null },
                    { 84, "Địa chỉ demo 84", 1, "BC000000084", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht84@gmail.com", "Bệnh nhân demo 84", "Nam", "ID000000084", null, true, "090000084", null },
                    { 85, "Địa chỉ demo 85", 1, "BC000000085", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht85@gmail.com", "Bệnh nhân demo 85", "Nữ", "ID000000085", null, true, "090000085", null },
                    { 86, "Địa chỉ demo 86", 1, "BC000000086", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht86@gmail.com", "Bệnh nhân demo 86", "Nam", "ID000000086", null, true, "090000086", null },
                    { 87, "Địa chỉ demo 87", 1, "BC000000087", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht87@gmail.com", "Bệnh nhân demo 87", "Nữ", "ID000000087", null, true, "090000087", null },
                    { 88, "Địa chỉ demo 88", 1, "BC000000088", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht88@gmail.com", "Bệnh nhân demo 88", "Nam", "ID000000088", null, true, "090000088", null },
                    { 89, "Địa chỉ demo 89", 1, "BC000000089", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 3, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht89@gmail.com", "Bệnh nhân demo 89", "Nữ", "ID000000089", null, true, "090000089", null },
                    { 90, "Địa chỉ demo 90", 1, "BC000000090", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht90@gmail.com", "Bệnh nhân demo 90", "Nam", "ID000000090", null, true, "090000090", null },
                    { 91, "Địa chỉ demo 91", 1, "BC000000091", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 4, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht91@gmail.com", "Bệnh nhân demo 91", "Nữ", "ID000000091", null, true, "090000091", null },
                    { 92, "Địa chỉ demo 92", 1, "BC000000092", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 4, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht92@gmail.com", "Bệnh nhân demo 92", "Nam", "ID000000092", null, true, "090000092", null },
                    { 93, "Địa chỉ demo 93", 1, "BC000000093", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht93@gmail.com", "Bệnh nhân demo 93", "Nữ", "ID000000093", null, true, "090000093", null },
                    { 94, "Địa chỉ demo 94", 1, "BC000000094", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht94@gmail.com", "Bệnh nhân demo 94", "Nam", "ID000000094", null, true, "090000094", null },
                    { 95, "Địa chỉ demo 95", 1, "BC000000095", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 4, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht95@gmail.com", "Bệnh nhân demo 95", "Nữ", "ID000000095", null, true, "090000095", null },
                    { 96, "Địa chỉ demo 96", 1, "BC000000096", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 4, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht96@gmail.com", "Bệnh nhân demo 96", "Nam", "ID000000096", null, true, "090000096", null },
                    { 97, "Địa chỉ demo 97", 1, "BC000000097", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 4, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht97@gmail.com", "Bệnh nhân demo 97", "Nữ", "ID000000097", null, true, "090000097", null },
                    { 98, "Địa chỉ demo 98", 1, "BC000000098", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 4, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht98@gmail.com", "Bệnh nhân demo 98", "Nam", "ID000000098", null, true, "090000098", null },
                    { 99, "Địa chỉ demo 99", 1, "BC000000099", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 4, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht99@gmail.com", "Bệnh nhân demo 99", "Nữ", "ID000000099", null, true, "090000099", null },
                    { 100, "Địa chỉ demo 100", 1, "BC000000100", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 4, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "dominht100@gmail.com", "Bệnh nhân demo 100", "Nam", "ID000000100", null, true, "090000100", null }
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
