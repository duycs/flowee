using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalogInfrastructure.DataAccess.Migrations
{
    public partial class AddCatalogDbContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Name = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Addons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SpecificationId = table.Column<int>(type: "int", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    CurrencyId = table.Column<int>(type: "int", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DateDeleted = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addons_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Catalogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(type: "varchar(36)", maxLength: 36, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    QuantityAvailable = table.Column<int>(type: "int", nullable: true),
                    PriceStandar = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    CurrencyId = table.Column<int>(type: "int", nullable: true),
                    SpecificationId = table.Column<int>(type: "int", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DateDeleted = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Catalogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Catalogs_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CatalogAddons",
                columns: table => new
                {
                    CatalogId = table.Column<int>(type: "int", nullable: false),
                    AddonId = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DateDeleted = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogAddons", x => new { x.CatalogId, x.AddonId });
                    table.ForeignKey(
                        name: "FK_CatalogAddons_Addons_AddonId",
                        column: x => x.AddonId,
                        principalTable: "Addons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CatalogAddons_Catalogs_CatalogId",
                        column: x => x.CatalogId,
                        principalTable: "Catalogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Addons_CurrencyId",
                table: "Addons",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogAddons_AddonId",
                table: "CatalogAddons",
                column: "AddonId");

            migrationBuilder.CreateIndex(
                name: "IX_Catalogs_CurrencyId",
                table: "Catalogs",
                column: "CurrencyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CatalogAddons");

            migrationBuilder.DropTable(
                name: "Addons");

            migrationBuilder.DropTable(
                name: "Catalogs");

            migrationBuilder.DropTable(
                name: "Currencies");
        }
    }
}
