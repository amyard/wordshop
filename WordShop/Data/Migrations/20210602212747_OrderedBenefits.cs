using Microsoft.EntityFrameworkCore.Migrations;

namespace WordShop.Data.Migrations
{
    public partial class OrderedBenefits : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TariffBenefitOrdered",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderPosition = table.Column<int>(type: "int", nullable: false),
                    Courses = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    TariffBenefitId = table.Column<int>(type: "int", nullable: false),
                    AdvantageTariffId = table.Column<int>(type: "int", nullable: false),
                    DisadvantageTariffId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TariffBenefitOrdered", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TariffBenefitOrdered_TariffBenefits_TariffBenefitId",
                        column: x => x.TariffBenefitId,
                        principalTable: "TariffBenefits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TariffBenefitOrdered_Tariffs_AdvantageTariffId",
                        column: x => x.AdvantageTariffId,
                        principalTable: "Tariffs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TariffBenefitOrdered_Tariffs_DisadvantageTariffId",
                        column: x => x.DisadvantageTariffId,
                        principalTable: "Tariffs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TariffBenefitOrdered_AdvantageTariffId",
                table: "TariffBenefitOrdered",
                column: "AdvantageTariffId");

            migrationBuilder.CreateIndex(
                name: "IX_TariffBenefitOrdered_DisadvantageTariffId",
                table: "TariffBenefitOrdered",
                column: "DisadvantageTariffId");

            migrationBuilder.CreateIndex(
                name: "IX_TariffBenefitOrdered_TariffBenefitId",
                table: "TariffBenefitOrdered",
                column: "TariffBenefitId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TariffBenefitOrdered");
        }
    }
}
