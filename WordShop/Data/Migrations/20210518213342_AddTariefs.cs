using Microsoft.EntityFrameworkCore.Migrations;

namespace WordShop.Data.Migrations
{
    public partial class AddTariefs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TariffId",
                table: "CustomerInfos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Tariffs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OldPrice = table.Column<int>(type: "int", nullable: false),
                    NewPrice = table.Column<int>(type: "int", nullable: false),
                    Courses = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tariffs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerInfos_TariffId",
                table: "CustomerInfos",
                column: "TariffId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerInfos_Tariffs_TariffId",
                table: "CustomerInfos",
                column: "TariffId",
                principalTable: "Tariffs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerInfos_Tariffs_TariffId",
                table: "CustomerInfos");

            migrationBuilder.DropTable(
                name: "Tariffs");

            migrationBuilder.DropIndex(
                name: "IX_CustomerInfos_TariffId",
                table: "CustomerInfos");

            migrationBuilder.DropColumn(
                name: "TariffId",
                table: "CustomerInfos");
        }
    }
}
