using Microsoft.EntityFrameworkCore.Migrations;

namespace SharpCrokite.DataAccess.Migrations
{
    public partial class AddQualityToMaterial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Quality",
                table: "Materials",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "HarvestableId",
                table: "MaterialContents",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quality",
                table: "Materials");

            migrationBuilder.AlterColumn<int>(
                name: "HarvestableId",
                table: "MaterialContents",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }
    }
}
