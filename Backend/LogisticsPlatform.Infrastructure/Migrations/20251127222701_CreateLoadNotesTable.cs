using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogisticsPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateLoadNotesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LoadNotes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    LoadId = table.Column<Guid>(nullable: false),
                    Message = table.Column<string>(nullable: false),
                    CreatedByUserId = table.Column<Guid>(nullable: false),
                    IsInternal = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoadNotes", x => x.Id);

                    table.ForeignKey(
                        name: "FK_LoadNotes_Loads_LoadId",
                        column: x => x.LoadId,
                        principalTable: "Loads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);

                    table.ForeignKey(
                        name: "FK_LoadNotes_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LoadNotes_LoadId",
                table: "LoadNotes",
                column: "LoadId");

            migrationBuilder.CreateIndex(
                name: "IX_LoadNotes_CreatedByUserId",
                table: "LoadNotes",
                column: "CreatedByUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 27, 22, 16, 31, 551, DateTimeKind.Utc).AddTicks(6110));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 27, 22, 16, 31, 551, DateTimeKind.Utc).AddTicks(6125));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 27, 22, 16, 31, 551, DateTimeKind.Utc).AddTicks(6129));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa4"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 27, 22, 16, 31, 551, DateTimeKind.Utc).AddTicks(6142));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa5"),
                column: "CreatedAt",
                value: new DateTime(2025, 11, 27, 22, 16, 31, 551, DateTimeKind.Utc).AddTicks(6144));
        }
    }
}
