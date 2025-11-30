using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogisticsPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NormalizeContactsAndAddresses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CustomerAddresses_CustomerId",
                table: "CustomerAddresses");

            migrationBuilder.DropIndex(
                name: "IX_CarrierAddresses_CarrierId",
                table: "CarrierAddresses");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "CustomerContacts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPrimary",
                table: "CustomerContacts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "CustomerAddresses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "CarrierContacts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPrimary",
                table: "CarrierContacts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "CarrierAddresses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 30, 0, 31, 31, 692, DateTimeKind.Utc).AddTicks(3148));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 30, 0, 31, 31, 692, DateTimeKind.Utc).AddTicks(3183));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 30, 0, 31, 31, 692, DateTimeKind.Utc).AddTicks(3187));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa4"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 30, 0, 31, 31, 692, DateTimeKind.Utc).AddTicks(3191));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa5"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 30, 0, 31, 31, 692, DateTimeKind.Utc).AddTicks(3195));

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAddresses_CustomerId_IsActive",
                table: "CustomerAddresses",
                columns: new[] { "CustomerId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAddresses_CustomerId_IsPrimary",
                table: "CustomerAddresses",
                columns: new[] { "CustomerId", "IsPrimary" });

            migrationBuilder.CreateIndex(
                name: "IX_CarrierAddresses_CarrierId_IsActive",
                table: "CarrierAddresses",
                columns: new[] { "CarrierId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_CarrierAddresses_CarrierId_IsPrimary",
                table: "CarrierAddresses",
                columns: new[] { "CarrierId", "IsPrimary" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CustomerAddresses_CustomerId_IsActive",
                table: "CustomerAddresses");

            migrationBuilder.DropIndex(
                name: "IX_CustomerAddresses_CustomerId_IsPrimary",
                table: "CustomerAddresses");

            migrationBuilder.DropIndex(
                name: "IX_CarrierAddresses_CarrierId_IsActive",
                table: "CarrierAddresses");

            migrationBuilder.DropIndex(
                name: "IX_CarrierAddresses_CarrierId_IsPrimary",
                table: "CarrierAddresses");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "CustomerContacts");

            migrationBuilder.DropColumn(
                name: "IsPrimary",
                table: "CustomerContacts");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "CustomerAddresses");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "CarrierContacts");

            migrationBuilder.DropColumn(
                name: "IsPrimary",
                table: "CarrierContacts");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "CarrierAddresses");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 29, 16, 29, 16, 918, DateTimeKind.Utc).AddTicks(5296));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 29, 16, 29, 16, 918, DateTimeKind.Utc).AddTicks(5326));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 29, 16, 29, 16, 918, DateTimeKind.Utc).AddTicks(5335));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa4"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 29, 16, 29, 16, 918, DateTimeKind.Utc).AddTicks(5343));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa5"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 29, 16, 29, 16, 918, DateTimeKind.Utc).AddTicks(5349));

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAddresses_CustomerId",
                table: "CustomerAddresses",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CarrierAddresses_CarrierId",
                table: "CarrierAddresses",
                column: "CarrierId");
        }
    }
}
