using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogisticsPlatform.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ConvertCustomerAddressType_String_To_Enum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TypeTemp",
                table: "CustomerAddresses",
                nullable: false,
                defaultValue: 1); 

            migrationBuilder.Sql("""
        UPDATE CustomerAddresses SET TypeTemp = 1 WHERE Type = 'Main';
        UPDATE CustomerAddresses SET TypeTemp = 2 WHERE Type = 'Billing';
        UPDATE CustomerAddresses SET TypeTemp = 3 WHERE Type = 'Shipping';
        UPDATE CustomerAddresses SET TypeTemp = 4 WHERE Type = 'Work';
        UPDATE CustomerAddresses SET TypeTemp = 5 WHERE Type = 'Warehouse';
    """);

            migrationBuilder.DropColumn(
                name: "Type",
                table: "CustomerAddresses");

            migrationBuilder.RenameColumn(
                name: "TypeTemp",
                table: "CustomerAddresses",
                newName: "Type");

            migrationBuilder.AddColumn<bool>(
                name: "IsInternal",
                table: "LoadDocuments",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TypeTemp",
                table: "CustomerAddresses",
                nullable: false,
                defaultValue: "Main");

            migrationBuilder.Sql("""
        UPDATE CustomerAddresses SET TypeTemp = 'Main' WHERE Type = 1;
        UPDATE CustomerAddresses SET TypeTemp = 'Billing' WHERE Type = 2;
        UPDATE CustomerAddresses SET TypeTemp = 'Shipping' WHERE Type = 3;
        UPDATE CustomerAddresses SET TypeTemp = 'Work' WHERE Type = 4;
        UPDATE CustomerAddresses SET TypeTemp = 'Warehouse' WHERE Type = 5;
    """);

            migrationBuilder.DropColumn(
                name: "Type",
                table: "CustomerAddresses");

            migrationBuilder.RenameColumn(
                name: "TypeTemp",
                table: "CustomerAddresses",
                newName: "Type");

            migrationBuilder.DropColumn(
                name: "IsInternal",
                table: "LoadDocuments");
        }
    }
}
