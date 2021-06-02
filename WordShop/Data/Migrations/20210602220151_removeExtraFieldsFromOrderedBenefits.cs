using Microsoft.EntityFrameworkCore.Migrations;

namespace WordShop.Data.Migrations
{
    public partial class removeExtraFieldsFromOrderedBenefits : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Courses",
                table: "TariffBenefitOrdered");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "TariffBenefitOrdered");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Courses",
                table: "TariffBenefitOrdered",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "TariffBenefitOrdered",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
