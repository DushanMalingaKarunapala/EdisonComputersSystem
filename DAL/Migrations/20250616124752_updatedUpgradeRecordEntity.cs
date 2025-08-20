using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class updatedUpgradeRecordEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpgradeDate",
                table: "UpgradeRecords",
                newName: "WarrantyExpiry");

            migrationBuilder.AddColumn<string>(
                name: "UpgradeDescription",
                table: "UpgradeRecords",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpgradeDescription",
                table: "UpgradeRecords");

            migrationBuilder.RenameColumn(
                name: "WarrantyExpiry",
                table: "UpgradeRecords",
                newName: "UpgradeDate");
        }
    }
}
