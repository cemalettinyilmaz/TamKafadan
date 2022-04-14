using Microsoft.EntityFrameworkCore.Migrations;

namespace TamKafadan.Migrations
{
    public partial class onayeklendi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "OnayliMi",
                table: "Makaleler",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OnayliMi",
                table: "Makaleler");
        }
    }
}
