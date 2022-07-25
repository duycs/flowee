using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillInfrastructure.DataAccess.Migrations
{
    public partial class UpdateEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "WorkerSkillLevels",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "DateDeleted",
                table: "WorkerSkillLevels",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "WorkerSkillLevels",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "SpecificationSkillLevels",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "DateDeleted",
                table: "SpecificationSkillLevels",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "SpecificationSkillLevels",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "WorkerSkillLevels");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "WorkerSkillLevels");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "WorkerSkillLevels");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "SpecificationSkillLevels");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "SpecificationSkillLevels");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "SpecificationSkillLevels");
        }
    }
}
