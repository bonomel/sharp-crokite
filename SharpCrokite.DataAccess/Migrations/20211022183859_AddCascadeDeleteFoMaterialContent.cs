using Microsoft.EntityFrameworkCore.Migrations;

namespace SharpCrokite.DataAccess.Migrations
{
    public partial class AddCascadeDeleteFoMaterialContent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaterialContents_Harvestables_HarvestableId",
                table: "MaterialContents");

            migrationBuilder.DropForeignKey(
                name: "FK_MaterialContents_Materials_MaterialId",
                table: "MaterialContents");

            migrationBuilder.AddForeignKey(
                name: "FK_MaterialContents_Harvestables_HarvestableId",
                table: "MaterialContents",
                column: "HarvestableId",
                principalTable: "Harvestables",
                principalColumn: "HarvestableId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MaterialContents_Materials_MaterialId",
                table: "MaterialContents",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "MaterialId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaterialContents_Harvestables_HarvestableId",
                table: "MaterialContents");

            migrationBuilder.DropForeignKey(
                name: "FK_MaterialContents_Materials_MaterialId",
                table: "MaterialContents");

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
    }
}
