using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Clinic.Services.Migrations
{
    /// <inheritdoc />
    public partial class AddReminderTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReminderTasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    TaskType = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DueDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    IsDone = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DoneByStaffId = table.Column<int>(type: "int", nullable: true),
                    DoneAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReminderTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReminderTasks_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "ReminderTasks",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Description", "DoneAt", "DoneByStaffId", "DueDate", "IsDone", "PatientId", "Priority", "TaskType" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 12, 27, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Siêu âm gan 6 tháng (viêm gan B mạn)", null, null, new DateTime(2024, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), false, 7, 1, 3 },
                    { 2, new DateTime(2023, 12, 20, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Kiểm tra sau phẫu thuật tim (1 tháng)", new DateTime(2023, 12, 28, 11, 0, 0, 0, DateTimeKind.Utc), 2, new DateTime(2023, 12, 28, 0, 0, 0, 0, DateTimeKind.Utc), true, 12, 2, 0 },
                    { 3, new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Kiểm tra sau phẫu thuật tim (1 tháng)", null, null, new DateTime(2023, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), false, 29, 2, 0 },
                    { 4, new DateTime(2024, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Gửi kết quả xét nghiệm qua email", new DateTime(2024, 1, 8, 12, 0, 0, 0, DateTimeKind.Utc), 3, new DateTime(2024, 1, 8, 0, 0, 0, 0, DateTimeKind.Utc), true, 3, 0, 4 },
                    { 5, new DateTime(2023, 12, 15, 0, 0, 0, 0, DateTimeKind.Utc), -1, "Gửi thiệp chúc mừng sinh nhật qua email", new DateTime(2023, 12, 22, 14, 0, 0, 0, DateTimeKind.Utc), 4, new DateTime(2023, 12, 22, 0, 0, 0, 0, DateTimeKind.Utc), true, 42, 0, 1 },
                    { 6, new DateTime(2024, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Tái khám sau phẫu thuật ruột thừa (7 ngày)", null, null, new DateTime(2024, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), false, 3, 2, 0 },
                    { 7, new DateTime(2023, 12, 15, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Xét nghiệm chức năng tuyến giáp 6 tháng", new DateTime(2023, 12, 25, 8, 0, 0, 0, DateTimeKind.Utc), 3, new DateTime(2023, 12, 25, 0, 0, 0, 0, DateTimeKind.Utc), true, 33, 1, 3 },
                    { 8, new DateTime(2023, 12, 28, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Siêu âm gan 6 tháng (viêm gan B mạn)", null, null, new DateTime(2023, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), false, 1, 1, 3 },
                    { 9, new DateTime(2023, 12, 20, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Tiêm nhắc lại mũi 3 - Viêm gan B", null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false, 40, 1, 2 },
                    { 10, new DateTime(2023, 12, 25, 0, 0, 0, 0, DateTimeKind.Utc), 4, "Tái khám sau phẫu thuật ruột thừa (7 ngày)", null, null, new DateTime(2024, 1, 7, 0, 0, 0, 0, DateTimeKind.Utc), false, 4, 2, 0 },
                    { 11, new DateTime(2023, 12, 12, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Tiêm vaccine COVID-19 mũi tăng cường", null, null, new DateTime(2023, 12, 23, 0, 0, 0, 0, DateTimeKind.Utc), false, 31, 1, 2 },
                    { 12, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Tiêm vaccine COVID-19 mũi tăng cường", null, null, new DateTime(2024, 1, 13, 0, 0, 0, 0, DateTimeKind.Utc), false, 15, 1, 2 },
                    { 13, new DateTime(2023, 12, 22, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Cập nhật thông tin bảo hiểm y tế", new DateTime(2023, 12, 26, 14, 0, 0, 0, DateTimeKind.Utc), 2, new DateTime(2023, 12, 26, 0, 0, 0, 0, DateTimeKind.Utc), true, 36, 0, 4 },
                    { 14, new DateTime(2024, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), -1, "Gửi thiệp chúc mừng sinh nhật qua email", null, null, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), false, 26, 0, 1 },
                    { 15, new DateTime(2024, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Xét nghiệm chức năng tuyến giáp 6 tháng", null, null, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), false, 28, 2, 3 },
                    { 16, new DateTime(2023, 12, 15, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Kiểm tra vết mổ sau 14 ngày - Phẫu thuật gối", null, null, new DateTime(2023, 12, 22, 0, 0, 0, 0, DateTimeKind.Utc), false, 14, 2, 0 },
                    { 17, new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Utc), 4, "Follow-up sau cắt amidan (10 ngày)", new DateTime(2023, 12, 24, 14, 0, 0, 0, DateTimeKind.Utc), 2, new DateTime(2023, 12, 24, 0, 0, 0, 0, DateTimeKind.Utc), true, 7, 2, 0 },
                    { 18, new DateTime(2023, 12, 18, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Kiểm tra sau phẫu thuật tim (1 tháng)", new DateTime(2023, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), 4, new DateTime(2023, 12, 27, 0, 0, 0, 0, DateTimeKind.Utc), true, 45, 2, 0 },
                    { 19, new DateTime(2023, 12, 13, 0, 0, 0, 0, DateTimeKind.Utc), 4, "Siêu âm gan 6 tháng (viêm gan B mạn)", null, null, new DateTime(2023, 12, 26, 0, 0, 0, 0, DateTimeKind.Utc), false, 15, 1, 3 },
                    { 20, new DateTime(2023, 12, 17, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Siêu âm gan 6 tháng (viêm gan B mạn)", new DateTime(2023, 12, 26, 14, 0, 0, 0, DateTimeKind.Utc), 3, new DateTime(2023, 12, 26, 0, 0, 0, 0, DateTimeKind.Utc), true, 45, 1, 3 },
                    { 21, new DateTime(2023, 12, 17, 0, 0, 0, 0, DateTimeKind.Utc), -1, "Gọi điện chúc mừng sinh nhật", new DateTime(2023, 12, 30, 12, 0, 0, 0, DateTimeKind.Utc), 2, new DateTime(2023, 12, 30, 0, 0, 0, 0, DateTimeKind.Utc), true, 23, 0, 1 },
                    { 22, new DateTime(2023, 12, 15, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Nhắc bệnh nhân uống thuốc đúng liều", new DateTime(2023, 12, 22, 11, 0, 0, 0, DateTimeKind.Utc), 3, new DateTime(2023, 12, 22, 0, 0, 0, 0, DateTimeKind.Utc), true, 6, 0, 4 },
                    { 23, new DateTime(2024, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Follow-up sau cắt amidan (10 ngày)", null, null, new DateTime(2024, 1, 13, 0, 0, 0, 0, DateTimeKind.Utc), false, 49, 2, 0 },
                    { 24, new DateTime(2023, 12, 29, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Đo mật độ xương định kỳ", null, null, new DateTime(2024, 1, 9, 0, 0, 0, 0, DateTimeKind.Utc), false, 42, 1, 3 },
                    { 25, new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Utc), -1, "Gọi điện chúc mừng sinh nhật", null, null, new DateTime(2023, 12, 26, 0, 0, 0, 0, DateTimeKind.Utc), false, 34, 0, 1 },
                    { 26, new DateTime(2023, 12, 24, 0, 0, 0, 0, DateTimeKind.Utc), 4, "Liên hệ bệnh nhân xác nhận lịch hẹn", new DateTime(2024, 1, 1, 16, 0, 0, 0, DateTimeKind.Utc), 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, 5, 2, 4 },
                    { 27, new DateTime(2023, 12, 26, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Nhắc bệnh nhân mang theo hồ sơ cũ", null, null, new DateTime(2024, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), false, 8, 0, 4 },
                    { 28, new DateTime(2023, 12, 27, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Kiểm tra sau phẫu thuật tim (1 tháng)", null, null, new DateTime(2024, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), false, 18, 2, 0 },
                    { 29, new DateTime(2024, 1, 8, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Xét nghiệm mỡ máu định kỳ 6 tháng", null, null, new DateTime(2024, 1, 14, 0, 0, 0, 0, DateTimeKind.Utc), false, 10, 1, 3 },
                    { 30, new DateTime(2024, 1, 6, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Tái khám sau mổ sỏi thận (1 tháng)", null, null, new DateTime(2024, 1, 13, 0, 0, 0, 0, DateTimeKind.Utc), false, 14, 2, 0 },
                    { 31, new DateTime(2023, 12, 17, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Chụp X-quang phổi định kỳ hàng năm", null, null, new DateTime(2023, 12, 30, 0, 0, 0, 0, DateTimeKind.Utc), false, 50, 2, 3 },
                    { 32, new DateTime(2024, 1, 8, 0, 0, 0, 0, DateTimeKind.Utc), 4, "Tiêm phòng uốn ván nhắc lại", null, null, new DateTime(2024, 1, 14, 0, 0, 0, 0, DateTimeKind.Utc), false, 20, 0, 2 },
                    { 33, new DateTime(2023, 12, 12, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Nhắc bệnh nhân mang theo hồ sơ cũ", new DateTime(2023, 12, 22, 16, 0, 0, 0, DateTimeKind.Utc), 1, new DateTime(2023, 12, 22, 0, 0, 0, 0, DateTimeKind.Utc), true, 45, 0, 4 },
                    { 34, new DateTime(2024, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Đo mật độ xương định kỳ", null, null, new DateTime(2024, 1, 14, 0, 0, 0, 0, DateTimeKind.Utc), false, 15, 2, 3 },
                    { 35, new DateTime(2023, 12, 22, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Tái khám sau điều trị viêm phổi", null, null, new DateTime(2023, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), false, 27, 2, 0 },
                    { 36, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4, "Xét nghiệm mỡ máu định kỳ 6 tháng", null, null, new DateTime(2024, 1, 7, 0, 0, 0, 0, DateTimeKind.Utc), false, 17, 1, 3 },
                    { 37, new DateTime(2023, 12, 26, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Cập nhật thông tin bảo hiểm y tế", null, null, new DateTime(2024, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), false, 1, 1, 4 },
                    { 38, new DateTime(2023, 12, 28, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Follow-up sau cắt amidan (10 ngày)", null, null, new DateTime(2024, 1, 7, 0, 0, 0, 0, DateTimeKind.Utc), false, 10, 2, 0 },
                    { 39, new DateTime(2023, 12, 23, 0, 0, 0, 0, DateTimeKind.Utc), 4, "Cập nhật thông tin bảo hiểm y tế", null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false, 31, 1, 4 },
                    { 40, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Tiêm vaccine phế cầu khuẩn", null, null, new DateTime(2024, 1, 7, 0, 0, 0, 0, DateTimeKind.Utc), false, 21, 2, 2 },
                    { 41, new DateTime(2023, 12, 17, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Tiêm vaccine phế cầu khuẩn", new DateTime(2023, 12, 29, 16, 0, 0, 0, DateTimeKind.Utc), 2, new DateTime(2023, 12, 29, 0, 0, 0, 0, DateTimeKind.Utc), true, 35, 1, 2 },
                    { 42, new DateTime(2023, 12, 18, 0, 0, 0, 0, DateTimeKind.Utc), 4, "Tái khám sau phẫu thuật mắt (2 tuần)", new DateTime(2023, 12, 26, 8, 0, 0, 0, DateTimeKind.Utc), 2, new DateTime(2023, 12, 26, 0, 0, 0, 0, DateTimeKind.Utc), true, 22, 2, 0 },
                    { 43, new DateTime(2023, 12, 27, 0, 0, 0, 0, DateTimeKind.Utc), -1, "Chúc mừng sinh nhật bệnh nhân", null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false, 36, 0, 1 },
                    { 44, new DateTime(2024, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), 4, "Nội soi đại tràng định kỳ (1 năm)", null, null, new DateTime(2024, 1, 14, 0, 0, 0, 0, DateTimeKind.Utc), false, 46, 2, 3 },
                    { 45, new DateTime(2023, 12, 22, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Chụp X-quang phổi định kỳ hàng năm", null, null, new DateTime(2024, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), false, 43, 1, 3 },
                    { 46, new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Đo mật độ xương định kỳ", null, null, new DateTime(2023, 12, 25, 0, 0, 0, 0, DateTimeKind.Utc), false, 29, 1, 3 },
                    { 47, new DateTime(2023, 12, 18, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Tái khám sau điều trị viêm phổi", new DateTime(2023, 12, 27, 15, 0, 0, 0, DateTimeKind.Utc), 4, new DateTime(2023, 12, 27, 0, 0, 0, 0, DateTimeKind.Utc), true, 2, 2, 0 },
                    { 48, new DateTime(2023, 12, 25, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Kiểm tra sau phẫu thuật tim (1 tháng)", null, null, new DateTime(2024, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), false, 15, 2, 0 },
                    { 49, new DateTime(2023, 12, 25, 0, 0, 0, 0, DateTimeKind.Utc), 4, "Kiểm tra hồi phục sau nội soi dạ dày", null, null, new DateTime(2024, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), false, 40, 2, 0 },
                    { 50, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4, "Cập nhật thông tin bảo hiểm y tế", null, null, new DateTime(2024, 1, 12, 0, 0, 0, 0, DateTimeKind.Utc), false, 21, 2, 4 },
                    { 51, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Xét nghiệm máu định kỳ 3 tháng (tiểu đường)", null, null, new DateTime(2024, 1, 14, 0, 0, 0, 0, DateTimeKind.Utc), false, 40, 2, 3 },
                    { 52, new DateTime(2024, 1, 7, 0, 0, 0, 0, DateTimeKind.Utc), 4, "Nhắc bệnh nhân mang theo hồ sơ cũ", null, null, new DateTime(2024, 1, 13, 0, 0, 0, 0, DateTimeKind.Utc), false, 23, 0, 4 },
                    { 53, new DateTime(2023, 12, 10, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Tiêm nhắc lại sởi - quai bị - rubella", new DateTime(2023, 12, 23, 11, 0, 0, 0, DateTimeKind.Utc), 4, new DateTime(2023, 12, 23, 0, 0, 0, 0, DateTimeKind.Utc), true, 46, 2, 2 },
                    { 54, new DateTime(2024, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Follow-up sau cắt amidan (10 ngày)", null, null, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), false, 26, 2, 0 },
                    { 55, new DateTime(2024, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), -1, "Gửi lời chúc sinh nhật", null, null, new DateTime(2024, 1, 9, 0, 0, 0, 0, DateTimeKind.Utc), false, 10, 0, 1 },
                    { 56, new DateTime(2024, 1, 7, 0, 0, 0, 0, DateTimeKind.Utc), 4, "Tiêm vaccine COVID-19 mũi tăng cường", null, null, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), false, 29, 2, 2 },
                    { 57, new DateTime(2023, 12, 24, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Xét nghiệm mỡ máu định kỳ 6 tháng", new DateTime(2023, 12, 29, 16, 0, 0, 0, DateTimeKind.Utc), 4, new DateTime(2023, 12, 29, 0, 0, 0, 0, DateTimeKind.Utc), true, 3, 2, 3 },
                    { 58, new DateTime(2023, 12, 18, 0, 0, 0, 0, DateTimeKind.Utc), 4, "Tiêm vaccine phế cầu khuẩn", new DateTime(2023, 12, 23, 15, 0, 0, 0, DateTimeKind.Utc), 4, new DateTime(2023, 12, 23, 0, 0, 0, 0, DateTimeKind.Utc), true, 44, 2, 2 },
                    { 59, new DateTime(2023, 12, 27, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Nhắc bệnh nhân uống thuốc đúng liều", null, null, new DateTime(2024, 1, 6, 0, 0, 0, 0, DateTimeKind.Utc), false, 32, 1, 4 },
                    { 60, new DateTime(2023, 12, 16, 0, 0, 0, 0, DateTimeKind.Utc), -1, "Chúc mừng sinh nhật bệnh nhân", null, null, new DateTime(2023, 12, 24, 0, 0, 0, 0, DateTimeKind.Utc), false, 8, 0, 1 },
                    { 61, new DateTime(2024, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), 4, "Tiêm vaccine COVID-19 mũi tăng cường", null, null, new DateTime(2024, 1, 8, 0, 0, 0, 0, DateTimeKind.Utc), false, 24, 0, 2 },
                    { 62, new DateTime(2023, 12, 25, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Nhắc bệnh nhân mang theo hồ sơ cũ", null, null, new DateTime(2023, 12, 29, 0, 0, 0, 0, DateTimeKind.Utc), false, 17, 0, 4 },
                    { 63, new DateTime(2023, 12, 27, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Follow-up sau cắt amidan (10 ngày)", null, null, new DateTime(2024, 1, 9, 0, 0, 0, 0, DateTimeKind.Utc), false, 20, 2, 0 },
                    { 64, new DateTime(2023, 12, 29, 0, 0, 0, 0, DateTimeKind.Utc), 4, "Tiêm vaccine phế cầu khuẩn", null, null, new DateTime(2024, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), false, 6, 0, 2 },
                    { 65, new DateTime(2023, 12, 22, 0, 0, 0, 0, DateTimeKind.Utc), -1, "Gửi thiệp chúc mừng sinh nhật qua email", null, null, new DateTime(2024, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), false, 19, 0, 1 },
                    { 66, new DateTime(2023, 12, 23, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Tái khám sau điều trị viêm phổi", null, null, new DateTime(2024, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), false, 14, 2, 0 },
                    { 67, new DateTime(2023, 12, 19, 0, 0, 0, 0, DateTimeKind.Utc), -1, "Gửi lời chúc sinh nhật", null, null, new DateTime(2023, 12, 23, 0, 0, 0, 0, DateTimeKind.Utc), false, 25, 0, 1 },
                    { 68, new DateTime(2023, 12, 28, 0, 0, 0, 0, DateTimeKind.Utc), -1, "Gửi lời chúc sinh nhật", new DateTime(2023, 12, 31, 11, 0, 0, 0, DateTimeKind.Utc), 1, new DateTime(2023, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), true, 37, 0, 1 },
                    { 69, new DateTime(2023, 12, 24, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Nhắc bệnh nhân uống thuốc đúng liều", new DateTime(2024, 1, 5, 8, 0, 0, 0, DateTimeKind.Utc), 4, new DateTime(2024, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), true, 8, 2, 4 },
                    { 70, new DateTime(2024, 1, 12, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Nhắc bệnh nhân uống thuốc đúng liều", null, null, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), false, 7, 0, 4 },
                    { 71, new DateTime(2024, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Kiểm tra vết mổ sau 14 ngày - Phẫu thuật gối", null, null, new DateTime(2024, 1, 7, 0, 0, 0, 0, DateTimeKind.Utc), false, 8, 2, 0 },
                    { 72, new DateTime(2024, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Tiêm vaccine phế cầu khuẩn", null, null, new DateTime(2024, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), false, 45, 1, 2 },
                    { 73, new DateTime(2023, 12, 15, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Cập nhật thông tin bảo hiểm y tế", new DateTime(2023, 12, 24, 15, 0, 0, 0, DateTimeKind.Utc), 2, new DateTime(2023, 12, 24, 0, 0, 0, 0, DateTimeKind.Utc), true, 30, 2, 4 },
                    { 74, new DateTime(2023, 12, 11, 0, 0, 0, 0, DateTimeKind.Utc), 4, "Tiêm phòng cúm mùa hàng năm", new DateTime(2023, 12, 23, 8, 0, 0, 0, DateTimeKind.Utc), 3, new DateTime(2023, 12, 23, 0, 0, 0, 0, DateTimeKind.Utc), true, 3, 0, 2 },
                    { 75, new DateTime(2023, 12, 28, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Follow-up sau cắt amidan (10 ngày)", null, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false, 11, 2, 0 },
                    { 76, new DateTime(2023, 12, 17, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Follow-up sau cắt amidan (10 ngày)", new DateTime(2023, 12, 27, 8, 0, 0, 0, DateTimeKind.Utc), 2, new DateTime(2023, 12, 27, 0, 0, 0, 0, DateTimeKind.Utc), true, 24, 2, 0 },
                    { 77, new DateTime(2023, 12, 22, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Xét nghiệm máu định kỳ 3 tháng (tiểu đường)", new DateTime(2023, 12, 27, 8, 0, 0, 0, DateTimeKind.Utc), 4, new DateTime(2023, 12, 27, 0, 0, 0, 0, DateTimeKind.Utc), true, 19, 2, 3 },
                    { 78, new DateTime(2024, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Tái khám sau phẫu thuật mắt (2 tuần)", new DateTime(2024, 1, 15, 14, 0, 0, 0, DateTimeKind.Utc), 2, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), true, 25, 2, 0 },
                    { 79, new DateTime(2023, 12, 25, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Tái khám sau phẫu thuật ruột thừa (7 ngày)", null, null, new DateTime(2023, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), false, 9, 2, 0 },
                    { 80, new DateTime(2023, 12, 18, 0, 0, 0, 0, DateTimeKind.Utc), 4, "Liên hệ bệnh nhân xác nhận lịch hẹn", null, null, new DateTime(2023, 12, 26, 0, 0, 0, 0, DateTimeKind.Utc), false, 21, 2, 4 },
                    { 81, new DateTime(2023, 12, 25, 0, 0, 0, 0, DateTimeKind.Utc), -1, "Gửi thiệp chúc mừng sinh nhật qua email", null, null, new DateTime(2024, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), false, 37, 0, 1 },
                    { 82, new DateTime(2023, 12, 30, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Nhắc bệnh nhân uống thuốc đúng liều", null, null, new DateTime(2024, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), false, 19, 1, 4 },
                    { 83, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Tiêm nhắc lại sởi - quai bị - rubella", null, null, new DateTime(2024, 1, 9, 0, 0, 0, 0, DateTimeKind.Utc), false, 38, 0, 2 },
                    { 84, new DateTime(2023, 12, 19, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Đo mật độ xương định kỳ", null, null, new DateTime(2023, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), false, 27, 2, 3 },
                    { 85, new DateTime(2024, 1, 11, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Tiêm nhắc lại mũi 3 - Viêm gan B", null, null, new DateTime(2024, 1, 14, 0, 0, 0, 0, DateTimeKind.Utc), false, 30, 2, 2 },
                    { 86, new DateTime(2024, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Tiêm nhắc lại mũi 3 - Viêm gan B", new DateTime(2024, 1, 13, 16, 0, 0, 0, DateTimeKind.Utc), 4, new DateTime(2024, 1, 13, 0, 0, 0, 0, DateTimeKind.Utc), true, 6, 2, 2 },
                    { 87, new DateTime(2023, 12, 13, 0, 0, 0, 0, DateTimeKind.Utc), -1, "Chúc mừng sinh nhật bệnh nhân", new DateTime(2023, 12, 23, 16, 0, 0, 0, DateTimeKind.Utc), 4, new DateTime(2023, 12, 23, 0, 0, 0, 0, DateTimeKind.Utc), true, 22, 0, 1 },
                    { 88, new DateTime(2024, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Liên hệ bệnh nhân xác nhận lịch hẹn", null, null, new DateTime(2024, 1, 11, 0, 0, 0, 0, DateTimeKind.Utc), false, 3, 0, 4 },
                    { 89, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Tái khám sau phẫu thuật mắt (2 tuần)", null, null, new DateTime(2024, 1, 8, 0, 0, 0, 0, DateTimeKind.Utc), false, 6, 2, 0 },
                    { 90, new DateTime(2023, 12, 12, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Xét nghiệm chức năng tuyến giáp 6 tháng", new DateTime(2023, 12, 22, 16, 0, 0, 0, DateTimeKind.Utc), 3, new DateTime(2023, 12, 22, 0, 0, 0, 0, DateTimeKind.Utc), true, 5, 2, 3 },
                    { 91, new DateTime(2023, 12, 30, 0, 0, 0, 0, DateTimeKind.Utc), 4, "Nhắc bệnh nhân mang theo hồ sơ cũ", null, null, new DateTime(2024, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), false, 9, 1, 4 },
                    { 92, new DateTime(2023, 12, 16, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Tái khám sau phẫu thuật mắt (2 tuần)", new DateTime(2023, 12, 22, 10, 0, 0, 0, DateTimeKind.Utc), 4, new DateTime(2023, 12, 22, 0, 0, 0, 0, DateTimeKind.Utc), true, 38, 2, 0 },
                    { 93, new DateTime(2024, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Gửi kết quả xét nghiệm qua email", null, null, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), false, 44, 1, 4 },
                    { 94, new DateTime(2023, 12, 19, 0, 0, 0, 0, DateTimeKind.Utc), 4, "Chụp X-quang phổi định kỳ hàng năm", null, null, new DateTime(2023, 12, 27, 0, 0, 0, 0, DateTimeKind.Utc), false, 38, 2, 3 },
                    { 95, new DateTime(2023, 12, 26, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Tái khám sau mổ sỏi thận (1 tháng)", null, null, new DateTime(2023, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), false, 44, 2, 0 },
                    { 96, new DateTime(2023, 12, 30, 0, 0, 0, 0, DateTimeKind.Utc), -1, "Gửi thiệp chúc mừng sinh nhật qua email", null, null, new DateTime(2024, 1, 8, 0, 0, 0, 0, DateTimeKind.Utc), false, 8, 0, 1 },
                    { 97, new DateTime(2023, 12, 25, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Xét nghiệm máu định kỳ 3 tháng (tiểu đường)", new DateTime(2023, 12, 30, 13, 0, 0, 0, DateTimeKind.Utc), 2, new DateTime(2023, 12, 30, 0, 0, 0, 0, DateTimeKind.Utc), true, 35, 1, 3 },
                    { 98, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4, "Nhắc bệnh nhân uống thuốc đúng liều", null, null, new DateTime(2024, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), false, 35, 1, 4 },
                    { 99, new DateTime(2023, 12, 21, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Follow-up sau cắt amidan (10 ngày)", null, null, new DateTime(2024, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), false, 27, 2, 0 },
                    { 100, new DateTime(2023, 12, 30, 0, 0, 0, 0, DateTimeKind.Utc), -1, "Gửi lời chúc sinh nhật", null, null, new DateTime(2024, 1, 6, 0, 0, 0, 0, DateTimeKind.Utc), false, 16, 0, 1 }
                });

            migrationBuilder.UpdateData(
                table: "UserAccounts",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "ISPSLQ2SjKZRO9mYqjJxsiDDSQy6p1eKguL21HHoa5rQ6rBHR4TyEBlfyhFcnTnL");

            migrationBuilder.UpdateData(
                table: "UserAccounts",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "H9Ti9sExvxnMDbK3t+ei9/ujR7/EAbTkePe7VEbg2OpKJiODtDlzr6LcvHSM+4gt");

            migrationBuilder.UpdateData(
                table: "UserAccounts",
                keyColumn: "Id",
                keyValue: 3,
                column: "PasswordHash",
                value: "Vabj0LIpvJB5hNryURYx+yQvi2AJdE/6RVUrAIaI624k37ITx4a0ljGcCpGGjSIs");

            migrationBuilder.UpdateData(
                table: "UserAccounts",
                keyColumn: "Id",
                keyValue: 4,
                column: "PasswordHash",
                value: "VeVuLbp0j2bMCqu0G7lRjYVrMBytp8xbl1kiy5fhm3YlTk64aORlgtZj3VRwrc9m");

            migrationBuilder.CreateIndex(
                name: "IX_ReminderTasks_DueDate",
                table: "ReminderTasks",
                column: "DueDate");

            migrationBuilder.CreateIndex(
                name: "IX_ReminderTasks_IsDone",
                table: "ReminderTasks",
                column: "IsDone");

            migrationBuilder.CreateIndex(
                name: "IX_ReminderTasks_PatientId",
                table: "ReminderTasks",
                column: "PatientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReminderTasks");

            migrationBuilder.UpdateData(
                table: "UserAccounts",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "LmRChSwsulOIPd4qo9TuNqhVj7QHDmkF0zHPF+If7+YWfET+DkllRN6Hk6V8fYmq");

            migrationBuilder.UpdateData(
                table: "UserAccounts",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "phYehB6ZAxX2Hu0l0tJ0Y8qYzj/EDrEU7Nbu+2oLcBSwki95/QG4DhkV3vl8side");

            migrationBuilder.UpdateData(
                table: "UserAccounts",
                keyColumn: "Id",
                keyValue: 3,
                column: "PasswordHash",
                value: "A1GEY3Jqrg8UjDYWzW/RTUR94Vk7bomaEylHtgWhhnStpObe7itZtA4hxWZBNkT2");

            migrationBuilder.UpdateData(
                table: "UserAccounts",
                keyColumn: "Id",
                keyValue: 4,
                column: "PasswordHash",
                value: "LEB05hJoH2AGN8q4G1oaMd9J3g/YtfdUjKkU+p5WFuD7iaBQp0oLGOEEEyA3tlJb");
        }
    }
}
