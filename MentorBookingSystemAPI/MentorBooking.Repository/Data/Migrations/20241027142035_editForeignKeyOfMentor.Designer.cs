﻿// <auto-generated />
using System;
using MentorBooking.Repository.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MentorBooking.Repository.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241027142035_editForeignKeyOfMentor")]
    partial class editForeignKeyOfMentor
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MentorBooking.Repository.Entities.GroupFeedback", b =>
                {
                    b.Property<Guid>("FeedbackId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Comment")
                        .HasMaxLength(1)
                        .HasColumnType("nvarchar(1)");

                    b.Property<DateTime>("CreateAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<int>("GroupId")
                        .HasColumnType("int");

                    b.Property<Guid>("MentorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte>("Rating")
                        .HasColumnType("tinyint");

                    b.Property<Guid>("SessionId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("FeedbackId")
                        .HasName("PK__GroupFee__6A4BEDD6E709A159");

                    b.HasIndex("GroupId");

                    b.HasIndex("MentorId");

                    b.HasIndex(new[] { "FeedbackId" }, "UQ__GroupFee__6A4BEDD7BC495357")
                        .IsUnique();

                    b.HasIndex(new[] { "SessionId" }, "UQ__GroupFee__C9F492913EE01081")
                        .IsUnique();

                    b.ToTable("GroupFeedback", (string)null);
                });

            modelBuilder.Entity("MentorBooking.Repository.Entities.Mentor", b =>
                {
                    b.Property<Guid>("MentorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<byte>("ExperienceYears")
                        .HasColumnType("tinyint");

                    b.Property<string>("MentorDescription")
                        .HasMaxLength(1)
                        .HasColumnType("nvarchar(1)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("MentorId")
                        .HasName("PK__Mentors__053B7E98A1D585C9");

                    b.HasIndex(new[] { "MentorId" }, "UQ__Mentors__053B7E99EBF8FADB")
                        .IsUnique();

                    b.HasIndex(new[] { "UserId" }, "UQ__Mentors__1788CC4D5F137BEE")
                        .IsUnique();

                    b.ToTable("Mentors");
                });

            modelBuilder.Entity("MentorBooking.Repository.Entities.MentorFeedback", b =>
                {
                    b.Property<Guid>("FeedbackId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Comment")
                        .HasMaxLength(1)
                        .HasColumnType("nvarchar(1)");

                    b.Property<DateTime>("CreateAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<Guid>("MentorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte>("Rating")
                        .HasColumnType("tinyint");

                    b.Property<Guid>("SessionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("StudentId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("FeedbackId")
                        .HasName("PK__MentorFe__6A4BEDD6B818C861");

                    b.HasIndex("MentorId");

                    b.HasIndex("SessionId");

                    b.HasIndex("StudentId");

                    b.HasIndex(new[] { "FeedbackId" }, "UQ__MentorFe__6A4BEDD72A8F378A")
                        .IsUnique();

                    b.ToTable("MentorFeedback", (string)null);
                });

            modelBuilder.Entity("MentorBooking.Repository.Entities.MentorSkill", b =>
                {
                    b.Property<Guid>("MentorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("SkillId")
                        .HasColumnType("int");

                    b.HasKey("MentorId", "SkillId");

                    b.HasIndex("SkillId");

                    b.ToTable("MentorSkills");
                });

            modelBuilder.Entity("MentorBooking.Repository.Entities.MentorSupportSession", b =>
                {
                    b.Property<Guid>("SessionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("GroupId")
                        .HasColumnType("int");

                    b.Property<Guid>("MentorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<short>("PointsPerSession")
                        .HasColumnType("smallint");

                    b.Property<byte>("SessionCount")
                        .HasColumnType("tinyint");

                    b.Property<int>("TotalPoints")
                        .HasColumnType("int");

                    b.HasKey("SessionId")
                        .HasName("PK__MentorSu__C9F49290299FC032");

                    b.HasIndex("GroupId");

                    b.HasIndex("MentorId");

                    b.HasIndex(new[] { "SessionId" }, "UQ__MentorSu__C9F4929183F9C51A")
                        .IsUnique();

                    b.ToTable("MentorSupportSessions");
                });

            modelBuilder.Entity("MentorBooking.Repository.Entities.MentorWorkSchedule", b =>
                {
                    b.Property<Guid>("ScheduleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<TimeOnly>("EndTime")
                        .HasColumnType("time");

                    b.Property<Guid>("SessionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<TimeOnly>("StartTime")
                        .HasColumnType("time");

                    b.Property<byte>("UnavailableDate")
                        .HasColumnType("tinyint");

                    b.Property<DateOnly>("WorkDate")
                        .HasColumnType("date");

                    b.HasKey("ScheduleId")
                        .HasName("PK__MentorWo__9C8A5B490B6777AE");

                    b.HasIndex("SessionId");

                    b.HasIndex(new[] { "ScheduleId" }, "UQ__MentorWo__9C8A5B48345AF3A7")
                        .IsUnique();

                    b.ToTable("MentorWorkSchedules");
                });

            modelBuilder.Entity("MentorBooking.Repository.Entities.PointTransaction", b =>
                {
                    b.Property<Guid>("TransactionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CreateAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("Description")
                        .HasMaxLength(1)
                        .HasColumnType("nvarchar(1)");

                    b.Property<int?>("PointsChanged")
                        .HasColumnType("int");

                    b.Property<bool>("TransactionType")
                        .HasColumnType("bit");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("TransactionId")
                        .HasName("PK__PointTra__55433A6BC5192108");

                    b.HasIndex("UserId");

                    b.HasIndex(new[] { "TransactionId" }, "UQ__PointTra__55433A6A702A041C")
                        .IsUnique();

                    b.ToTable("PointTransactions");
                });

            modelBuilder.Entity("MentorBooking.Repository.Entities.ProjectGroup", b =>
                {
                    b.Property<int>("GroupId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("GroupId"));

                    b.Property<DateTime>("CreateAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<Guid>("CreateBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("GroupName")
                        .HasMaxLength(1)
                        .HasColumnType("nvarchar(1)");

                    b.Property<string>("Topic")
                        .HasMaxLength(1)
                        .HasColumnType("nvarchar(1)");

                    b.HasKey("GroupId")
                        .HasName("PK__ProjectG__149AF36A56A56A19");

                    b.HasIndex(new[] { "GroupId" }, "UQ__ProjectG__149AF36B441F56CF")
                        .IsUnique();

                    b.ToTable("ProjectGroups");
                });

            modelBuilder.Entity("MentorBooking.Repository.Entities.ProjectProgress", b =>
                {
                    b.Property<int>("ProgressId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProgressId"));

                    b.Property<string>("Description")
                        .HasMaxLength(1)
                        .HasColumnType("nvarchar(1)");

                    b.Property<int>("GroupId")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdateAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.HasKey("ProgressId")
                        .HasName("PK__ProjectP__BAE29CA59712E6FC");

                    b.HasIndex("GroupId");

                    b.HasIndex(new[] { "ProgressId" }, "UQ__ProjectP__BAE29CA42BD6879C")
                        .IsUnique();

                    b.ToTable("ProjectProgress", (string)null);
                });

            modelBuilder.Entity("MentorBooking.Repository.Entities.RoleClaims", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("RoleClaims", (string)null);
                });

            modelBuilder.Entity("MentorBooking.Repository.Entities.Roles", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("Roles", (string)null);
                });

            modelBuilder.Entity("MentorBooking.Repository.Entities.Skill", b =>
                {
                    b.Property<int>("SkillId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("SkillId")
                        .HasName("PK__Skills__B1C89E18A86A1E4B");

                    b.ToTable("Skills");
                });

            modelBuilder.Entity("MentorBooking.Repository.Entities.Student", b =>
                {
                    b.Property<Guid>("StudentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("StudentId")
                        .HasName("PK__Students__32C52B99EF2F4E93");

                    b.HasIndex(new[] { "UserId" }, "UQ__Students__1788CC4DF24ECFC0")
                        .IsUnique();

                    b.HasIndex(new[] { "StudentId" }, "UQ__Students__32C52B9871763433")
                        .IsUnique();

                    b.ToTable("Students");
                });

            modelBuilder.Entity("MentorBooking.Repository.Entities.StudentGroup", b =>
                {
                    b.Property<Guid>("StudentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("GroupId")
                        .HasColumnType("int");

                    b.Property<DateTime>("JoinAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.HasKey("StudentId", "GroupId")
                        .HasName("PK__StudentG__838C84AFD966A83D");

                    b.HasIndex("GroupId");

                    b.HasIndex(new[] { "StudentId" }, "UQ__StudentG__32C52B98BDCB51AE")
                        .IsUnique();

                    b.ToTable("StudentGroups");
                });

            modelBuilder.Entity("MentorBooking.Repository.Entities.StudentsPaymentSession", b =>
                {
                    b.Property<Guid>("SessionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("StudentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("PaidAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<int>("PointsChanged")
                        .HasColumnType("int");

                    b.HasKey("SessionId", "StudentId")
                        .HasName("PK__Students__4AD8C0293BC278AC");

                    b.HasIndex("StudentId");

                    b.HasIndex(new[] { "SessionId" }, "UQ__Students__C9F492911F34EC6C")
                        .IsUnique();

                    b.ToTable("StudentsPaymentSession", (string)null);
                });

            modelBuilder.Entity("MentorBooking.Repository.Entities.UserClaims", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserClaims", (string)null);
                });

            modelBuilder.Entity("MentorBooking.Repository.Entities.UserLogins", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("UserLogins", (string)null);
                });

            modelBuilder.Entity("MentorBooking.Repository.Entities.UserPoint", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("PointsBalance")
                        .HasColumnType("int");

                    b.HasKey("UserId")
                        .HasName("PK__UserPoin__1788CC4C298B88EC");

                    b.HasIndex(new[] { "UserId" }, "UQ__UserPoin__1788CC4D87499114")
                        .IsUnique();

                    b.ToTable("UserPoints");
                });

            modelBuilder.Entity("MentorBooking.Repository.Entities.UserRoles", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles", (string)null);
                });

            modelBuilder.Entity("MentorBooking.Repository.Entities.UserTokens", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("Expired")
                        .HasColumnType("datetime2");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("UserTokens", (string)null);
                });

            modelBuilder.Entity("MentorBooking.Repository.Entities.Users", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("MentorBooking.Repository.Entities.GroupFeedback", b =>
                {
                    b.HasOne("MentorBooking.Repository.Entities.ProjectGroup", "Group")
                        .WithMany("GroupFeedbacks")
                        .HasForeignKey("GroupId")
                        .IsRequired()
                        .HasConstraintName("FK__GroupFeed__Group__247D636F");

                    b.HasOne("MentorBooking.Repository.Entities.Mentor", "Mentor")
                        .WithMany("GroupFeedbacks")
                        .HasForeignKey("MentorId")
                        .IsRequired()
                        .HasConstraintName("FK__GroupFeed__Mento__257187A8");

                    b.HasOne("MentorBooking.Repository.Entities.MentorSupportSession", "Session")
                        .WithOne("GroupFeedback")
                        .HasForeignKey("MentorBooking.Repository.Entities.GroupFeedback", "SessionId")
                        .IsRequired()
                        .HasConstraintName("FK__GroupFeed__Sessi__2665ABE1");

                    b.Navigation("Group");

                    b.Navigation("Mentor");

                    b.Navigation("Session");
                });

            modelBuilder.Entity("MentorBooking.Repository.Entities.Mentor", b =>
                {
                    b.HasOne("MentorBooking.Repository.Entities.Users", "User")
                        .WithOne("Mentor")
                        .HasForeignKey("MentorBooking.Repository.Entities.Mentor", "UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("MentorBooking.Repository.Entities.MentorFeedback", b =>
                {
                    b.HasOne("MentorBooking.Repository.Entities.Mentor", "Mentor")
                        .WithMany("MentorFeedbacks")
                        .HasForeignKey("MentorId")
                        .IsRequired()
                        .HasConstraintName("FK__MentorFee__Mento__21A0F6C4");

                    b.HasOne("MentorBooking.Repository.Entities.MentorSupportSession", "Session")
                        .WithMany("MentorFeedbacks")
                        .HasForeignKey("SessionId")
                        .IsRequired()
                        .HasConstraintName("FK__MentorFee__Sessi__22951AFD");

                    b.HasOne("MentorBooking.Repository.Entities.Student", "Student")
                        .WithMany("MentorFeedbacks")
                        .HasForeignKey("StudentId")
                        .IsRequired()
                        .HasConstraintName("FK__MentorFee__Stude__23893F36");

                    b.Navigation("Mentor");

                    b.Navigation("Session");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("MentorBooking.Repository.Entities.MentorSkill", b =>
                {
                    b.HasOne("MentorBooking.Repository.Entities.Mentor", "Mentor")
                        .WithMany("MentorSkills")
                        .HasForeignKey("MentorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MentorBooking.Repository.Entities.Skill", "Skill")
                        .WithMany("MentorSkills")
                        .HasForeignKey("SkillId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Mentor");

                    b.Navigation("Skill");
                });

            modelBuilder.Entity("MentorBooking.Repository.Entities.MentorSupportSession", b =>
                {
                    b.HasOne("MentorBooking.Repository.Entities.ProjectGroup", "Group")
                        .WithMany("MentorSupportSessions")
                        .HasForeignKey("GroupId")
                        .IsRequired()
                        .HasConstraintName("FK__MentorSup__Group__1CDC41A7");

                    b.HasOne("MentorBooking.Repository.Entities.Mentor", "Mentor")
                        .WithMany("MentorSupportSessions")
                        .HasForeignKey("MentorId")
                        .IsRequired()
                        .HasConstraintName("FK__MentorSup__Mento__1DD065E0");

                    b.Navigation("Group");

                    b.Navigation("Mentor");
                });

            modelBuilder.Entity("MentorBooking.Repository.Entities.MentorWorkSchedule", b =>
                {
                    b.HasOne("MentorBooking.Repository.Entities.MentorSupportSession", "Session")
                        .WithMany("MentorWorkSchedules")
                        .HasForeignKey("SessionId")
                        .IsRequired()
                        .HasConstraintName("FK__MentorWor__Sessi__1EC48A19");

                    b.Navigation("Session");
                });

            modelBuilder.Entity("MentorBooking.Repository.Entities.PointTransaction", b =>
                {
                    b.HasOne("MentorBooking.Repository.Entities.UserPoint", "User")
                        .WithMany("PointTransactions")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK__PointTran__UserI__17236851");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MentorBooking.Repository.Entities.ProjectProgress", b =>
                {
                    b.HasOne("MentorBooking.Repository.Entities.ProjectGroup", "Group")
                        .WithMany("ProjectProgresses")
                        .HasForeignKey("GroupId")
                        .IsRequired()
                        .HasConstraintName("FK__ProjectPr__Group__19FFD4FC");

                    b.Navigation("Group");
                });

            modelBuilder.Entity("MentorBooking.Repository.Entities.RoleClaims", b =>
                {
                    b.HasOne("MentorBooking.Repository.Entities.Roles", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MentorBooking.Repository.Entities.Student", b =>
                {
                    b.HasOne("MentorBooking.Repository.Entities.Users", "User")
                        .WithOne("Student")
                        .HasForeignKey("MentorBooking.Repository.Entities.Student", "StudentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("MentorBooking.Repository.Entities.StudentGroup", b =>
                {
                    b.HasOne("MentorBooking.Repository.Entities.ProjectGroup", "Group")
                        .WithMany("StudentGroups")
                        .HasForeignKey("GroupId")
                        .IsRequired()
                        .HasConstraintName("FK__StudentGr__Group__190BB0C3");

                    b.HasOne("MentorBooking.Repository.Entities.Student", "Student")
                        .WithOne("StudentGroup")
                        .HasForeignKey("MentorBooking.Repository.Entities.StudentGroup", "StudentId")
                        .IsRequired()
                        .HasConstraintName("FK__StudentGr__Stude__18178C8A");

                    b.Navigation("Group");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("MentorBooking.Repository.Entities.StudentsPaymentSession", b =>
                {
                    b.HasOne("MentorBooking.Repository.Entities.MentorSupportSession", "Session")
                        .WithOne("StudentsPaymentSession")
                        .HasForeignKey("MentorBooking.Repository.Entities.StudentsPaymentSession", "SessionId")
                        .IsRequired()
                        .HasConstraintName("FK__StudentsP__Sessi__1FB8AE52");

                    b.HasOne("MentorBooking.Repository.Entities.Student", "Student")
                        .WithMany("StudentsPaymentSessions")
                        .HasForeignKey("StudentId")
                        .IsRequired()
                        .HasConstraintName("FK__StudentsP__Stude__20ACD28B");

                    b.Navigation("Session");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("MentorBooking.Repository.Entities.UserClaims", b =>
                {
                    b.HasOne("MentorBooking.Repository.Entities.Users", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MentorBooking.Repository.Entities.UserLogins", b =>
                {
                    b.HasOne("MentorBooking.Repository.Entities.Users", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MentorBooking.Repository.Entities.UserPoint", b =>
                {
                    b.HasOne("MentorBooking.Repository.Entities.Users", "User")
                        .WithOne("UserPoint")
                        .HasForeignKey("MentorBooking.Repository.Entities.UserPoint", "UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("MentorBooking.Repository.Entities.UserRoles", b =>
                {
                    b.HasOne("MentorBooking.Repository.Entities.Roles", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MentorBooking.Repository.Entities.Users", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MentorBooking.Repository.Entities.UserTokens", b =>
                {
                    b.HasOne("MentorBooking.Repository.Entities.Users", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MentorBooking.Repository.Entities.Mentor", b =>
                {
                    b.Navigation("GroupFeedbacks");

                    b.Navigation("MentorFeedbacks");

                    b.Navigation("MentorSkills");

                    b.Navigation("MentorSupportSessions");
                });

            modelBuilder.Entity("MentorBooking.Repository.Entities.MentorSupportSession", b =>
                {
                    b.Navigation("GroupFeedback");

                    b.Navigation("MentorFeedbacks");

                    b.Navigation("MentorWorkSchedules");

                    b.Navigation("StudentsPaymentSession");
                });

            modelBuilder.Entity("MentorBooking.Repository.Entities.ProjectGroup", b =>
                {
                    b.Navigation("GroupFeedbacks");

                    b.Navigation("MentorSupportSessions");

                    b.Navigation("ProjectProgresses");

                    b.Navigation("StudentGroups");
                });

            modelBuilder.Entity("MentorBooking.Repository.Entities.Skill", b =>
                {
                    b.Navigation("MentorSkills");
                });

            modelBuilder.Entity("MentorBooking.Repository.Entities.Student", b =>
                {
                    b.Navigation("MentorFeedbacks");

                    b.Navigation("StudentGroup");

                    b.Navigation("StudentsPaymentSessions");
                });

            modelBuilder.Entity("MentorBooking.Repository.Entities.UserPoint", b =>
                {
                    b.Navigation("PointTransactions");
                });

            modelBuilder.Entity("MentorBooking.Repository.Entities.Users", b =>
                {
                    b.Navigation("Mentor")
                        .IsRequired();

                    b.Navigation("Student")
                        .IsRequired();

                    b.Navigation("UserPoint")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
