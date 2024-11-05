using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MentorBooking.Repository.Entities;
namespace MentorBooking.Repository.Data;

public partial class ApplicationDbContext : IdentityDbContext<
    Users,          // Kiểu User tùy chỉnh (GUID làm khóa chính)
    Roles,           // Kiểu Role tùy chỉnh (int làm khóa chính)
    Guid,                      // Khóa chính của User là GUID
    UserClaims,   // UserClaims ánh xạ theo khóa GUID
    UserRoles,     // UserRoles ánh xạ với Role dùng int
    UserLogins,   // UserLogin ánh xạ với khóa GUID
    RoleClaims,    // RoleClaim ánh xạ với khóa int
    UserTokens    // UserToken ánh xạ với khóa GUID
>
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    // public virtual DbSet<SchedulesAvailable> SchedulesAvailables { get; set; }
    public virtual DbSet<GroupFeedback> GroupFeedbacks { get; set; }

    public virtual DbSet<Mentor> Mentors { get; set; }

    public virtual DbSet<MentorFeedback> MentorFeedbacks { get; set; }

    public virtual DbSet<MentorSupportSession> MentorSupportSessions { get; set; }

    public virtual DbSet<MentorWorkSchedule> MentorWorkSchedules { get; set; }

    public virtual DbSet<PointTransaction> PointTransactions { get; set; }

    public virtual DbSet<ProjectGroup> ProjectGroups { get; set; }

    public virtual DbSet<ProjectProgress> ProjectProgresses { get; set; }

    public virtual DbSet<Skill> Skills { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<StudentGroup> StudentGroups { get; set; }

    public virtual DbSet<StudentsPaymentSession> StudentsPaymentSessions { get; set; }

    public virtual DbSet<UserPoint> UserPoints { get; set; }
    public virtual DbSet<MentorSkill> MentorSkills { get; set; }
    public virtual DbSet<SchedulesAvailable> SchedulesAvailable { get; set; }
    public virtual DbSet<Users> User { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Ánh xạ bảng User với tên bảng tùy chỉnh nếu cần
        modelBuilder.Entity<Users>()
            .ToTable("Users");

        // Ánh xạ bảng Role với tên bảng tùy chỉnh nếu cần
        modelBuilder.Entity<Roles>()
            .ToTable("Roles");

        // Ánh xạ các bảng khác nếu cần
        modelBuilder.Entity<UserClaims>()
            .ToTable("UserClaims");

        modelBuilder.Entity<UserRoles>()
            .ToTable("UserRoles");

        modelBuilder.Entity<UserLogins>()
            .ToTable("UserLogins");

        modelBuilder.Entity<RoleClaims>()
            .ToTable("RoleClaims");

        modelBuilder.Entity<UserTokens>()
            .ToTable("UserTokens");
        modelBuilder.Entity<GroupFeedback>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("PK__GroupFee__6A4BEDD6E709A159");

            entity.ToTable("GroupFeedback");

            entity.HasIndex(e => e.FeedbackId, "UQ__GroupFee__6A4BEDD7BC495357").IsUnique();

            entity.HasIndex(e => e.SessionId, "UQ__GroupFee__C9F492913EE01081").IsUnique();

            entity.Property(e => e.FeedbackId).ValueGeneratedNever();
            entity.Property(e => e.Comment).HasMaxLength(1000);
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Group).WithMany(p => p.GroupFeedbacks)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__GroupFeed__Group__247D636F");

            entity.HasOne(d => d.Mentor).WithMany(p => p.GroupFeedbacks)
                .HasForeignKey(d => d.MentorId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__GroupFeed__Mento__257187A8");

            entity.HasOne(d => d.Session).WithOne(p => p.GroupFeedback)
                .HasForeignKey<GroupFeedback>(d => d.SessionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GroupFeed__Sessi__2665ABE1");
        });

        

        modelBuilder.Entity<MentorFeedback>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("PK__MentorFe__6A4BEDD6B818C861");

            entity.ToTable("MentorFeedback");

            entity.HasIndex(e => e.FeedbackId, "UQ__MentorFe__6A4BEDD72A8F378A").IsUnique();

            entity.Property(e => e.FeedbackId).ValueGeneratedNever();
            entity.Property(e => e.Comment).HasMaxLength(1000);
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Mentor).WithMany(p => p.MentorFeedbacks)
                .HasForeignKey(d => d.MentorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MentorFee__Mento__21A0F6C4");

            entity.HasOne(d => d.Session).WithMany(p => p.MentorFeedbacks)
                .HasForeignKey(d => d.SessionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MentorFee__Sessi__22951AFD");

            entity.HasOne(d => d.Student).WithMany(p => p.MentorFeedbacks)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MentorFee__Stude__23893F36");
        });

        modelBuilder.Entity<MentorSupportSession>(entity =>
        {
            entity.HasKey(e => e.SessionId).HasName("PK__MentorSu__C9F49290299FC032");

            entity.HasIndex(e => e.SessionId, "UQ__MentorSu__C9F4929183F9C51A").IsUnique();

            entity.Property(e => e.SessionId).ValueGeneratedNever();

            entity.HasOne(d => d.Group).WithMany(p => p.MentorSupportSessions)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__MentorSup__Group__1CDC41A7");

            entity.HasOne(d => d.Mentor).WithMany(p => p.MentorSupportSessions)
                .HasForeignKey(d => d.MentorId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__MentorSup__Mento__1DD065E0");
        });
        modelBuilder.Entity<SchedulesAvailable>(entity =>
        {
            entity.HasKey(e => e.ScheduleAvailableId);
        
            entity.Property(e => e.FreeDay)
                .HasColumnType("date");
        
            entity.Property(e => e.StartTime)
                .HasColumnType("time");
        
            entity.Property(e => e.EndTime)
                .HasColumnType("time");

        });
        modelBuilder.Entity<MentorWorkSchedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("PK__MentorWo__9C8A5B490B6777AE");

            entity.HasIndex(e => e.ScheduleId, "UQ__MentorWo__9C8A5B48345AF3A7").IsUnique();

            entity.Property(e => e.ScheduleId).ValueGeneratedNever();

            entity.HasOne(d => d.Session).WithMany(p => p.MentorWorkSchedules)
                .HasForeignKey(d => d.SessionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MentorWor__Sessi__1EC48A19");
        });

        modelBuilder.Entity<PointTransaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__PointTra__55433A6BC5192108");

            entity.HasIndex(e => e.TransactionId, "UQ__PointTra__55433A6A702A041C").IsUnique();

            entity.Property(e => e.TransactionId).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(1000);

            entity.HasOne(d => d.UserPoint).WithMany(p => p.PointTransactions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PointTran__UserI__17236851");
        });

        modelBuilder.Entity<ProjectGroup>(entity =>
        {
            entity.HasKey(e => e.GroupId).HasName("PK__ProjectG__149AF36A56A56A19");

            entity.HasIndex(e => e.GroupId, "UQ__ProjectG__149AF36B441F56CF").IsUnique();
            entity.Property(e => e.GroupId).HasColumnType("uniqueidentifier");
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.GroupName).HasMaxLength(100);
            entity.Property(e => e.Topic).HasMaxLength(100);
        });

        modelBuilder.Entity<ProjectProgress>(entity =>
        {
            entity.HasKey(e => e.ProgressId).HasName("PK__ProjectP__BAE29CA59712E6FC");

            entity.ToTable("ProjectProgress");

            entity.HasIndex(e => e.ProgressId, "UQ__ProjectP__BAE29CA42BD6879C").IsUnique();

            entity.Property(e => e.Description).HasMaxLength(10000);
            entity.Property(e => e.UpdateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.MentorSupportSession).WithMany(p => p.ProjectProgresses)
                .HasForeignKey(d => d.SessionId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__ProjectPr__Group__19FFD4FC");
        });


        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK__Students__32C52B99EF2F4E93");

            entity.HasIndex(e => e.UserId, "UQ__Students__1788CC4DF24ECFC0").IsUnique();

            entity.HasIndex(e => e.StudentId, "UQ__Students__32C52B9871763433").IsUnique();

            entity.Property(e => e.StudentId).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<StudentGroup>(entity =>
        {
            entity.HasKey(e => new { e.StudentId, e.GroupId }).HasName("PK__StudentG__838C84AFD966A83D");


            entity.Property(e => e.JoinAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Group).WithMany(p => p.StudentGroups)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__StudentGr__Group__190BB0C3");

            entity.HasOne(d => d.Student).WithOne(p => p.StudentGroup)
                .HasForeignKey<StudentGroup>(d => d.StudentId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__StudentGr__Stude__18178C8A");
        });

        modelBuilder.Entity<StudentsPaymentSession>(entity =>
        {
            entity.HasKey(e => new { e.SessionId, e.StudentId }).HasName("PK__Students__4AD8C0293BC278AC");

            entity.ToTable("StudentsPaymentSession");

            entity.HasIndex(e => e.SessionId, "UQ__Students__C9F492911F34EC6C").IsUnique();

            entity.Property(e => e.PaidAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Session).WithOne(p => p.StudentsPaymentSession)
                .HasForeignKey<StudentsPaymentSession>(d => d.SessionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StudentsP__Sessi__1FB8AE52");

            entity.HasOne(d => d.Student).WithMany(p => p.StudentsPaymentSessions)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StudentsP__Stude__20ACD28B");
        });

        modelBuilder.Entity<UserPoint>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__UserPoin__1788CC4C298B88EC");

            entity.HasIndex(e => e.UserId, "UQ__UserPoin__1788CC4D87499114").IsUnique();

        });
        modelBuilder.Entity<Mentor>()
                .HasOne(m => m.User)
                .WithOne(m => m.Mentor)
                .HasForeignKey<Mentor>(m => m.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Student>()
            .HasOne(s => s.User)
            .WithOne(u => u.Student)
            .HasForeignKey<Student>(s => s.StudentId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<SchedulesAvailable>()
            .HasOne(m => m.Mentor)
            .WithMany(m => m.SchedulesAvailable)
            .HasForeignKey(f => f.MentorId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_SchedulesAvailable_Mentor");
        modelBuilder.Entity<MentorWorkSchedule>()
            .HasOne(m => m.ScheduleAvailable)
            .WithOne(m => m.MentorWorkSchedule)
            .HasForeignKey<MentorWorkSchedule>(f => f.ScheduleAvailableId)
            .HasConstraintName("FK_SchedulesAvailable_MentorWorkSchedule");
        // Thiết lập quan hệ 1-1 giữa Users và UserPoint
        modelBuilder.Entity<UserPoint>()
            .HasOne(up => up.User)
            .WithOne(u => u.UserPoint)
            .HasForeignKey<UserPoint>(up => up.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Mentor>(entity =>
        {
            entity.HasKey(e => e.MentorId).HasName("PK__Mentors__053B7E98A1D585C9");

            entity.HasIndex(e => e.MentorId, "UQ__Mentors__053B7E99EBF8FADB").IsUnique();
            entity.HasIndex(e => e.UserId, "UQ__Mentors__1788CC4D5F137BEE").IsUnique();

            entity.Property(e => e.MentorId).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.MentorDescription).HasMaxLength(1000);

            entity.HasMany(m => m.MentorSkills)
                .WithOne(ms => ms.Mentor)
                .HasForeignKey(ms => ms.MentorId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Skill>(entity =>
        {
            entity.HasKey(e => e.SkillId).HasName("PK__Skills__B1C89E18A86A1E4B");

            entity.Property(e => e.SkillId).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);

            entity.HasMany(s => s.MentorSkills)
                .WithOne(ms => ms.Skill)
                .HasForeignKey(ms => ms.SkillId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<MentorSkill>(entity =>
        {
            entity.HasKey(ms => new { ms.MentorId, ms.SkillId });

            entity.HasOne(ms => ms.Mentor)
                .WithMany(m => m.MentorSkills)
                .HasForeignKey(ms => ms.MentorId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(ms => ms.Skill)
                .WithMany(s => s.MentorSkills)
                .HasForeignKey(ms => ms.SkillId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
