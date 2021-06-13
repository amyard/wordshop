using Microsoft.EntityFrameworkCore.Migrations;

namespace WordShop.Data.Migrations
{
    public partial class DayInfoEntetiesInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DayInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Position = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DayInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DayInfoBlocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DayInfoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DayInfoBlocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DayInfoBlocks_DayInfo_DayInfoId",
                        column: x => x.DayInfoId,
                        principalTable: "DayInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DayInfoSequenceItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DayInfoBlockId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DayInfoSequenceItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DayInfoSequenceItems_DayInfoBlocks_DayInfoBlockId",
                        column: x => x.DayInfoBlockId,
                        principalTable: "DayInfoBlocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DayInfoBlocks_DayInfoId",
                table: "DayInfoBlocks",
                column: "DayInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_DayInfoSequenceItems_DayInfoBlockId",
                table: "DayInfoSequenceItems",
                column: "DayInfoBlockId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DayInfoSequenceItems");

            migrationBuilder.DropTable(
                name: "DayInfoBlocks");

            migrationBuilder.DropTable(
                name: "DayInfo");
        }
    }
}
