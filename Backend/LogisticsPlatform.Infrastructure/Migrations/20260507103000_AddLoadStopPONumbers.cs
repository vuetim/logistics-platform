using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using LogisticsPlatform.Infrastructure.Persistence;

#nullable disable

namespace LogisticsPlatform.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20260507103000_AddLoadStopPONumbers")]
    public partial class AddLoadStopPONumbers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF COL_LENGTH('LoadStops', 'PONumbers') IS NULL
BEGIN
    ALTER TABLE [LoadStops] ADD [PONumbers] nvarchar(500) NULL;
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
IF COL_LENGTH('LoadStops', 'PONumbers') IS NOT NULL
BEGIN
    ALTER TABLE [LoadStops] DROP COLUMN [PONumbers];
END");
        }
    }
}
