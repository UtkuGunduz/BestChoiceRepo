using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BestChoice.API.Migrations
{
    /// <inheritdoc />
    public partial class DBRenewFix1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StockQuantity",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "StockStatuses",
                newName: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Status",
                table: "StockStatuses",
                newName: "Name");

            migrationBuilder.AddColumn<int>(
                name: "StockQuantity",
                table: "Products",
                type: "int",
                nullable: true);
        }
    }
}
