using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MentorBooking.Repository.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDeleteRowCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__GroupFeed__Group__247D636F",
                table: "GroupFeedback");

            migrationBuilder.DropForeignKey(
                name: "FK__GroupFeed__Mento__257187A8",
                table: "GroupFeedback");

            migrationBuilder.DropForeignKey(
                name: "FK__MentorSup__Group__1CDC41A7",
                table: "MentorSupportSessions");

            migrationBuilder.DropForeignKey(
                name: "FK__MentorSup__Mento__1DD065E0",
                table: "MentorSupportSessions");

            migrationBuilder.DropForeignKey(
                name: "FK__ProjectPr__Group__19FFD4FC",
                table: "ProjectProgress");

            migrationBuilder.DropForeignKey(
                name: "FK__StudentGr__Group__190BB0C3",
                table: "StudentGroups");

            migrationBuilder.DropForeignKey(
                name: "FK__StudentGr__Stude__18178C8A",
                table: "StudentGroups");

            migrationBuilder.AddForeignKey(
                name: "FK__GroupFeed__Group__247D636F",
                table: "GroupFeedback",
                column: "GroupId",
                principalTable: "ProjectGroups",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__GroupFeed__Mento__257187A8",
                table: "GroupFeedback",
                column: "MentorId",
                principalTable: "Mentors",
                principalColumn: "MentorId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__MentorSup__Group__1CDC41A7",
                table: "MentorSupportSessions",
                column: "GroupId",
                principalTable: "ProjectGroups",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__MentorSup__Mento__1DD065E0",
                table: "MentorSupportSessions",
                column: "MentorId",
                principalTable: "Mentors",
                principalColumn: "MentorId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__ProjectPr__Group__19FFD4FC",
                table: "ProjectProgress",
                column: "GroupId",
                principalTable: "ProjectGroups",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__StudentGr__Group__190BB0C3",
                table: "StudentGroups",
                column: "GroupId",
                principalTable: "ProjectGroups",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__StudentGr__Stude__18178C8A",
                table: "StudentGroups",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "StudentId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__GroupFeed__Group__247D636F",
                table: "GroupFeedback");

            migrationBuilder.DropForeignKey(
                name: "FK__GroupFeed__Mento__257187A8",
                table: "GroupFeedback");

            migrationBuilder.DropForeignKey(
                name: "FK__MentorSup__Group__1CDC41A7",
                table: "MentorSupportSessions");

            migrationBuilder.DropForeignKey(
                name: "FK__MentorSup__Mento__1DD065E0",
                table: "MentorSupportSessions");

            migrationBuilder.DropForeignKey(
                name: "FK__ProjectPr__Group__19FFD4FC",
                table: "ProjectProgress");

            migrationBuilder.DropForeignKey(
                name: "FK__StudentGr__Group__190BB0C3",
                table: "StudentGroups");

            migrationBuilder.DropForeignKey(
                name: "FK__StudentGr__Stude__18178C8A",
                table: "StudentGroups");

            migrationBuilder.AddForeignKey(
                name: "FK__GroupFeed__Group__247D636F",
                table: "GroupFeedback",
                column: "GroupId",
                principalTable: "ProjectGroups",
                principalColumn: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK__GroupFeed__Mento__257187A8",
                table: "GroupFeedback",
                column: "MentorId",
                principalTable: "Mentors",
                principalColumn: "MentorId");

            migrationBuilder.AddForeignKey(
                name: "FK__MentorSup__Group__1CDC41A7",
                table: "MentorSupportSessions",
                column: "GroupId",
                principalTable: "ProjectGroups",
                principalColumn: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK__MentorSup__Mento__1DD065E0",
                table: "MentorSupportSessions",
                column: "MentorId",
                principalTable: "Mentors",
                principalColumn: "MentorId");

            migrationBuilder.AddForeignKey(
                name: "FK__ProjectPr__Group__19FFD4FC",
                table: "ProjectProgress",
                column: "GroupId",
                principalTable: "ProjectGroups",
                principalColumn: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK__StudentGr__Group__190BB0C3",
                table: "StudentGroups",
                column: "GroupId",
                principalTable: "ProjectGroups",
                principalColumn: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK__StudentGr__Stude__18178C8A",
                table: "StudentGroups",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "StudentId");
        }
    }
}
