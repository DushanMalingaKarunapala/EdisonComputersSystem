using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangedRepairRecordUpgradeRecordComponentUpgradeRecordComponentEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WarrantyExpiry",
                table: "UpgradeRecords");

            migrationBuilder.DropColumn(
                name: "WarrantyExpiry",
                table: "RepairRecords");

            migrationBuilder.AddColumn<int>(
                name: "WarrantyPeriodInMonths",
                table: "UpgradeRecordComponents",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WarrantyPeriodinMonths",
                table: "RepairRecords",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CollectionDate",
                table: "Jobs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DefaultWarrantyInMonths",
                table: "Components",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WarrantyPeriodInMonths",
                table: "UpgradeRecordComponents");

            migrationBuilder.DropColumn(
                name: "WarrantyPeriodinMonths",
                table: "RepairRecords");

            migrationBuilder.DropColumn(
                name: "CollectionDate",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "DefaultWarrantyInMonths",
                table: "Components");

            migrationBuilder.AddColumn<DateTime>(
                name: "WarrantyExpiry",
                table: "UpgradeRecords",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "WarrantyExpiry",
                table: "RepairRecords",
                type: "datetime2",
                nullable: true);
        }
    }
}
