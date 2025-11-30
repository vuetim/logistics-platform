using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogisticsPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedIndexOptimization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Loads_CustomerId",
                table: "Loads");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 28, 22, 59, 53, 570, DateTimeKind.Utc).AddTicks(112));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 28, 22, 59, 53, 570, DateTimeKind.Utc).AddTicks(137));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 28, 22, 59, 53, 570, DateTimeKind.Utc).AddTicks(140));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa4"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 28, 22, 59, 53, 570, DateTimeKind.Utc).AddTicks(142));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa5"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 28, 22, 59, 53, 570, DateTimeKind.Utc).AddTicks(144));

            migrationBuilder.CreateIndex(
                name: "IX_Loads_CustomerId_Status",
                table: "Loads",
                columns: new[] { "CustomerId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Loads_IsArchived",
                table: "Loads",
                column: "IsArchived");

            migrationBuilder.CreateIndex(
                name: "IX_Loads_IsArchived_Status",
                table: "Loads",
                columns: new[] { "IsArchived", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Loads_Status",
                table: "Loads",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Loads_CustomerId_Status",
                table: "Loads");

            migrationBuilder.DropIndex(
                name: "IX_Loads_IsArchived",
                table: "Loads");

            migrationBuilder.DropIndex(
                name: "IX_Loads_IsArchived_Status",
                table: "Loads");

            migrationBuilder.DropIndex(
                name: "IX_Loads_Status",
                table: "Loads");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 27, 22, 27, 0, 925, DateTimeKind.Utc).AddTicks(5203));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 27, 22, 27, 0, 925, DateTimeKind.Utc).AddTicks(5219));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 27, 22, 27, 0, 925, DateTimeKind.Utc).AddTicks(5221));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa4"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 27, 22, 27, 0, 925, DateTimeKind.Utc).AddTicks(5223));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa5"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 27, 22, 27, 0, 925, DateTimeKind.Utc).AddTicks(5225));

            migrationBuilder.CreateIndex(
                name: "IX_Loads_CustomerId",
                table: "Loads",
                column: "CustomerId");
        }
    }
}
