using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clinic.Services.Migrations
{
    /// <inheritdoc />
    public partial class AddOperatingRoomIdToAppointment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_OperatingRooms_OperatingRoomId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_OperatingRoomId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "OperatingRoomId",
                table: "Appointments");

            migrationBuilder.UpdateData(
                table: "UserAccounts",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "3+AA5qBJHmS0umIG/j+q4LQcHH9e5rLssF2xtRsoP2m9UICOWRBQ5EMcRXYtQNV+");

            migrationBuilder.UpdateData(
                table: "UserAccounts",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "4UiIyyL7ua1WmkmurEwiDSFSxzsraGjOrEcWMcxhc/JMCqsDsiXW6p190UVu9BNt");

            migrationBuilder.UpdateData(
                table: "UserAccounts",
                keyColumn: "Id",
                keyValue: 3,
                column: "PasswordHash",
                value: "ogJ8NZvoHyKoLChfO3zx0ZHK+D/XkrA7KN2V+k9inat27FhgWjFsvERjS0yPiXID");

            migrationBuilder.UpdateData(
                table: "UserAccounts",
                keyColumn: "Id",
                keyValue: 4,
                column: "PasswordHash",
                value: "+vqn2sNMUQ+QAyu0KEV9AHCnOR8Ngw2hqV0Y3t/pAJO5t5SiAzwRN8gqH53s6K61");
        }
    }
}
