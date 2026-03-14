using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clinic.Services.Migrations
{
    /// <inheritdoc />
    public partial class MoveSurgeryRoomToOperatingRoomFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_OperatingRooms_OperatingRoomId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_OperatingRoomId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "Room",
                table: "SurgerySchedules");

            migrationBuilder.DropColumn(
                name: "OperatingRoomId",
                table: "Appointments");

            migrationBuilder.AddColumn<int>(
                name: "OperatingRoomId",
                table: "SurgerySchedules",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "SurgerySchedules",
                keyColumn: "Id",
                keyValue: 1,
                column: "OperatingRoomId",
                value: 1);

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

            migrationBuilder.CreateIndex(
                name: "IX_SurgerySchedules_OperatingRoomId",
                table: "SurgerySchedules",
                column: "OperatingRoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_SurgerySchedules_OperatingRooms_OperatingRoomId",
                table: "SurgerySchedules",
                column: "OperatingRoomId",
                principalTable: "OperatingRooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SurgerySchedules_OperatingRooms_OperatingRoomId",
                table: "SurgerySchedules");

            migrationBuilder.DropIndex(
                name: "IX_SurgerySchedules_OperatingRoomId",
                table: "SurgerySchedules");

            migrationBuilder.DropColumn(
                name: "OperatingRoomId",
                table: "SurgerySchedules");

            migrationBuilder.AddColumn<string>(
                name: "Room",
                table: "SurgerySchedules",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "OperatingRoomId",
                table: "Appointments",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 1,
                column: "OperatingRoomId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Appointments",
                keyColumn: "Id",
                keyValue: 2,
                column: "OperatingRoomId",
                value: null);

            migrationBuilder.UpdateData(
                table: "SurgerySchedules",
                keyColumn: "Id",
                keyValue: 1,
                column: "Room",
                value: "Phòng 101");

            migrationBuilder.UpdateData(
                table: "UserAccounts",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "DlgHc/9lUg/4IZAwnLjBAsHP7dEzMrdl8NQYsAr2KBgd0PpWMfardAXmQdkevqSx");

            migrationBuilder.UpdateData(
                table: "UserAccounts",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "tEkcf4cb4oh8Km+ZjbnRXSmTpGHJBotLNXZruXJKMvE/kiw6eENaya7SmRZU2d6E");

            migrationBuilder.UpdateData(
                table: "UserAccounts",
                keyColumn: "Id",
                keyValue: 3,
                column: "PasswordHash",
                value: "hExY8IrbGf6QCABMmKoWfTkKFhGXgohHKRUEtQt+WO+1mesEkVj+91fMtaPCJ/H2");

            migrationBuilder.UpdateData(
                table: "UserAccounts",
                keyColumn: "Id",
                keyValue: 4,
                column: "PasswordHash",
                value: "aCLyeOfj2vaH2ySDdGZI3Ih6p7FLiXz7s1O2qG4cqaFEHwVemA63vp/wMmbG3xRw");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_OperatingRoomId",
                table: "Appointments",
                column: "OperatingRoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_OperatingRooms_OperatingRoomId",
                table: "Appointments",
                column: "OperatingRoomId",
                principalTable: "OperatingRooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
