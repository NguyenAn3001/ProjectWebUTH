using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MentorBooking.Repository.Migrations
{
    /// <inheritdoc />
    public partial class deleteUniqueStudentIdOnStudentGroupTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "UQ__StudentG__32C52B98BDCB51AE",
                table: "StudentGroups",
                newName: "IX_StudentGroups_StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_StudentGroups_StudentId",
                table: "StudentGroups",
                newName: "UQ__StudentG__32C52B98BDCB51AE");
        }
    }
}
