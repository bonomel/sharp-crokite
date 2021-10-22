using Microsoft.EntityFrameworkCore.Migrations;

namespace SharpCrokite.DataAccess.Migrations
{
    public partial class AddNavigationPropertyToMaterialContent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaterialContents_Harvestables_HarvestableId",
                table: "MaterialContents");

            migrationBuilder.AlterColumn<int>(
                name: "MaterialId",
                table: "MaterialContents",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "HarvestableId",
                table: "MaterialContents",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialContents_MaterialId",
                table: "MaterialContents",
                column: "MaterialId");

            migrationBuilder.AddForeignKey(
                name: "FK_MaterialContents_Harvestables_HarvestableId",
                table: "MaterialContents",
                column: "HarvestableId",
                principalTable: "Harvestables",
                principalColumn: "HarvestableId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MaterialContents_Materials_MaterialId",
                table: "MaterialContents",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "MaterialId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaterialContents_Harvestables_HarvestableId",
                table: "MaterialContents");

            migrationBuilder.DropForeignKey(
                name: "FK_MaterialContents_Materials_MaterialId",
                table: "MaterialContents");

            migrationBuilder.DropIndex(
                name: "IX_MaterialContents_MaterialId",
                table: "MaterialContents");

            migrationBuilder.AlterColumn<int>(
                name: "MaterialId",
                table: "MaterialContents",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "HarvestableId",
                table: "MaterialContents",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MaterialContents_Harvestables_HarvestableId",
                table: "MaterialContents",
                column: "HarvestableId",
                principalTable: "Harvestables",
                principalColumn: "HarvestableId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
