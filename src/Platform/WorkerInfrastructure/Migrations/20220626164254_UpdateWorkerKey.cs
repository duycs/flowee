using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkerInfrastructure.Migrations
{
    public partial class UpdateWorkerKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "WorkerSkills");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "WorkerRoles");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "WorkerGroups");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "WorkerSkills",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "WorkerRoles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "WorkerGroups",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
