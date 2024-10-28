using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MentorBooking.Repository.Migrations
{
    /// <inheritdoc />
    public partial class editForeignKeyOfMentor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mentors_Users_MentorId",
                table: "Mentors");

            migrationBuilder.AddForeignKey(
                name: "FK_Mentors_Users_UserId",
                table: "Mentors",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mentors_Users_UserId",
                table: "Mentors");

            migrationBuilder.AddForeignKey(
                name: "FK_Mentors_Users_MentorId",
                table: "Mentors",
                column: "MentorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
