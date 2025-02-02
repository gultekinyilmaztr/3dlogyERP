using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _3dlogyERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateEymens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipment_EquipmentTypes_EquipmentTypeId",
                table: "Equipment");

            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceRecord_Equipment_EquipmentId",
                table: "MaintenanceRecord");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderServices_Equipment_EquipmentId",
                table: "OrderServices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Equipment",
                table: "Equipment");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "MaterialTypes");

            migrationBuilder.DropColumn(
                name: "StockCategory",
                table: "Materials");

            migrationBuilder.RenameTable(
                name: "Equipment",
                newName: "Equipments");

            migrationBuilder.RenameIndex(
                name: "IX_Equipment_SerialNumber",
                table: "Equipments",
                newName: "IX_Equipments_SerialNumber");

            migrationBuilder.RenameIndex(
                name: "IX_Equipment_EquipmentTypeId",
                table: "Equipments",
                newName: "IX_Equipments_EquipmentTypeId");

            migrationBuilder.AddColumn<int>(
                name: "StockCategoryId",
                table: "MaterialTypes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StockCategoryId",
                table: "Materials",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Equipments",
                table: "Equipments",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "StockCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockCategories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MaterialTypes_StockCategoryId",
                table: "MaterialTypes",
                column: "StockCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Materials_StockCategoryId",
                table: "Materials",
                column: "StockCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipments_EquipmentTypes_EquipmentTypeId",
                table: "Equipments",
                column: "EquipmentTypeId",
                principalTable: "EquipmentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceRecord_Equipments_EquipmentId",
                table: "MaintenanceRecord",
                column: "EquipmentId",
                principalTable: "Equipments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_StockCategories_StockCategoryId",
                table: "Materials",
                column: "StockCategoryId",
                principalTable: "StockCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_MaterialTypes_StockCategories_StockCategoryId",
                table: "MaterialTypes",
                column: "StockCategoryId",
                principalTable: "StockCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderServices_Equipments_EquipmentId",
                table: "OrderServices",
                column: "EquipmentId",
                principalTable: "Equipments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Equipments_EquipmentTypes_EquipmentTypeId",
                table: "Equipments");

            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceRecord_Equipments_EquipmentId",
                table: "MaintenanceRecord");

            migrationBuilder.DropForeignKey(
                name: "FK_Materials_StockCategories_StockCategoryId",
                table: "Materials");

            migrationBuilder.DropForeignKey(
                name: "FK_MaterialTypes_StockCategories_StockCategoryId",
                table: "MaterialTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderServices_Equipments_EquipmentId",
                table: "OrderServices");

            migrationBuilder.DropTable(
                name: "StockCategories");

            migrationBuilder.DropIndex(
                name: "IX_MaterialTypes_StockCategoryId",
                table: "MaterialTypes");

            migrationBuilder.DropIndex(
                name: "IX_Materials_StockCategoryId",
                table: "Materials");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Equipments",
                table: "Equipments");

            migrationBuilder.DropColumn(
                name: "StockCategoryId",
                table: "MaterialTypes");

            migrationBuilder.DropColumn(
                name: "StockCategoryId",
                table: "Materials");

            migrationBuilder.RenameTable(
                name: "Equipments",
                newName: "Equipment");

            migrationBuilder.RenameIndex(
                name: "IX_Equipments_SerialNumber",
                table: "Equipment",
                newName: "IX_Equipment_SerialNumber");

            migrationBuilder.RenameIndex(
                name: "IX_Equipments_EquipmentTypeId",
                table: "Equipment",
                newName: "IX_Equipment_EquipmentTypeId");

            migrationBuilder.AddColumn<int>(
                name: "Category",
                table: "MaterialTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StockCategory",
                table: "Materials",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Equipment",
                table: "Equipment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Equipment_EquipmentTypes_EquipmentTypeId",
                table: "Equipment",
                column: "EquipmentTypeId",
                principalTable: "EquipmentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceRecord_Equipment_EquipmentId",
                table: "MaintenanceRecord",
                column: "EquipmentId",
                principalTable: "Equipment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderServices_Equipment_EquipmentId",
                table: "OrderServices",
                column: "EquipmentId",
                principalTable: "Equipment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
