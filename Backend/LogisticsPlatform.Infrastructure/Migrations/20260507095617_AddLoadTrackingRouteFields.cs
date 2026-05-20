using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogisticsPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLoadTrackingRouteFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Accessorials",
                table: "OrderCosts",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "QuotedTotal",
                table: "OrderCosts",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "DistanceMiles",
                table: "Loads",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DurationMinutes",
                table: "Loads",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EncodedPolyline",
                table: "Loads",
                type: "nvarchar(8000)",
                maxLength: 8000,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "LastKnownLatitude",
                table: "Loads",
                type: "decimal(10,7)",
                precision: 10,
                scale: 7,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastKnownLocationAt",
                table: "Loads",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "LastKnownLongitude",
                table: "Loads",
                type: "decimal(10,7)",
                precision: 10,
                scale: 7,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrackingExternalId",
                table: "Loads",
                type: "nvarchar(160)",
                maxLength: 160,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrackingProvider",
                table: "Loads",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"),
                column: "CreatedAt",
                value: new DateTime(2026, 5, 7, 9, 56, 16, 47, DateTimeKind.Utc).AddTicks(5645));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"),
                column: "CreatedAt",
                value: new DateTime(2026, 5, 7, 9, 56, 16, 47, DateTimeKind.Utc).AddTicks(5669));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"),
                column: "CreatedAt",
                value: new DateTime(2026, 5, 7, 9, 56, 16, 47, DateTimeKind.Utc).AddTicks(5671));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa4"),
                column: "CreatedAt",
                value: new DateTime(2026, 5, 7, 9, 56, 16, 47, DateTimeKind.Utc).AddTicks(5673));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa5"),
                column: "CreatedAt",
                value: new DateTime(2026, 5, 7, 9, 56, 16, 47, DateTimeKind.Utc).AddTicks(5675));

            migrationBuilder.CreateIndex(
                name: "IX_Loads_TrackingExternalId",
                table: "Loads",
                column: "TrackingExternalId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Loads_TrackingExternalId",
                table: "Loads");

            migrationBuilder.DropColumn(
                name: "Accessorials",
                table: "OrderCosts");

            migrationBuilder.DropColumn(
                name: "QuotedTotal",
                table: "OrderCosts");

            migrationBuilder.DropColumn(
                name: "DistanceMiles",
                table: "Loads");

            migrationBuilder.DropColumn(
                name: "DurationMinutes",
                table: "Loads");

            migrationBuilder.DropColumn(
                name: "EncodedPolyline",
                table: "Loads");

            migrationBuilder.DropColumn(
                name: "LastKnownLatitude",
                table: "Loads");

            migrationBuilder.DropColumn(
                name: "LastKnownLocationAt",
                table: "Loads");

            migrationBuilder.DropColumn(
                name: "LastKnownLongitude",
                table: "Loads");

            migrationBuilder.DropColumn(
                name: "TrackingExternalId",
                table: "Loads");

            migrationBuilder.DropColumn(
                name: "TrackingProvider",
                table: "Loads");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"),
                column: "CreatedAt",
                value: new DateTime(2026, 5, 5, 23, 27, 11, 386, DateTimeKind.Utc).AddTicks(9735));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"),
                column: "CreatedAt",
                value: new DateTime(2026, 5, 5, 23, 27, 11, 386, DateTimeKind.Utc).AddTicks(9762));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"),
                column: "CreatedAt",
                value: new DateTime(2026, 5, 5, 23, 27, 11, 386, DateTimeKind.Utc).AddTicks(9764));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa4"),
                column: "CreatedAt",
                value: new DateTime(2026, 5, 5, 23, 27, 11, 386, DateTimeKind.Utc).AddTicks(9767));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa5"),
                column: "CreatedAt",
                value: new DateTime(2026, 5, 5, 23, 27, 11, 386, DateTimeKind.Utc).AddTicks(9769));
        }
    }
}
