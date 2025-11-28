using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogisticsPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RecreateLoadNotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 27, 22, 16, 31, 551, DateTimeKind.Utc).AddTicks(6110));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 27, 22, 16, 31, 551, DateTimeKind.Utc).AddTicks(6125));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 27, 22, 16, 31, 551, DateTimeKind.Utc).AddTicks(6129));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa4"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 27, 22, 16, 31, 551, DateTimeKind.Utc).AddTicks(6142));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa5"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 27, 22, 16, 31, 551, DateTimeKind.Utc).AddTicks(6144));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 27, 22, 13, 1, 972, DateTimeKind.Utc).AddTicks(7101));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 27, 22, 13, 1, 972, DateTimeKind.Utc).AddTicks(7124));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 27, 22, 13, 1, 972, DateTimeKind.Utc).AddTicks(7127));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa4"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 27, 22, 13, 1, 972, DateTimeKind.Utc).AddTicks(7130));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa5"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 27, 22, 13, 1, 972, DateTimeKind.Utc).AddTicks(7132));
        }
    }
}
