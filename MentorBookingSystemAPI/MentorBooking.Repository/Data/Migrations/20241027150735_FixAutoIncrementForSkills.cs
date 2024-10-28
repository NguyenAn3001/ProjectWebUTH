using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MentorBooking.Repository.Migrations
{
    public partial class FixAutoIncrementForSkills : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop foreign key constraints that reference SkillId
            migrationBuilder.DropForeignKey(
                name: "FK_MentorSkills_Skills_SkillId",
                table: "MentorSkills");

            // Drop the primary key constraint on Skills that depends on SkillId
            migrationBuilder.DropPrimaryKey(
                name: "PK_Skills",
                table: "Skills");

            // Drop the existing SkillId column
            migrationBuilder.DropColumn(
                name: "SkillId",
                table: "Skills");

            // Recreate the SkillId column with the IDENTITY property
            migrationBuilder.AddColumn<int>(
                name: "SkillId",
                table: "Skills",
                type: "int",
                nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1");

            // Re-add the primary key constraint
            migrationBuilder.AddPrimaryKey(
                name: "PK_Skills",
                table: "Skills",
                column: "SkillId");

            // Re-add foreign key constraints
            migrationBuilder.AddForeignKey(
                name: "FK_MentorSkills_Skills_SkillId",
                table: "MentorSkills",
                column: "SkillId",
                principalTable: "Skills",
                principalColumn: "SkillId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop foreign key constraints that reference SkillId
            migrationBuilder.DropForeignKey(
                name: "FK_MentorSkills_Skills_SkillId",
                table: "MentorSkills");

            // Drop the primary key constraint on Skills
            migrationBuilder.DropPrimaryKey(
                name: "PK_Skills",
                table: "Skills");

            // Drop the SkillId column that we just added
            migrationBuilder.DropColumn(
                name: "SkillId",
                table: "Skills");

            // Recreate the SkillId column without IDENTITY
            migrationBuilder.AddColumn<int>(
                name: "SkillId",
                table: "Skills",
                type: "int",
                nullable: false);

            // Re-add the primary key constraint without IDENTITY
            migrationBuilder.AddPrimaryKey(
                name: "PK_Skills",
                table: "Skills",
                column: "SkillId");

            // Re-add foreign key constraints (if any existed previously)
            migrationBuilder.AddForeignKey(
                name: "FK_MentorSkills_Skills_SkillId",
                table: "MentorSkills",
                column: "SkillId",
                principalTable: "Skills",
                principalColumn: "SkillId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
