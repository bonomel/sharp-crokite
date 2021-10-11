using Microsoft.EntityFrameworkCore.Migrations;

namespace MyEveToolset.Data.Migrations
{
    public partial class AddPrices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Prices",
                columns: table => new
                {
                    PriceId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MaterialId = table.Column<int>(type: "INTEGER", nullable: true),
                    HarvestableId = table.Column<int>(type: "INTEGER", nullable: true),
                    SystemId = table.Column<int>(type: "INTEGER", nullable: false),
                    BuyMax = table.Column<decimal>(type: "TEXT", nullable: false),
                    BuyMin = table.Column<decimal>(type: "TEXT", nullable: false),
                    BuyPercentile = table.Column<decimal>(type: "TEXT", nullable: false),
                    SellMax = table.Column<decimal>(type: "TEXT", nullable: false),
                    SellMin = table.Column<decimal>(type: "TEXT", nullable: false),
                    SellPercentile = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prices", x => x.PriceId);
                    table.ForeignKey(
                        name: "FK_Prices_Harvestables_HarvestableId",
                        column: x => x.HarvestableId,
                        principalTable: "Harvestables",
                        principalColumn: "HarvestableId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Prices_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "MaterialId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Prices_HarvestableId",
                table: "Prices",
                column: "HarvestableId");

            migrationBuilder.CreateIndex(
                name: "IX_Prices_MaterialId",
                table: "Prices",
                column: "MaterialId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Prices");
        }
    }
}
