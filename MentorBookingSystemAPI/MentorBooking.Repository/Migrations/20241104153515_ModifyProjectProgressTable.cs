using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MentorBooking.Repository.Migrations
{
    /// <inheritdoc />
    public partial class ModifyProjectProgressTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__ProjectPr__Group__19FFD4FC",
                table: "ProjectProgress");

            migrationBuilder.RenameColumn(
                name: "GroupId",
                table: "ProjectProgress",
                newName: "SessionId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectProgress_GroupId",
                table: "ProjectProgress",
                newName: "IX_ProjectProgress_SessionId");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "ProjectProgress",
                type: "nvarchar(max)",
                maxLength: 10000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK__ProjectPr__Group__19FFD4FC",
                table: "ProjectProgress",
                column: "SessionId",
                principalTable: "MentorSupportSessions",
                principalColumn: "SessionId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__ProjectPr__Group__19FFD4FC",
                table: "ProjectProgress");

            migrationBuilder.RenameColumn(
                name: "SessionId",
                table: "ProjectProgress",
                newName: "GroupId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectProgress_SessionId",
                table: "ProjectProgress",
                newName: "IX_ProjectProgress_GroupId");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "ProjectProgress",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 10000,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK__ProjectPr__Group__19FFD4FC",
                table: "ProjectProgress",
                column: "GroupId",
                principalTable: "ProjectGroups",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
