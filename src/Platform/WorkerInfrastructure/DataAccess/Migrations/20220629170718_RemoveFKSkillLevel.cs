using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkerInfrastructure.DataAccess.Migrations
{
    public partial class RemoveFKSkillLevel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkerSkills_SkillLevels_SkillLevelId",
                table: "WorkerSkills");

            migrationBuilder.DropIndex(
                name: "IX_WorkerSkills_SkillLevelId",
                table: "WorkerSkills");

            migrationBuilder.AlterColumn<int>(
                name: "SkillLevelId",
                table: "WorkerSkills",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SkillLevelId",
                table: "WorkerSkills",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkerSkills_SkillLevelId",
                table: "WorkerSkills",
                column: "SkillLevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkerSkills_SkillLevels_SkillLevelId",
                table: "WorkerSkills",
                column: "SkillLevelId",
                principalTable: "SkillLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
