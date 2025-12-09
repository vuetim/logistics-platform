using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogisticsPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixLoadOrderFKNoCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoadOrders_Loads_LoadId",
                table: "LoadOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_LoadOrders_Orders_OrderId",
                table: "LoadOrders");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrderId",
                table: "LoadOrders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 1, 19, 39, 13, 854, DateTimeKind.Utc).AddTicks(4338));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 1, 19, 39, 13, 854, DateTimeKind.Utc).AddTicks(4349));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 1, 19, 39, 13, 854, DateTimeKind.Utc).AddTicks(4352));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa4"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 1, 19, 39, 13, 854, DateTimeKind.Utc).AddTicks(4354));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa5"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 1, 19, 39, 13, 854, DateTimeKind.Utc).AddTicks(4356));

            migrationBuilder.AddForeignKey(
                name: "FK_LoadOrders_Loads_LoadId",
                table: "LoadOrders",
                column: "LoadId",
                principalTable: "Loads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LoadOrders_Orders_OrderId",
                table: "LoadOrders",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoadOrders_Loads_LoadId",
                table: "LoadOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_LoadOrders_Orders_OrderId",
                table: "LoadOrders");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrderId",
                table: "LoadOrders",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 1, 18, 29, 52, 833, DateTimeKind.Utc).AddTicks(3155));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 1, 18, 29, 52, 833, DateTimeKind.Utc).AddTicks(3166));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 1, 18, 29, 52, 833, DateTimeKind.Utc).AddTicks(3169));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa4"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 1, 18, 29, 52, 833, DateTimeKind.Utc).AddTicks(3170));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa5"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 1, 18, 29, 52, 833, DateTimeKind.Utc).AddTicks(3172));

            migrationBuilder.AddForeignKey(
                name: "FK_LoadOrders_Loads_LoadId",
                table: "LoadOrders",
                column: "LoadId",
                principalTable: "Loads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LoadOrders_Orders_OrderId",
                table: "LoadOrders",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");
        }
    }
}
