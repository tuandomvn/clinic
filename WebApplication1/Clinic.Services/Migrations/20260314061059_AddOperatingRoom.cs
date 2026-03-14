using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Clinic.Services.Migrations
{
    /// <inheritdoc />
    public partial class AddOperatingRoom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OperatingRooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Location = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperatingRooms", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "OperatingRooms",
                columns: new[] { "Id", "IsActive", "Location", "Name" },
                values: new object[,]
                {
                    { 1, true, "Tầng 2", "Phòng mổ 1" },
                    { 2, true, "Tầng 2", "Phòng mổ 2" },
                    { 3, true, "Tầng 3", "Phòng mổ 3" }
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OperatingRooms");

            migrationBuilder.UpdateData(
                table: "UserAccounts",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "tok0Ee0/1jksC/QThROL30lpd8pST/NY4rsxRJD8DNoX0YfvY3I08wLo1hRtq8Je");

            migrationBuilder.UpdateData(
                table: "UserAccounts",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "K5DmNNbDUgUbB+mOiNJpYHdbkjiwTwycdKdn6oRSi/PFcjsgQtmV4PSugmBp9DhH");

            migrationBuilder.UpdateData(
                table: "UserAccounts",
                keyColumn: "Id",
                keyValue: 3,
                column: "PasswordHash",
                value: "qbDSXTvZ0wJ09hHGYrgQ8S3fJcuotixI5R/a8XT0t7g1D9HU12cDy5QgY+0wVZ7z");

            migrationBuilder.UpdateData(
                table: "UserAccounts",
                keyColumn: "Id",
                keyValue: 4,
                column: "PasswordHash",
                value: "K101tkg5at70YXdK/uibRYRDXIpiSrZy313qUhl7APCaH9uqylDTw5PyK9doPmIC");
        }
    }
}
