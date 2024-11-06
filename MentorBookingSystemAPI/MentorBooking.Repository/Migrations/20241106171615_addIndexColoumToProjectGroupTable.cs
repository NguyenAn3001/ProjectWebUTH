using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MentorBooking.Repository.Migrations
{
    /// <inheritdoc />
    public partial class addIndexColoumToProjectGroupTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProgressIndex",
                table: "ProjectProgress",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProgressIndex",
                table: "ProjectProgress");
        }
    }
}
