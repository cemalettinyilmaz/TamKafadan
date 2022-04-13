using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TamKafadan.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Konular",
                columns: table => new
                {
                    KonuId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KonuAdi = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Konular", x => x.KonuId);
                });

            migrationBuilder.CreateTable(
                name: "Yazarlar",
                columns: table => new
                {
                    YazarId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    YazarAd = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Biografi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KullaniciAdi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResimYolu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SilindiMi = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Yazarlar", x => x.YazarId);
                });

            migrationBuilder.CreateTable(
                name: "KonuYazar",
                columns: table => new
                {
                    KonulariKonuId = table.Column<int>(type: "int", nullable: false),
                    YazarlarYazarId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KonuYazar", x => new { x.KonulariKonuId, x.YazarlarYazarId });
                    table.ForeignKey(
                        name: "FK_KonuYazar_Konular_KonulariKonuId",
                        column: x => x.KonulariKonuId,
                        principalTable: "Konular",
                        principalColumn: "KonuId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KonuYazar_Yazarlar_YazarlarYazarId",
                        column: x => x.YazarlarYazarId,
                        principalTable: "Yazarlar",
                        principalColumn: "YazarId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Makaleler",
                columns: table => new
                {
                    MakaleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Baslik = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Icerik = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OlusuturulmaZamani = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GoruntulenmeSayisi = table.Column<int>(type: "int", nullable: false),
                    YazarId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Makaleler", x => x.MakaleId);
                    table.ForeignKey(
                        name: "FK_Makaleler_Yazarlar_YazarId",
                        column: x => x.YazarId,
                        principalTable: "Yazarlar",
                        principalColumn: "YazarId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KonuMakale",
                columns: table => new
                {
                    KonulariKonuId = table.Column<int>(type: "int", nullable: false),
                    MakalelerMakaleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KonuMakale", x => new { x.KonulariKonuId, x.MakalelerMakaleId });
                    table.ForeignKey(
                        name: "FK_KonuMakale_Konular_KonulariKonuId",
                        column: x => x.KonulariKonuId,
                        principalTable: "Konular",
                        principalColumn: "KonuId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KonuMakale_Makaleler_MakalelerMakaleId",
                        column: x => x.MakalelerMakaleId,
                        principalTable: "Makaleler",
                        principalColumn: "MakaleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KonuMakale_MakalelerMakaleId",
                table: "KonuMakale",
                column: "MakalelerMakaleId");

            migrationBuilder.CreateIndex(
                name: "IX_KonuYazar_YazarlarYazarId",
                table: "KonuYazar",
                column: "YazarlarYazarId");

            migrationBuilder.CreateIndex(
                name: "IX_Makaleler_YazarId",
                table: "Makaleler",
                column: "YazarId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KonuMakale");

            migrationBuilder.DropTable(
                name: "KonuYazar");

            migrationBuilder.DropTable(
                name: "Makaleler");

            migrationBuilder.DropTable(
                name: "Konular");

            migrationBuilder.DropTable(
                name: "Yazarlar");
        }
    }
}
