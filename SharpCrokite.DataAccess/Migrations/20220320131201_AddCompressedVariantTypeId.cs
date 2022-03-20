using Microsoft.EntityFrameworkCore.Migrations;

namespace SharpCrokite.DataAccess.Migrations
{
    public partial class AddCompressedVariantTypeId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsCompressedVariantOfType",
                table: "Harvestables",
                newName: "IsCompressedVariantOfTypeId");

            migrationBuilder.AddColumn<int>(
                name: "CompressedVariantTypeId",
                table: "Harvestables",
                type: "INTEGER",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompressedVariantTypeId",
                table: "Harvestables");

            migrationBuilder.RenameColumn(
                name: "IsCompressedVariantOfTypeId",
                table: "Harvestables",
                newName: "IsCompressedVariantOfType");
        }
    }
}
