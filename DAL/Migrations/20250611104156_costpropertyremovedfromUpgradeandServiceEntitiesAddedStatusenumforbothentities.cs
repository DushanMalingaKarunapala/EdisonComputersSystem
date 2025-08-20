using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class costpropertyremovedfromUpgradeandServiceEntitiesAddedStatusenumforbothentities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cost",
                table: "UpgradeRecords");

            migrationBuilder.DropColumn(
                name: "Cost",
                table: "ServiceRecords");

            migrationBuilder.DropColumn(
                name: "ServiceType",
                table: "RepairRecords");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "RepairRecords",
                newName: "RepairStatus");

            migrationBuilder.AddColumn<string>(
                name: "UpgradeStatus",
                table: "UpgradeRecords",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ServiceStatus",
                table: "ServiceRecords",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpgradeStatus",
                table: "UpgradeRecords");

            migrationBuilder.DropColumn(
                name: "ServiceStatus",
                table: "ServiceRecords");

            migrationBuilder.RenameColumn(
                name: "RepairStatus",
                table: "RepairRecords",
                newName: "Status");

            migrationBuilder.AddColumn<decimal>(
                name: "Cost",
                table: "UpgradeRecords",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Cost",
                table: "ServiceRecords",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ServiceType",
                table: "RepairRecords",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
