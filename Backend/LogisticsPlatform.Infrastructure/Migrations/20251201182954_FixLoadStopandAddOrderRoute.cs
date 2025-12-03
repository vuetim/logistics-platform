using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogisticsPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixLoadStopandAddOrderRoute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_CustomerAddresses_DestinationAddressId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_CustomerAddresses_OriginAddressId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_DestinationAddressId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_OriginAddressId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DestinationAddressId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OriginAddressId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PlannedDate",
                table: "LoadStops");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DestinationAddressId",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "OriginAddressId",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "PlannedDate",
                table: "LoadStops",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 1, 3, 5, 11, 225, DateTimeKind.Utc).AddTicks(9192));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 1, 3, 5, 11, 225, DateTimeKind.Utc).AddTicks(9208));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 1, 3, 5, 11, 225, DateTimeKind.Utc).AddTicks(9211));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa4"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 1, 3, 5, 11, 225, DateTimeKind.Utc).AddTicks(9213));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa5"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 1, 3, 5, 11, 225, DateTimeKind.Utc).AddTicks(9215));

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DestinationAddressId",
                table: "Orders",
                column: "DestinationAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OriginAddressId",
                table: "Orders",
                column: "OriginAddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_CustomerAddresses_DestinationAddressId",
                table: "Orders",
                column: "DestinationAddressId",
                principalTable: "CustomerAddresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_CustomerAddresses_OriginAddressId",
                table: "Orders",
                column: "OriginAddressId",
                principalTable: "CustomerAddresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
