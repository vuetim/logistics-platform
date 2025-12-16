using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogisticsPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBigChangesLoadLoadAssigns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // =====================================================
            // ORDER EQUIPMENT REQUIREMENTS – SAFE ENUM MIGRATION
            // =====================================================

            migrationBuilder.RenameColumn(
                name: "RequiredTemperature",
                table: "OrderEquipmentRequirements",
                newName: "MinTemperature");

            // 1️⃣ TEMP COLUMNS (INT)
            migrationBuilder.AddColumn<int>(
                name: "WeightUnit_Int",
                table: "OrderEquipmentRequirements",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TemperatureUnit_Int",
                table: "OrderEquipmentRequirements",
                type: "int",
                nullable: true);

            // 2️⃣ DATA MAPPING STRING → ENUM
            migrationBuilder.Sql(@"
        UPDATE OrderEquipmentRequirements
        SET WeightUnit_Int =
            CASE LOWER(LTRIM(RTRIM(WeightUnit)))
                WHEN 'lb' THEN 0
                WHEN 'lbs' THEN 0
                WHEN 'kg' THEN 1
                ELSE 0
            END
    ");

            migrationBuilder.Sql(@"
        UPDATE OrderEquipmentRequirements
        SET TemperatureUnit_Int =
            CASE LOWER(LTRIM(RTRIM(TemperatureUnit)))
                WHEN 'f' THEN 0
                WHEN 'fahrenheit' THEN 0
                WHEN 'c' THEN 1
                WHEN 'celsius' THEN 1
                ELSE 0
            END
    ");

            // 3️⃣ DROP OLD STRING COLUMNS
            migrationBuilder.DropColumn(
                name: "WeightUnit",
                table: "OrderEquipmentRequirements");

            migrationBuilder.DropColumn(
                name: "TemperatureUnit",
                table: "OrderEquipmentRequirements");

            // 4️⃣ RENAME TEMP → FINAL
            migrationBuilder.RenameColumn(
                name: "WeightUnit_Int",
                table: "OrderEquipmentRequirements",
                newName: "WeightUnit");

            migrationBuilder.RenameColumn(
                name: "TemperatureUnit_Int",
                table: "OrderEquipmentRequirements",
                newName: "TemperatureUnit");

            // 5️⃣ ENFORCE NOT NULL + DEFAULT
            migrationBuilder.AlterColumn<int>(
                name: "WeightUnit",
                table: "OrderEquipmentRequirements",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "TemperatureUnit",
                table: "OrderEquipmentRequirements",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsPrefered",
                table: "OrderEquipmentRequirements",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "MaxTemperature",
                table: "OrderEquipmentRequirements",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: true);

            // =====================================================
            // ORDERS
            // =====================================================

            migrationBuilder.AddColumn<string>("Commodity", "Orders", nullable: true);
            migrationBuilder.AddColumn<string>("DeliveryNotes", "Orders", nullable: true);
            migrationBuilder.AddColumn<string>("DispatchNotes", "Orders", nullable: true);
            migrationBuilder.AddColumn<string>("PrimaryBolNumber", "Orders", nullable: true);
            migrationBuilder.AddColumn<string>("PrimaryPONumber", "Orders", nullable: true);
            migrationBuilder.AddColumn<string>("PrimaryProNumber", "Orders", nullable: true);
            migrationBuilder.AddColumn<int>("TotalPallets", "Orders", nullable: true);
            migrationBuilder.AddColumn<decimal>("TotalWeight", "Orders", "decimal(18,2)", nullable: true);
            migrationBuilder.AddColumn<decimal>("TotalVolume", "Orders", "decimal(18,4)", nullable: true);

            // =====================================================
            // ORDER ROUTES
            // =====================================================

            migrationBuilder.AddColumn<string>("AppointmentNumber", "OrderRoutes", nullable: true);
            migrationBuilder.AddColumn<string>("StopReference", "OrderRoutes", nullable: true);

            // =====================================================
            // LOAD STOPS
            // =====================================================

            migrationBuilder.AddColumn<DateTime>("ActualCheckedInTime", "LoadStops", nullable: true);
            migrationBuilder.AddColumn<DateTime>("ActualCheckedOutTime", "LoadStops", nullable: true);
            migrationBuilder.AddColumn<string>("AppointmentNumber", "LoadStops", nullable: true);
            migrationBuilder.AddColumn<string>("ContactName", "LoadStops", nullable: true);
            migrationBuilder.AddColumn<string>("ContactPhone", "LoadStops", nullable: true);
            migrationBuilder.AddColumn<bool>("IsLateArrival", "LoadStops", nullable: false, defaultValue: false);
            migrationBuilder.AddColumn<bool>("IsLateDeparture", "LoadStops", nullable: false, defaultValue: false);
            migrationBuilder.AddColumn<string>("StopReference", "LoadStops", nullable: false, defaultValue: "");

            // =====================================================
            // LOADS
            // =====================================================

            migrationBuilder.AddColumn<string>("BolNumber", "Loads", nullable: true);
            migrationBuilder.AddColumn<string>("CarrierSCAC", "Loads", nullable: true);
            migrationBuilder.AddColumn<string>("DriverEmail", "Loads", nullable: true);
            migrationBuilder.AddColumn<string>("DriverName", "Loads", nullable: true);
            migrationBuilder.AddColumn<string>("DriverPhone", "Loads", nullable: true);
            migrationBuilder.AddColumn<bool>("OnTimeDelivery", "Loads", nullable: false, defaultValue: false);
            migrationBuilder.AddColumn<bool>("OnTimePickup", "Loads", nullable: false, defaultValue: false);
            migrationBuilder.AddColumn<DateTime>("PodReceivedAt", "Loads", nullable: true);
            migrationBuilder.AddColumn<string>("PodUploadedBy", "Loads", nullable: true);
            migrationBuilder.AddColumn<string>("ProNumber", "Loads", nullable: true);
            migrationBuilder.AddColumn<string>("RateConfirmationNumber", "Loads", nullable: true);
            migrationBuilder.AddColumn<string>("TrackingNumber", "Loads", nullable: true);
            migrationBuilder.AddColumn<string>("TrailerNumber", "Loads", nullable: true);
            migrationBuilder.AddColumn<int>("TransitTimeHours", "Loads", nullable: true);
            migrationBuilder.AddColumn<string>("TruckNumber", "Loads", nullable: true);

            // =====================================================
            // LOAD ITEMS
            // =====================================================

            migrationBuilder.AddColumn<string>("CustomerReference", "LoadItems", nullable: true);
            migrationBuilder.AddColumn<string>("HazardClass", "LoadItems", nullable: true);
            migrationBuilder.AddColumn<string>("IdentificationNumber", "LoadItems", nullable: true);
            migrationBuilder.AddColumn<decimal>("Volume", "LoadItems", "decimal(18,4)", nullable: true);
            migrationBuilder.AddColumn<string>("VolumeUnit", "LoadItems", nullable: true);

            // =====================================================
            // LOAD EQUIPMENT
            // =====================================================

            migrationBuilder.AddColumn<bool>("IsPrefered", "LoadEquipment", nullable: false, defaultValue: false);
            migrationBuilder.AddColumn<int>("Quantity", "LoadEquipment", nullable: false, defaultValue: 0);
            migrationBuilder.AddColumn<Guid>("SourceOrderEquipmentRequirementId", "LoadEquipment", nullable: true);

            // =====================================================
            // LOAD CARRIER ASSIGNMENTS
            // =====================================================

            migrationBuilder.CreateTable(
                name: "LoadCarrierAssignments",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    LoadId = table.Column<Guid>(nullable: false),
                    CarrierId = table.Column<Guid>(nullable: false),
                    OfferedRate = table.Column<decimal>("decimal(12,2)", nullable: true),
                    Currency = table.Column<string>(nullable: true),
                    RateConfirmationNumber = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    TenderedAt = table.Column<DateTime>(nullable: false),
                    AcceptedAt = table.Column<DateTime>(nullable: true),
                    RejectedAt = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedByUserId = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoadCarrierAssignments", x => x.Id);
                    table.ForeignKey("FK_LoadCarrierAssignments_Loads_LoadId", x => x.LoadId, "Loads", "Id");
                    table.ForeignKey("FK_LoadCarrierAssignments_Carriers_CarrierId", x => x.CarrierId, "Carriers", "Id");
                });

            migrationBuilder.CreateIndex("IX_LoadCarrierAssignments_LoadId", "LoadCarrierAssignments", "LoadId");
            migrationBuilder.CreateIndex("IX_LoadCarrierAssignments_CarrierId", "LoadCarrierAssignments", "CarrierId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoadCarrierAssignments");

            migrationBuilder.DropColumn(
                name: "Commodity",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeliveryNotes",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DispatchNotes",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PrimaryBolNumber",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PrimaryPONumber",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PrimaryProNumber",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "TotalPallets",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "TotalVolume",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "TotalWeight",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "AppointmentNumber",
                table: "OrderRoutes");

            migrationBuilder.DropColumn(
                name: "StopReference",
                table: "OrderRoutes");

            migrationBuilder.DropColumn(
                name: "IsPrefered",
                table: "OrderEquipmentRequirements");

            migrationBuilder.DropColumn(
                name: "MaxTemperature",
                table: "OrderEquipmentRequirements");

            migrationBuilder.DropColumn(
                name: "ActualCheckedInTime",
                table: "LoadStops");

            migrationBuilder.DropColumn(
                name: "ActualCheckedOutTime",
                table: "LoadStops");

            migrationBuilder.DropColumn(
                name: "AppointmentNumber",
                table: "LoadStops");

            migrationBuilder.DropColumn(
                name: "ContactName",
                table: "LoadStops");

            migrationBuilder.DropColumn(
                name: "ContactPhone",
                table: "LoadStops");

            migrationBuilder.DropColumn(
                name: "IsLateArrival",
                table: "LoadStops");

            migrationBuilder.DropColumn(
                name: "IsLateDeparture",
                table: "LoadStops");

            migrationBuilder.DropColumn(
                name: "StopReference",
                table: "LoadStops");

            migrationBuilder.DropColumn(
                name: "BolNumber",
                table: "Loads");

            migrationBuilder.DropColumn(
                name: "CarrierSCAC",
                table: "Loads");

            migrationBuilder.DropColumn(
                name: "DriverEmail",
                table: "Loads");

            migrationBuilder.DropColumn(
                name: "DriverName",
                table: "Loads");

            migrationBuilder.DropColumn(
                name: "DriverPhone",
                table: "Loads");

            migrationBuilder.DropColumn(
                name: "OnTimeDelivery",
                table: "Loads");

            migrationBuilder.DropColumn(
                name: "OnTimePickup",
                table: "Loads");

            migrationBuilder.DropColumn(
                name: "PodReceivedAt",
                table: "Loads");

            migrationBuilder.DropColumn(
                name: "PodUploadedBy",
                table: "Loads");

            migrationBuilder.DropColumn(
                name: "ProNumber",
                table: "Loads");

            migrationBuilder.DropColumn(
                name: "RateConfirmationNumber",
                table: "Loads");

            migrationBuilder.DropColumn(
                name: "TrackingNumber",
                table: "Loads");

            migrationBuilder.DropColumn(
                name: "TrailerNumber",
                table: "Loads");

            migrationBuilder.DropColumn(
                name: "TransitTimeHours",
                table: "Loads");

            migrationBuilder.DropColumn(
                name: "TruckNumber",
                table: "Loads");

            migrationBuilder.DropColumn(
                name: "CustomerReference",
                table: "LoadItems");

            migrationBuilder.DropColumn(
                name: "HazardClass",
                table: "LoadItems");

            migrationBuilder.DropColumn(
                name: "IdentificationNumber",
                table: "LoadItems");

            migrationBuilder.DropColumn(
                name: "Volume",
                table: "LoadItems");

            migrationBuilder.DropColumn(
                name: "VolumeUnit",
                table: "LoadItems");

            migrationBuilder.DropColumn(
                name: "IsPrefered",
                table: "LoadEquipment");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "LoadEquipment");

            migrationBuilder.DropColumn(
                name: "SourceOrderEquipmentRequirementId",
                table: "LoadEquipment");

            migrationBuilder.RenameColumn(
                name: "MinTemperature",
                table: "OrderEquipmentRequirements",
                newName: "RequiredTemperature");

            migrationBuilder.AlterColumn<string>(
                name: "WeightUnit",
                table: "OrderEquipmentRequirements",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "TemperatureUnit",
                table: "OrderEquipmentRequirements",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 11, 8, 28, 12, 249, DateTimeKind.Utc).AddTicks(406));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 11, 8, 28, 12, 249, DateTimeKind.Utc).AddTicks(438));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 11, 8, 28, 12, 249, DateTimeKind.Utc).AddTicks(441));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa4"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 11, 8, 28, 12, 249, DateTimeKind.Utc).AddTicks(443));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa5"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 11, 8, 28, 12, 249, DateTimeKind.Utc).AddTicks(445));
        }
    }
}
