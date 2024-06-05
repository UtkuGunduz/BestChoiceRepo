using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BestChoice.API.Migrations
{
    /// <inheritdoc />
    public partial class DBRenew2Fix1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WishlistItems_Wishlists_WishlistId",
                table: "WishlistItems");

            migrationBuilder.DropTable(
                name: "Wishlists");

            migrationBuilder.DropIndex(
                name: "IX_WishlistItems_WishlistId",
                table: "WishlistItems");

            migrationBuilder.DropColumn(
                name: "WishlistId",
                table: "WishlistItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WishlistId",
                table: "WishlistItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Wishlists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wishlists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wishlists_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WishlistItems_WishlistId",
                table: "WishlistItems",
                column: "WishlistId");

            migrationBuilder.CreateIndex(
                name: "IX_Wishlists_UserId",
                table: "Wishlists",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_WishlistItems_Wishlists_WishlistId",
                table: "WishlistItems",
                column: "WishlistId",
                principalTable: "Wishlists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
