using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogisticsPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SyncCosts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChargeParty",
                table: "OrderCosts");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "OrderCosts");

            migrationBuilder.DropColumn(
                name: "CopyToLoad",
                table: "OrderCosts");

            migrationBuilder.DropColumn(
                name: "CostDate",
                table: "OrderCosts");

            migrationBuilder.DropColumn(
                name: "CostType",
                table: "OrderCosts");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "OrderCosts");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "OrderCosts");

            migrationBuilder.DropColumn(
                name: "IsBillable",
                table: "OrderCosts");

            migrationBuilder.DropColumn(
                name: "IsEstimated",
                table: "OrderCosts");

            migrationBuilder.DropColumn(
                name: "IsPayable",
                table: "OrderCosts");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "OrderCosts");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "OrderCosts");

            migrationBuilder.AddColumn<decimal>(
                name: "CustomerRate",
                table: "Orders",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "OrderCosts",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "CustomerRate",
                table: "Loads",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "CarrierRate",
                table: "Loads",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.CreateTable(
                name: "LoadCosts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoadId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoadCosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoadCosts_Loads_LoadId",
                        column: x => x.LoadId,
                        principalTable: "Loads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderCostLineItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderCostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TypeCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TypeLabel = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Qty = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    Billable = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderCostLineItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderCostLineItems_OrderCosts_OrderCostId",
                        column: x => x.OrderCostId,
                        principalTable: "OrderCosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LoadCostLineItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoadCostId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TypeCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TypeLabel = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Qty = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    Payable = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoadCostLineItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoadCostLineItems_LoadCosts_LoadCostId",
                        column: x => x.LoadCostId,
                        principalTable: "LoadCosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_LoadCostLineItems_LoadCostId",
                table: "LoadCostLineItems",
                column: "LoadCostId");

            migrationBuilder.CreateIndex(
                name: "IX_LoadCosts_LoadId",
                table: "LoadCosts",
                column: "LoadId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderCostLineItems_OrderCostId",
                table: "OrderCostLineItems",
                column: "OrderCostId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoadCostLineItems");

            migrationBuilder.DropTable(
                name: "OrderCostLineItems");

            migrationBuilder.DropTable(
                name: "LoadCosts");

            migrationBuilder.DropColumn(
                name: "CustomerRate",
                table: "Orders");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "OrderCosts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ChargeParty",
                table: "OrderCosts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "OrderCosts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "CopyToLoad",
                table: "OrderCosts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CostDate",
                table: "OrderCosts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CostType",
                table: "OrderCosts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Currency",
                table: "OrderCosts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "OrderCosts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsBillable",
                table: "OrderCosts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsEstimated",
                table: "OrderCosts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPayable",
                table: "OrderCosts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "Quantity",
                table: "OrderCosts",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                table: "OrderCosts",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "CustomerRate",
                table: "Loads",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "CarrierRate",
                table: "Loads",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 5, 1, 1, 7, 930, DateTimeKind.Utc).AddTicks(9589));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 5, 1, 1, 7, 930, DateTimeKind.Utc).AddTicks(9610));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 5, 1, 1, 7, 930, DateTimeKind.Utc).AddTicks(9613));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa4"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 5, 1, 1, 7, 930, DateTimeKind.Utc).AddTicks(9616));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa5"),
                column: "CreatedAt",
                value: new DateTime(2025, 12, 5, 1, 1, 7, 930, DateTimeKind.Utc).AddTicks(9618));
        }
    }
}
