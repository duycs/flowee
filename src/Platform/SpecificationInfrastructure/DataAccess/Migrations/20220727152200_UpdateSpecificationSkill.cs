using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpecificationInfrastructure.DataAccess.Migrations
{
    public partial class UpdateSpecificationSkill : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpecificationSkill_Specifications_SpecificationId",
                table: "SpecificationSkill");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SpecificationSkill",
                table: "SpecificationSkill");

            migrationBuilder.RenameTable(
                name: "SpecificationSkill",
                newName: "SpecificationSkills");

            migrationBuilder.RenameIndex(
                name: "IX_SpecificationSkill_SpecificationId",
                table: "SpecificationSkills",
                newName: "IX_SpecificationSkills_SpecificationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SpecificationSkills",
                table: "SpecificationSkills",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SpecificationSkills_Specifications_SpecificationId",
                table: "SpecificationSkills",
                column: "SpecificationId",
                principalTable: "Specifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpecificationSkills_Specifications_SpecificationId",
                table: "SpecificationSkills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SpecificationSkills",
                table: "SpecificationSkills");

            migrationBuilder.RenameTable(
                name: "SpecificationSkills",
                newName: "SpecificationSkill");

            migrationBuilder.RenameIndex(
                name: "IX_SpecificationSkills_SpecificationId",
                table: "SpecificationSkill",
                newName: "IX_SpecificationSkill_SpecificationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SpecificationSkill",
                table: "SpecificationSkill",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SpecificationSkill_Specifications_SpecificationId",
                table: "SpecificationSkill",
                column: "SpecificationId",
                principalTable: "Specifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
