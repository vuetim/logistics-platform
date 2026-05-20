using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using LogisticsPlatform.Infrastructure.Persistence;

#nullable disable

namespace LogisticsPlatform.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20260507110000_AddProductionLoadCoreRecords")]
    public partial class AddProductionLoadCoreRecords : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AppointmentConfirmed",
                table: "OrderRoutes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "AppointmentConfirmationNumber",
                table: "OrderRoutes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AppointmentStatus",
                table: "OrderRoutes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "AppointmentConfirmed",
                table: "LoadStops",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "AppointmentConfirmationNumber",
                table: "LoadStops",
                type: "nvarchar(120)",
                maxLength: 120,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AppointmentStatus",
                table: "LoadStops",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AcceptedByEmail",
                table: "LoadCarrierAssignments",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AcceptedByName",
                table: "LoadCarrierAssignments",
                type: "nvarchar(160)",
                maxLength: 160,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RejectedReason",
                table: "LoadCarrierAssignments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TenderExpiresAt",
                table: "LoadCarrierAssignments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenderMethod",
                table: "LoadCarrierAssignments",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenderNotes",
                table: "LoadCarrierAssignments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LoadExceptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoadId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoadStopId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExceptionKey = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    ExceptionValue = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    ReasonKey = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    ReasonValue = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: true),
                    EdiReasonCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ResponsiblePartyKey = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    ResponsiblePartyValue = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResolutionNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AffectedItemName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AffectedItemReference = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: true),
                    Unit = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    OccurredAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ResolvedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoadExceptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoadExceptions_LoadStops_LoadStopId",
                        column: x => x.LoadStopId,
                        principalTable: "LoadStops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LoadExceptions_Loads_LoadId",
                        column: x => x.LoadId,
                        principalTable: "Loads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LoadExceptions_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LoadStopServiceRequirements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoadStopId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceKey = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    ServiceValue = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPickupService = table.Column<bool>(type: "bit", nullable: false),
                    IsDeliveryService = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoadStopServiceRequirements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoadStopServiceRequirements_LoadStops_LoadStopId",
                        column: x => x.LoadStopId,
                        principalTable: "LoadStops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LoadExceptions_LoadId_Status",
                table: "LoadExceptions",
                columns: new[] { "LoadId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_LoadExceptions_LoadStopId",
                table: "LoadExceptions",
                column: "LoadStopId");

            migrationBuilder.CreateIndex(
                name: "IX_LoadExceptions_OrderId",
                table: "LoadExceptions",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_LoadStopServiceRequirements_LoadStopId_ServiceKey",
                table: "LoadStopServiceRequirements",
                columns: new[] { "LoadStopId", "ServiceKey" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "LoadExceptions");
            migrationBuilder.DropTable(name: "LoadStopServiceRequirements");

            migrationBuilder.DropColumn(name: "AppointmentConfirmed", table: "OrderRoutes");
            migrationBuilder.DropColumn(name: "AppointmentConfirmationNumber", table: "OrderRoutes");
            migrationBuilder.DropColumn(name: "AppointmentStatus", table: "OrderRoutes");

            migrationBuilder.DropColumn(name: "AppointmentConfirmed", table: "LoadStops");
            migrationBuilder.DropColumn(name: "AppointmentConfirmationNumber", table: "LoadStops");
            migrationBuilder.DropColumn(name: "AppointmentStatus", table: "LoadStops");

            migrationBuilder.DropColumn(name: "AcceptedByEmail", table: "LoadCarrierAssignments");
            migrationBuilder.DropColumn(name: "AcceptedByName", table: "LoadCarrierAssignments");
            migrationBuilder.DropColumn(name: "RejectedReason", table: "LoadCarrierAssignments");
            migrationBuilder.DropColumn(name: "TenderExpiresAt", table: "LoadCarrierAssignments");
            migrationBuilder.DropColumn(name: "TenderMethod", table: "LoadCarrierAssignments");
            migrationBuilder.DropColumn(name: "TenderNotes", table: "LoadCarrierAssignments");
        }
    }
}
