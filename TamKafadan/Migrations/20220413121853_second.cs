using Microsoft.EntityFrameworkCore.Migrations;

namespace TamKafadan.Migrations
{
    public partial class second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SilindiMi",
                table: "Yazarlar",
                newName: "IlkMi");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IlkMi",
                table: "Yazarlar",
                newName: "SilindiMi");
        }
    }
}
