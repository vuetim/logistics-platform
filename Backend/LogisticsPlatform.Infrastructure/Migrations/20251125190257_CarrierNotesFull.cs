using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogisticsPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CarrierNotesFull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CarrierNotes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CarrierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarrierNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarrierNotes_Carriers_CarrierId",
                        column: x => x.CarrierId,
                        principalTable: "Carriers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarrierNotes_Users_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarrierNotes_CarrierId",
                table: "CarrierNotes",
                column: "CarrierId");

            migrationBuilder.CreateIndex(
                name: "IX_CarrierNotes_CreatedByUserId",
                table: "CarrierNotes",
                column: "CreatedByUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarrierNotes");
        }
    }
}
