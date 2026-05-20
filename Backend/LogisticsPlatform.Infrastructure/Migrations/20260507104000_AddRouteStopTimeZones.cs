using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using LogisticsPlatform.Infrastructure.Persistence;

#nullable disable

namespace LogisticsPlatform.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20260507104000_AddRouteStopTimeZones")]
    public partial class AddRouteStopTimeZones : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TimeZone",
                table: "OrderRoutes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "UTC");

            migrationBuilder.AddColumn<string>(
                name: "TimeZone",
                table: "LoadStops",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "UTC");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeZone",
                table: "OrderRoutes");

            migrationBuilder.DropColumn(
                name: "TimeZone",
                table: "LoadStops");
        }
    }
}
