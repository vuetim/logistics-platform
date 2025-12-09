using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogisticsPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update_LoadStop_To_Turvo_Model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasTime",
                table: "LoadStops");

            migrationBuilder.RenameColumn(
                name: "Zip",
                table: "LoadStops",
                newName: "PostalCode");

            migrationBuilder.RenameColumn(
                name: "AppointmentTo",
                table: "LoadStops",
                newName: "RevisedArrivalTo");

            migrationBuilder.RenameColumn(
                name: "AppointmentFrom",
                table: "LoadStops",
                newName: "RevisedArrivalFrom");

            migrationBuilder.AddColumn<DateTime>(
                name: "ActualArrival",
                table: "LoadStops",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ActualDeparture",
                table: "LoadStops",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AddressLine1",
                table: "LoadStops",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AddressLine2",
                table: "LoadStops",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AppointmentType",
                table: "LoadStops",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "LoadStops",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "FlexMinutes",
                table: "LoadStops",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Latitude",
                table: "LoadStops",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Longitude",
                table: "LoadStops",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PlannedArrivalFrom",
                table: "LoadStops",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PlannedArrivalTo",
                table: "LoadStops",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PlannedDepartureFrom",
                table: "LoadStops",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PlannedDepartureTo",
                table: "LoadStops",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "LoadStops",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 2, 21, 18, 32, 21, DateTimeKind.Utc).AddTicks(3109));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 2, 21, 18, 32, 21, DateTimeKind.Utc).AddTicks(3129));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 2, 21, 18, 32, 21, DateTimeKind.Utc).AddTicks(3132));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa4"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 2, 21, 18, 32, 21, DateTimeKind.Utc).AddTicks(3135));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa5"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 2, 21, 18, 32, 21, DateTimeKind.Utc).AddTicks(3138));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActualArrival",
                table: "LoadStops");

            migrationBuilder.DropColumn(
                name: "ActualDeparture",
                table: "LoadStops");

            migrationBuilder.DropColumn(
                name: "AddressLine1",
                table: "LoadStops");

            migrationBuilder.DropColumn(
                name: "AddressLine2",
                table: "LoadStops");

            migrationBuilder.DropColumn(
                name: "AppointmentType",
                table: "LoadStops");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "LoadStops");

            migrationBuilder.DropColumn(
                name: "FlexMinutes",
                table: "LoadStops");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "LoadStops");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "LoadStops");

            migrationBuilder.DropColumn(
                name: "PlannedArrivalFrom",
                table: "LoadStops");

            migrationBuilder.DropColumn(
                name: "PlannedArrivalTo",
                table: "LoadStops");

            migrationBuilder.DropColumn(
                name: "PlannedDepartureFrom",
                table: "LoadStops");

            migrationBuilder.DropColumn(
                name: "PlannedDepartureTo",
                table: "LoadStops");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "LoadStops");

            migrationBuilder.RenameColumn(
                name: "RevisedArrivalTo",
                table: "LoadStops",
                newName: "AppointmentTo");

            migrationBuilder.RenameColumn(
                name: "RevisedArrivalFrom",
                table: "LoadStops",
                newName: "AppointmentFrom");

            migrationBuilder.RenameColumn(
                name: "PostalCode",
                table: "LoadStops",
                newName: "Zip");

            migrationBuilder.AddColumn<bool>(
                name: "HasTime",
                table: "LoadStops",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 1, 23, 11, 16, 614, DateTimeKind.Utc).AddTicks(7684));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 1, 23, 11, 16, 614, DateTimeKind.Utc).AddTicks(7722));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 1, 23, 11, 16, 614, DateTimeKind.Utc).AddTicks(7726));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa4"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 1, 23, 11, 16, 614, DateTimeKind.Utc).AddTicks(7728));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa5"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 1, 23, 11, 16, 614, DateTimeKind.Utc).AddTicks(7731));
        }
    }
}
