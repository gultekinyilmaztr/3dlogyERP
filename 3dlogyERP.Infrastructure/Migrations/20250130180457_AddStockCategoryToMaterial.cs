using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _3dlogyERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStockCategoryToMaterial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StockCategory",
                table: "Materials",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StockCategory",
                table: "Materials");
        }
    }
}
