using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class addedpropertiestoupgraderecordcomponententityQuantityusedPriceperUnit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "UpgradeRecordComponents",
                newName: "QuantityUsed");

            migrationBuilder.AddColumn<decimal>(
                name: "PricePerUnit",
                table: "UpgradeRecordComponents",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PricePerUnit",
                table: "UpgradeRecordComponents");

            migrationBuilder.RenameColumn(
                name: "QuantityUsed",
                table: "UpgradeRecordComponents",
                newName: "Quantity");
        }
    }
}
