using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BestChoice.API.Migrations
{
    /// <inheritdoc />
    public partial class DBRenew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Campaigns_CampaignId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Manufacturer_ManufacturerId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_StockStatuses_StockStatusId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Warranties_WarrantyId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Visuals_Products_ProductId",
                table: "Visuals");

            migrationBuilder.DropTable(
                name: "Campaigns");

            migrationBuilder.DropTable(
                name: "Manufacturer");

            migrationBuilder.DropTable(
                name: "Warranties");

            migrationBuilder.DropIndex(
                name: "IX_Products_BrandId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_CampaignId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ManufacturerId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_StockStatusId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "StockStatuses");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "StockStatuses");

            migrationBuilder.DropColumn(
                name: "Barcode",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CampaignId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DiscountedPrice",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ExpiryDate",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ManufacturerId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ManufacturerInfo",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ManufacturerPartNumber",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "MaxStockLevel",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "MinStockLevel",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductionDate",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SKU",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SaleEndDate",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SaleStartDate",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "StockStatusId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "StorageConditions",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "Updated",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Brands");

            migrationBuilder.RenameColumn(
                name: "WarrantyId",
                table: "Products",
                newName: "StockId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_WarrantyId",
                table: "Products",
                newName: "IX_Products_StockId");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "Visuals",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "StockStatuses",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountAmount",
                table: "Discounts",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<DateTime>(
                name: "DiscountEndDate",
                table: "Discounts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DiscountStartDate",
                table: "Discounts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    UserId1 = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rating = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_AspNetUsers_UserId1",
                        column: x => x.UserId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reviews_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_BrandId",
                table: "Products",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ProductId",
                table: "Reviews",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId1",
                table: "Reviews",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_StockStatuses_StockId",
                table: "Products",
                column: "StockId",
                principalTable: "StockStatuses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Visuals_Products_ProductId",
                table: "Visuals",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_StockStatuses_StockId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Visuals_Products_ProductId",
                table: "Visuals");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Products_BrandId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "StockStatuses");

            migrationBuilder.DropColumn(
                name: "DiscountEndDate",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "DiscountStartDate",
                table: "Discounts");

            migrationBuilder.RenameColumn(
                name: "StockId",
                table: "Products",
                newName: "WarrantyId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_StockId",
                table: "Products",
                newName: "IX_Products_WarrantyId");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "Visuals",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "StockStatuses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "StockStatuses",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Barcode",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CampaignId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountedPrice",
                table: "Products",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiryDate",
                table: "Products",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ManufacturerId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ManufacturerInfo",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ManufacturerPartNumber",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxStockLevel",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MinStockLevel",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ProductionDate",
                table: "Products",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SKU",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SaleEndDate",
                table: "Products",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SaleStartDate",
                table: "Products",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StockStatusId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StorageConditions",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Weight",
                table: "Products",
                type: "float",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountAmount",
                table: "Discounts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Discounts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Discounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Discounts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Discounts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Categories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "Updated",
                table: "Categories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Brands",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Brands",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Brands",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Campaigns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campaigns", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Manufacturer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manufacturer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Warranties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warranties", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_BrandId",
                table: "Products",
                column: "BrandId",
                unique: true,
                filter: "[BrandId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CampaignId",
                table: "Products",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ManufacturerId",
                table: "Products",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_StockStatusId",
                table: "Products",
                column: "StockStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Campaigns_CampaignId",
                table: "Products",
                column: "CampaignId",
                principalTable: "Campaigns",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Manufacturer_ManufacturerId",
                table: "Products",
                column: "ManufacturerId",
                principalTable: "Manufacturer",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_StockStatuses_StockStatusId",
                table: "Products",
                column: "StockStatusId",
                principalTable: "StockStatuses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Warranties_WarrantyId",
                table: "Products",
                column: "WarrantyId",
                principalTable: "Warranties",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Visuals_Products_ProductId",
                table: "Visuals",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
