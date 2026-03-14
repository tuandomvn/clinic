using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clinic.Services.Migrations
{
    /// <inheritdoc />
    public partial class AddServiceOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ServiceOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    HealthRecordId = table.Column<int>(type: "int", nullable: false),
                    ServiceName = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Notes = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceOrders_HistoryRecords_HealthRecordId",
                        column: x => x.HealthRecordId,
                        principalTable: "HistoryRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

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

            migrationBuilder.CreateIndex(
                name: "IX_ServiceOrders_HealthRecordId",
                table: "ServiceOrders",
                column: "HealthRecordId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceOrders");

            migrationBuilder.UpdateData(
                table: "UserAccounts",
                keyColumn: "Id",
                keyValue: 1,
                column: "PasswordHash",
                value: "OS+BeqeVbGmSJK4/Y55rn4KN3D9MCelHh5ChQ7o6tGg+/hIJDGQoOh6oHJtbHJ9/");

            migrationBuilder.UpdateData(
                table: "UserAccounts",
                keyColumn: "Id",
                keyValue: 2,
                column: "PasswordHash",
                value: "w8wvkDXsgqD7dLEA6Hr4N1D/jcwTL0vxhQz/mbDRvkAUAH9zfINLY26ATYxVSY/J");

            migrationBuilder.UpdateData(
                table: "UserAccounts",
                keyColumn: "Id",
                keyValue: 3,
                column: "PasswordHash",
                value: "Xf7HIc9EkhHB2S3LKty06Q+sE9eUzqYh5SfZP6HfZdlFwpta4PJ0FFZ+dNUhdrSM");

            migrationBuilder.UpdateData(
                table: "UserAccounts",
                keyColumn: "Id",
                keyValue: 4,
                column: "PasswordHash",
                value: "zY4QQnwn2YKe1FQmM9/cYGfIZVnmlqeW0M7OXMTTOJAY8PNgzJuyH0klKwcNUJr2");
        }
    }
}
