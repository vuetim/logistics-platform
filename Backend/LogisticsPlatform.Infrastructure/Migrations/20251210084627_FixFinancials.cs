using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogisticsPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixFinancials : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "CustomerInvoices");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "CustomerInvoices");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "CustomerInvoiceLineItems");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "CustomerInvoiceLineItems");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "CarrierSettlements");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "CarrierSettlements");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "CarrierSettlementLineItems");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "CarrierSettlementLineItems");

            migrationBuilder.AddColumn<int>(
                name: "InvoiceType",
                table: "CustomerInvoices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "CustomerInvoices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Billable",
                table: "CustomerInvoiceLineItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "CustomerInvoiceLineItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Billable",
                table: "CarrierSettlementLineItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "CarrierSettlementLineItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 10, 8, 46, 26, 579, DateTimeKind.Utc).AddTicks(5942));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 10, 8, 46, 26, 579, DateTimeKind.Utc).AddTicks(5957));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 10, 8, 46, 26, 579, DateTimeKind.Utc).AddTicks(5959));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa4"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 10, 8, 46, 26, 579, DateTimeKind.Utc).AddTicks(5961));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa5"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 10, 8, 46, 26, 579, DateTimeKind.Utc).AddTicks(5963));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvoiceType",
                table: "CustomerInvoices");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "CustomerInvoices");

            migrationBuilder.DropColumn(
                name: "Billable",
                table: "CustomerInvoiceLineItems");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "CustomerInvoiceLineItems");

            migrationBuilder.DropColumn(
                name: "Billable",
                table: "CarrierSettlementLineItems");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "CarrierSettlementLineItems");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "CustomerInvoices",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "CustomerInvoices",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "CustomerInvoiceLineItems",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "CustomerInvoiceLineItems",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "CarrierSettlements",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "CarrierSettlements",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "CarrierSettlementLineItems",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "CarrierSettlementLineItems",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 9, 11, 28, 0, 445, DateTimeKind.Utc).AddTicks(4561));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 9, 11, 28, 0, 445, DateTimeKind.Utc).AddTicks(4597));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 9, 11, 28, 0, 445, DateTimeKind.Utc).AddTicks(4603));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa4"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 9, 11, 28, 0, 445, DateTimeKind.Utc).AddTicks(4608));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa5"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 9, 11, 28, 0, 445, DateTimeKind.Utc).AddTicks(4612));
        }
    }
}
