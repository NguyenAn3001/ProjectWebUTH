using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MentorBooking.Repository.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectGroups",
                columns: table => new
                {
                    GroupId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupName = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    Topic = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    CreateBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ProjectG__149AF36A56A56A19", x => x.GroupId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    SkillId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Skills__DFA0918791FCB829", x => x.SkillId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectProgress",
                columns: table => new
                {
                    ProgressId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    UpdateAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ProjectP__BAE29CA59712E6FC", x => x.ProgressId);
                    table.ForeignKey(
                        name: "FK__ProjectPr__Group__19FFD4FC",
                        column: x => x.GroupId,
                        principalTable: "ProjectGroups",
                        principalColumn: "GroupId");
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Mentors",
                columns: table => new
                {
                    MentorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExperienceYears = table.Column<byte>(type: "tinyint", nullable: false),
                    MentorDescription = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Mentors__053B7E98A1D585C9", x => x.MentorId);
                    table.ForeignKey(
                        name: "FK_Mentors_Users_MentorId",
                        column: x => x.MentorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Students__32C52B99EF2F4E93", x => x.StudentId);
                    table.ForeignKey(
                        name: "FK_Students_Users_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPoints",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PointsBalance = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__UserPoin__1788CC4C298B88EC", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_UserPoints_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Expired = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MentorSkills",
                columns: table => new
                {
                    MentorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SkillId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MentorSk__68C1778069093006", x => new { x.MentorId, x.SkillId });
                    table.ForeignKey(
                        name: "FK__MentorSki__Mento__1AF3F935",
                        column: x => x.MentorId,
                        principalTable: "Mentors",
                        principalColumn: "MentorId");
                    table.ForeignKey(
                        name: "FK__MentorSki__Skill__1BE81D6E",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "SkillId");
                });

            migrationBuilder.CreateTable(
                name: "MentorSupportSessions",
                columns: table => new
                {
                    SessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SessionCount = table.Column<byte>(type: "tinyint", nullable: false),
                    PointsPerSession = table.Column<short>(type: "smallint", nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    MentorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalPoints = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MentorSu__C9F49290299FC032", x => x.SessionId);
                    table.ForeignKey(
                        name: "FK__MentorSup__Group__1CDC41A7",
                        column: x => x.GroupId,
                        principalTable: "ProjectGroups",
                        principalColumn: "GroupId");
                    table.ForeignKey(
                        name: "FK__MentorSup__Mento__1DD065E0",
                        column: x => x.MentorId,
                        principalTable: "Mentors",
                        principalColumn: "MentorId");
                });

            migrationBuilder.CreateTable(
                name: "StudentGroups",
                columns: table => new
                {
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    JoinAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__StudentG__838C84AFD966A83D", x => new { x.StudentId, x.GroupId });
                    table.ForeignKey(
                        name: "FK__StudentGr__Group__190BB0C3",
                        column: x => x.GroupId,
                        principalTable: "ProjectGroups",
                        principalColumn: "GroupId");
                    table.ForeignKey(
                        name: "FK__StudentGr__Stude__18178C8A",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId");
                });

            migrationBuilder.CreateTable(
                name: "PointTransactions",
                columns: table => new
                {
                    TransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PointsChanged = table.Column<int>(type: "int", nullable: true),
                    TransactionType = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__PointTra__55433A6BC5192108", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK__PointTran__UserI__17236851",
                        column: x => x.UserId,
                        principalTable: "UserPoints",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "GroupFeedback",
                columns: table => new
                {
                    FeedbackId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    MentorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Rating = table.Column<byte>(type: "tinyint", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__GroupFee__6A4BEDD6E709A159", x => x.FeedbackId);
                    table.ForeignKey(
                        name: "FK__GroupFeed__Group__247D636F",
                        column: x => x.GroupId,
                        principalTable: "ProjectGroups",
                        principalColumn: "GroupId");
                    table.ForeignKey(
                        name: "FK__GroupFeed__Mento__257187A8",
                        column: x => x.MentorId,
                        principalTable: "Mentors",
                        principalColumn: "MentorId");
                    table.ForeignKey(
                        name: "FK__GroupFeed__Sessi__2665ABE1",
                        column: x => x.SessionId,
                        principalTable: "MentorSupportSessions",
                        principalColumn: "SessionId");
                });

            migrationBuilder.CreateTable(
                name: "MentorFeedback",
                columns: table => new
                {
                    FeedbackId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MentorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Rating = table.Column<byte>(type: "tinyint", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    CreateAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MentorFe__6A4BEDD6B818C861", x => x.FeedbackId);
                    table.ForeignKey(
                        name: "FK__MentorFee__Mento__21A0F6C4",
                        column: x => x.MentorId,
                        principalTable: "Mentors",
                        principalColumn: "MentorId");
                    table.ForeignKey(
                        name: "FK__MentorFee__Sessi__22951AFD",
                        column: x => x.SessionId,
                        principalTable: "MentorSupportSessions",
                        principalColumn: "SessionId");
                    table.ForeignKey(
                        name: "FK__MentorFee__Stude__23893F36",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId");
                });

            migrationBuilder.CreateTable(
                name: "MentorWorkSchedules",
                columns: table => new
                {
                    ScheduleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkDate = table.Column<DateOnly>(type: "date", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    UnavailableDate = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MentorWo__9C8A5B490B6777AE", x => x.ScheduleId);
                    table.ForeignKey(
                        name: "FK__MentorWor__Sessi__1EC48A19",
                        column: x => x.SessionId,
                        principalTable: "MentorSupportSessions",
                        principalColumn: "SessionId");
                });

            migrationBuilder.CreateTable(
                name: "StudentsPaymentSession",
                columns: table => new
                {
                    SessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PointsChanged = table.Column<int>(type: "int", nullable: false),
                    PaidAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Students__4AD8C0293BC278AC", x => new { x.SessionId, x.StudentId });
                    table.ForeignKey(
                        name: "FK__StudentsP__Sessi__1FB8AE52",
                        column: x => x.SessionId,
                        principalTable: "MentorSupportSessions",
                        principalColumn: "SessionId");
                    table.ForeignKey(
                        name: "FK__StudentsP__Stude__20ACD28B",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupFeedback_GroupId",
                table: "GroupFeedback",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupFeedback_MentorId",
                table: "GroupFeedback",
                column: "MentorId");

            migrationBuilder.CreateIndex(
                name: "UQ__GroupFee__6A4BEDD7BC495357",
                table: "GroupFeedback",
                column: "FeedbackId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__GroupFee__C9F492913EE01081",
                table: "GroupFeedback",
                column: "SessionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MentorFeedback_MentorId",
                table: "MentorFeedback",
                column: "MentorId");

            migrationBuilder.CreateIndex(
                name: "IX_MentorFeedback_SessionId",
                table: "MentorFeedback",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_MentorFeedback_StudentId",
                table: "MentorFeedback",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "UQ__MentorFe__6A4BEDD72A8F378A",
                table: "MentorFeedback",
                column: "FeedbackId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Mentors__053B7E99EBF8FADB",
                table: "Mentors",
                column: "MentorId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Mentors__1788CC4D5F137BEE",
                table: "Mentors",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MentorSkills_SkillId",
                table: "MentorSkills",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_MentorSupportSessions_GroupId",
                table: "MentorSupportSessions",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_MentorSupportSessions_MentorId",
                table: "MentorSupportSessions",
                column: "MentorId");

            migrationBuilder.CreateIndex(
                name: "UQ__MentorSu__C9F4929183F9C51A",
                table: "MentorSupportSessions",
                column: "SessionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MentorWorkSchedules_SessionId",
                table: "MentorWorkSchedules",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "UQ__MentorWo__9C8A5B48345AF3A7",
                table: "MentorWorkSchedules",
                column: "ScheduleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PointTransactions_UserId",
                table: "PointTransactions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "UQ__PointTra__55433A6A702A041C",
                table: "PointTransactions",
                column: "TransactionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__ProjectG__149AF36B441F56CF",
                table: "ProjectGroups",
                column: "GroupId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectProgress_GroupId",
                table: "ProjectProgress",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "UQ__ProjectP__BAE29CA42BD6879C",
                table: "ProjectProgress",
                column: "ProgressId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Roles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UQ__Skills__DFA09186EEFBBC72",
                table: "Skills",
                column: "SkillId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentGroups_GroupId",
                table: "StudentGroups",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "UQ__StudentG__32C52B98BDCB51AE",
                table: "StudentGroups",
                column: "StudentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Students__1788CC4DF24ECFC0",
                table: "Students",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Students__32C52B9871763433",
                table: "Students",
                column: "StudentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentsPaymentSession_StudentId",
                table: "StudentsPaymentSession",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "UQ__Students__C9F492911F34EC6C",
                table: "StudentsPaymentSession",
                column: "SessionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "UQ__UserPoin__1788CC4D87499114",
                table: "UserPoints",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Users",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupFeedback");

            migrationBuilder.DropTable(
                name: "MentorFeedback");

            migrationBuilder.DropTable(
                name: "MentorSkills");

            migrationBuilder.DropTable(
                name: "MentorWorkSchedules");

            migrationBuilder.DropTable(
                name: "PointTransactions");

            migrationBuilder.DropTable(
                name: "ProjectProgress");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "StudentGroups");

            migrationBuilder.DropTable(
                name: "StudentsPaymentSession");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropTable(
                name: "UserPoints");

            migrationBuilder.DropTable(
                name: "MentorSupportSessions");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "ProjectGroups");

            migrationBuilder.DropTable(
                name: "Mentors");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
