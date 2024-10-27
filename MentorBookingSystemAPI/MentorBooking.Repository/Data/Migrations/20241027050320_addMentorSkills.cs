using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MentorBooking.Repository.Migrations
{
    public partial class addMentorSkills : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Xóa ràng buộc ngoại khóa trước
            migrationBuilder.DropForeignKey(
                name: "FK__MentorSki__Mento__1AF3F935",
                table: "MentorSkills");

            migrationBuilder.DropForeignKey(
                name: "FK__MentorSki__Skill__1BE81D6E",
                table: "MentorSkills");

            // Xóa khóa chính hiện tại
            migrationBuilder.DropPrimaryKey(
                name: "PK__Skills__DFA0918791FCB829",
                table: "Skills");

            // Nếu có index duy nhất, xóa nó
            migrationBuilder.DropIndex(
                name: "UQ__Skills__DFA09186EEFBBC72",
                table: "Skills");

            migrationBuilder.DropPrimaryKey(
                name: "PK__MentorSk__68C1778069093006",
                table: "MentorSkills");

            // Thay đổi cột SkillId: trước hết xóa cột SkillId
            migrationBuilder.DropColumn(
                name: "SkillId",
                table: "Skills");

            // Tạo lại cột SkillId với IDENTITY
            migrationBuilder.AddColumn<int>(
                name: "SkillId",
                table: "Skills",
                type: "int",
                nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1");

            // Cập nhật cột Name
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Skills",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false);

            // Thêm lại khóa chính mới cho bảng Skills
            migrationBuilder.AddPrimaryKey(
                name: "PK_Skills",
                table: "Skills",
                column: "SkillId");

            // Thêm khóa chính cho bảng MentorSkills
            migrationBuilder.AddPrimaryKey(
                name: "PK_MentorSkills",
                table: "MentorSkills",
                columns: new[] { "MentorId", "SkillId" });

            // Thêm lại các ràng buộc ngoại khóa
            migrationBuilder.AddForeignKey(
                name: "FK_MentorSkills_Mentors_MentorId",
                table: "MentorSkills",
                column: "MentorId",
                principalTable: "Mentors",
                principalColumn: "MentorId",
                onDelete: ReferentialAction.Cascade);

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
            // Quay lại các thay đổi trong phương thức Down
            migrationBuilder.DropForeignKey(
                name: "FK_MentorSkills_Mentors_MentorId",
                table: "MentorSkills");

            migrationBuilder.DropForeignKey(
                name: "FK_MentorSkills_Skills_SkillId",
                table: "MentorSkills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Skills",
                table: "Skills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MentorSkills",
                table: "MentorSkills");

            // Quay lại kiểu dữ liệu ban đầu cho trường Name
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Skills",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: false);

            // Tạo lại cột SkillId với IDENTITY
            migrationBuilder.DropColumn(
                name: "SkillId",
                table: "Skills");

            migrationBuilder.AddColumn<int>(
                name: "SkillId",
                table: "Skills",
                type: "int",
                nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Skills__DFA0918791FCB829",
                table: "Skills",
                column: "SkillId");

            migrationBuilder.AddPrimaryKey(
                name: "PK__MentorSk__68C1778069093006",
                table: "MentorSkills",
                columns: new[] { "MentorId", "SkillId" });

            // Tạo lại index nếu cần
            migrationBuilder.CreateIndex(
                name: "UQ__Skills__DFA09186EEFBBC72",
                table: "Skills",
                column: "SkillId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK__MentorSki__Mento__1AF3F935",
                table: "MentorSkills",
                column: "MentorId",
                principalTable: "Mentors",
                principalColumn: "MentorId");

            migrationBuilder.AddForeignKey(
                name: "FK__MentorSki__Skill__1BE81D6E",
                table: "MentorSkills",
                column: "SkillId",
                principalTable: "Skills",
                principalColumn: "SkillId");
        }
    }
}
