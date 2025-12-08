using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogisticsPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCostsRefactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCarrier",
                table: "OrderCostLineItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsCustomer",
                table: "OrderCostLineItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "OrderCostLineItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsCarrier",
                table: "LoadCostLineItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsCustomer",
                table: "LoadCostLineItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "LoadCostLineItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 8, 8, 9, 29, 740, DateTimeKind.Utc).AddTicks(8772));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 8, 8, 9, 29, 740, DateTimeKind.Utc).AddTicks(8788));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 8, 8, 9, 29, 740, DateTimeKind.Utc).AddTicks(8791));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa4"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 8, 8, 9, 29, 740, DateTimeKind.Utc).AddTicks(8793));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa5"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 8, 8, 9, 29, 740, DateTimeKind.Utc).AddTicks(8805));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCarrier",
                table: "OrderCostLineItems");

            migrationBuilder.DropColumn(
                name: "IsCustomer",
                table: "OrderCostLineItems");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "OrderCostLineItems");

            migrationBuilder.DropColumn(
                name: "IsCarrier",
                table: "LoadCostLineItems");

            migrationBuilder.DropColumn(
                name: "IsCustomer",
                table: "LoadCostLineItems");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "LoadCostLineItems");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 5, 23, 21, 55, 660, DateTimeKind.Utc).AddTicks(2635));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 5, 23, 21, 55, 660, DateTimeKind.Utc).AddTicks(2647));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 5, 23, 21, 55, 660, DateTimeKind.Utc).AddTicks(2649));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa4"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 5, 23, 21, 55, 660, DateTimeKind.Utc).AddTicks(2651));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa5"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 5, 23, 21, 55, 660, DateTimeKind.Utc).AddTicks(2654));
        }
    }
}
