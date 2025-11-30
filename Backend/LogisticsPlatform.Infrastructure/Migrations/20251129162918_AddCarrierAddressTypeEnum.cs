using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Security.Claims;

#nullable disable

namespace LogisticsPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCarrierAddressTypeEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1️⃣ Shto kolonë TEMP për enum
            migrationBuilder.AddColumn<int>(
                name: "TypeTemp",
                table: "CarrierAddresses",
                nullable: false,
                defaultValue: 1); // General

            // 2️⃣ Map STRING → ENUM INT
            migrationBuilder.Sql("""
        UPDATE CarrierAddresses SET TypeTemp = 1 WHERE Type = 'Main';
        UPDATE CarrierAddresses SET TypeTemp = 2 WHERE Type = 'Billing';
        UPDATE CarrierAddresses SET TypeTemp = 3 WHERE Type = 'Mailing';
        UPDATE CarrierAddresses SET TypeTemp = 4 WHERE Type = 'Physical';
        UPDATE CarrierAddresses SET TypeTemp = 5 WHERE Type = 'Claims';
        UPDATE CarrierAddresses SET TypeTemp = 6 WHERE Type = 'General';
        UPDATE CarrierAddresses SET TypeTemp = 99 WHERE Type = 'Other';
    """);
         
            // 3️⃣ Drop kolonën e vjetër (string)
            migrationBuilder.DropColumn(
                name: "Type",
                table: "CarrierAddresses");

            // 4️⃣ Rename temp → Type
            migrationBuilder.RenameColumn(
                name: "TypeTemp",
                table: "CarrierAddresses",
                newName: "Type");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TypeTemp",
                table: "CarrierAddresses",
                nullable: false,
                defaultValue: "General");

            migrationBuilder.Sql("""
        UPDATE CarrierAddresses SET TypeTemp = 1 WHERE Type = 'Main';
        UPDATE CarrierAddresses SET TypeTemp = 2 WHERE Type = 'Billing';
        UPDATE CarrierAddresses SET TypeTemp = 3 WHERE Type = 'Mailing';
        UPDATE CarrierAddresses SET TypeTemp = 4 WHERE Type = 'Physical';
        UPDATE CarrierAddresses SET TypeTemp = 5 WHERE Type = 'Claims';
        UPDATE CarrierAddresses SET TypeTemp = 6 WHERE Type = 'General';
        UPDATE CarrierAddresses SET TypeTemp = 99 WHERE Type = 'Other';
    """);

            migrationBuilder.DropColumn(
                name: "Type",
                table: "CarrierAddresses");

            migrationBuilder.RenameColumn(
                name: "TypeTemp",
                table: "CarrierAddresses",
                newName: "Type");
        }
    }
}
