using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalogInfrastructure.DataAccess.Migrations
{
    public partial class UpdateEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Currencies",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "DateDeleted",
                table: "Currencies",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "Currencies",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "Currencies");
        }
    }
}
