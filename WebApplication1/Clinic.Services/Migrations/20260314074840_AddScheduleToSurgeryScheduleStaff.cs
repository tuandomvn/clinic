using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clinic.Services.Migrations
{
    /// <inheritdoc />
    public partial class AddScheduleToSurgeryScheduleStaff : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DurationMinutes",
                table: "SurgeryScheduleStaff",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ScheduledAt",
                table: "SurgeryScheduleStaff",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "SurgeryScheduleStaff",
                keyColumns: new[] { "StaffId", "SurgeryScheduleId" },
                keyValues: new object[] { 3, 1 },
                columns: new[] { "DurationMinutes", "ScheduledAt" },
                values: new object[] { 120, new DateTime(2024, 3, 25, 7, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "UserAccounts",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "AqfqQT1wAvYt3KQ6hCZ5rBdSe9WmGpYYCShm49nCvkj7EwTAlAv6jRk3NMXqMtxb");

            migrationBuilder.UpdateData(
                table: "UserAccounts",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "7dyQeynSbnc6BJyzNBb71St6JQSaMpGFkphKDQkCgKem9exFbp/r/7fR43PWA9cl");

            migrationBuilder.UpdateData(
                table: "UserAccounts",
                keyColumn: "Id",
                keyValue: 3,
                column: "PasswordHash",
                value: "BoxSyL8/Vosz9zHYsN2UXynOfUa3rj5jtiGTTjuPvIofKyglJuotBatj/i71ZRpD");

            migrationBuilder.UpdateData(
                table: "UserAccounts",
                keyColumn: "Id",
                keyValue: 4,
                column: "PasswordHash",
                value: "Gkhbkgx2ICHfXMR4RST1V18w2jdkGA6orD2yu+MFL15AXh1yMQVzFWYSYx+FNc0W");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DurationMinutes",
                table: "SurgeryScheduleStaff");

            migrationBuilder.DropColumn(
                name: "ScheduledAt",
                table: "SurgeryScheduleStaff");

            migrationBuilder.UpdateData(
                table: "UserAccounts",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "D1Yu0sgtDdhqtsQZlCKy3yFW/34K/4cDfkqxYzordElqdZYK+AGeuKq0zZQJ0b0h");

            migrationBuilder.UpdateData(
                table: "UserAccounts",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "bK433VoU423Gr+oXkl2GFx0VbH8iZ4/S5/0TQA72oqzLTQAsjITb6rUWzojMVCax");

            migrationBuilder.UpdateData(
                table: "UserAccounts",
                keyColumn: "Id",
                keyValue: 3,
                column: "PasswordHash",
                value: "GsH9IU+c+mnzdw+jlW4miESxBNrWxl0cK54bVt8H4GszlUsGrPebQtYdiVOhBKHS");

            migrationBuilder.UpdateData(
                table: "UserAccounts",
                keyColumn: "Id",
                keyValue: 4,
                column: "PasswordHash",
                value: "ObZ1+1/sx8TxjLqksTZ/BosKgTH+ChLguSjfoDqbsJDvwb4dxdpng0EiBq010eag");
        }
    }
}
