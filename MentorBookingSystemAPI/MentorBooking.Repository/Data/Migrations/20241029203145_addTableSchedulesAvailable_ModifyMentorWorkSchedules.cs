using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MentorBooking.Repository.Migrations
{
    /// <inheritdoc />
    public partial class addTableSchedulesAvailable_ModifyMentorWorkSchedules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "MentorWorkSchedules");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "MentorWorkSchedules");

            migrationBuilder.DropColumn(
                name: "WorkDate",
                table: "MentorWorkSchedules");

            migrationBuilder.AlterColumn<bool>(
                name: "UnavailableDate",
                table: "MentorWorkSchedules",
                type: "bit",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AddColumn<Guid>(
                name: "ScheduleAvailableId",
                table: "MentorWorkSchedules",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "SchedulesAvailable",
                columns: table => new
                {
                    ScheduleAvailableId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MentorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FreeDay = table.Column<DateOnly>(type: "date", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchedulesAvailable", x => x.ScheduleAvailableId);
                    table.ForeignKey(
                        name: "FK_SchedulesAvailable_Mentor",
                        column: x => x.MentorId,
                        principalTable: "Mentors",
                        principalColumn: "MentorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MentorWorkSchedules_ScheduleAvailableId",
                table: "MentorWorkSchedules",
                column: "ScheduleAvailableId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SchedulesAvailable_MentorId",
                table: "SchedulesAvailable",
                column: "MentorId");

            migrationBuilder.AddForeignKey(
                name: "FK_SchedulesAvailable_MentorWorkSchedule",
                table: "MentorWorkSchedules",
                column: "ScheduleAvailableId",
                principalTable: "SchedulesAvailable",
                principalColumn: "ScheduleAvailableId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchedulesAvailable_MentorWorkSchedule",
                table: "MentorWorkSchedules");

            migrationBuilder.DropTable(
                name: "SchedulesAvailable");

            migrationBuilder.DropIndex(
                name: "IX_MentorWorkSchedules_ScheduleAvailableId",
                table: "MentorWorkSchedules");

            migrationBuilder.DropColumn(
                name: "ScheduleAvailableId",
                table: "MentorWorkSchedules");

            migrationBuilder.AlterColumn<byte>(
                name: "UnavailableDate",
                table: "MentorWorkSchedules",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<TimeOnly>(
                name: "EndTime",
                table: "MentorWorkSchedules",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "StartTime",
                table: "MentorWorkSchedules",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<DateOnly>(
                name: "WorkDate",
                table: "MentorWorkSchedules",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }
    }
}
