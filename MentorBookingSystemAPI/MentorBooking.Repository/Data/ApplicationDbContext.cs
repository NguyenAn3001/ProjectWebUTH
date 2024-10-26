using System;
using System.Collections.Generic;
using MentorBooking.Repository.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MentorBooking.Repository.Data;

public partial class ApplicationDbContext : IdentityDbContext<
    Users,          // Kiểu User tùy chỉnh (GUID làm khóa chính)
    Roles,           // Kiểu Role tùy chỉnh (int làm khóa chính)
    Guid,                      // Khóa chính của User là GUID
    IdentityUserClaim<Guid>,   // UserClaims ánh xạ theo khóa GUID
    IdentityUserRole<Guid>,     // UserRoles ánh xạ với Role dùng int
    IdentityUserLogin<Guid>,   // UserLogin ánh xạ với khóa GUID
    IdentityRoleClaim<Guid>,    // RoleClaim ánh xạ với khóa int
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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=TINHHT7518\\SQLEXPRESS;Initial Catalog=MentorBookingSystemDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");

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
        modelBuilder.Entity<IdentityUserClaim<Guid>>()
            .ToTable("UserClaims");

        modelBuilder.Entity<IdentityUserRole<Guid>>()
            .ToTable("UserRoles");

        modelBuilder.Entity<IdentityUserLogin<Guid>>()
            .ToTable("UserLogins");

        modelBuilder.Entity<IdentityRoleClaim<Guid>>()
            .ToTable("RoleClaims");

        modelBuilder.Entity<UserTokens>()
            .ToTable("UserTokens");
        modelBuilder.Entity<GroupFeedback>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("PK__GroupFee__6A4BEDD664AAEBB9");

            entity.ToTable("GroupFeedback");

            entity.HasIndex(e => e.FeedbackId, "UQ__GroupFee__6A4BEDD7FB6AF5B9").IsUnique();

            entity.HasIndex(e => e.SessionId, "UQ__GroupFee__C9F4929142F64988").IsUnique();

            entity.Property(e => e.FeedbackId).ValueGeneratedNever();
            entity.Property(e => e.Comment).HasMaxLength(1);
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Group).WithMany(p => p.GroupFeedbacks)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GroupFeed__Group__2882FE7D");

            entity.HasOne(d => d.Mentor).WithMany(p => p.GroupFeedbacks)
                .HasForeignKey(d => d.MentorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GroupFeed__Mento__297722B6");

            entity.HasOne(d => d.Session).WithOne(p => p.GroupFeedback)
                .HasForeignKey<GroupFeedback>(d => d.SessionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GroupFeed__Sessi__2A6B46EF");
        });

        modelBuilder.Entity<Mentor>(entity =>
        {
            entity.HasKey(e => e.MentorId).HasName("PK__Mentors__053B7E987CAF4940");

            entity.HasIndex(e => e.MentorId, "UQ__Mentors__053B7E997F2CAB59").IsUnique();

            entity.HasIndex(e => e.UserId, "UQ__Mentors__1788CC4D929B3EF3").IsUnique();

            entity.Property(e => e.MentorId).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.MentorDescription).HasMaxLength(1);

            entity.HasMany(d => d.Skills).WithMany(p => p.Mentors)
                .UsingEntity<Dictionary<string, object>>(
                    "MentorSkill",
                    r => r.HasOne<Skill>().WithMany()
                        .HasForeignKey("SkillId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__MentorSki__Skill__1FEDB87C"),
                    l => l.HasOne<Mentor>().WithMany()
                        .HasForeignKey("MentorId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__MentorSki__Mento__1EF99443"),
                    j =>
                    {
                        j.HasKey("MentorId", "SkillId").HasName("PK__MentorSk__68C17780A33E19EF");
                        j.ToTable("MentorSkills");
                    });
        });

        modelBuilder.Entity<MentorFeedback>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("PK__MentorFe__6A4BEDD6D5A2F3B9");

            entity.ToTable("MentorFeedback");

            entity.HasIndex(e => e.FeedbackId, "UQ__MentorFe__6A4BEDD73F28FDD6").IsUnique();

            entity.Property(e => e.FeedbackId).ValueGeneratedNever();
            entity.Property(e => e.Comment).HasMaxLength(1);
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Mentor).WithMany(p => p.MentorFeedbacks)
                .HasForeignKey(d => d.MentorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MentorFee__Mento__25A691D2");

            entity.HasOne(d => d.Session).WithMany(p => p.MentorFeedbacks)
                .HasForeignKey(d => d.SessionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MentorFee__Sessi__269AB60B");

            entity.HasOne(d => d.Student).WithMany(p => p.MentorFeedbacks)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MentorFee__Stude__278EDA44");
        });

        modelBuilder.Entity<MentorSupportSession>(entity =>
        {
            entity.HasKey(e => e.SessionId).HasName("PK__MentorSu__C9F49290D96D731E");

            entity.HasIndex(e => e.SessionId, "UQ__MentorSu__C9F49291CA69CDB4").IsUnique();

            entity.Property(e => e.SessionId).ValueGeneratedNever();

            entity.HasOne(d => d.Group).WithMany(p => p.MentorSupportSessions)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MentorSup__Group__20E1DCB5");

            entity.HasOne(d => d.Mentor).WithMany(p => p.MentorSupportSessions)
                .HasForeignKey(d => d.MentorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MentorSup__Mento__21D600EE");
        });

        modelBuilder.Entity<MentorWorkSchedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("PK__MentorWo__9C8A5B495AABC993");

            entity.HasIndex(e => e.ScheduleId, "UQ__MentorWo__9C8A5B488B70D39C").IsUnique();

            entity.Property(e => e.ScheduleId).ValueGeneratedNever();

            entity.HasOne(d => d.Session).WithMany(p => p.MentorWorkSchedules)
                .HasForeignKey(d => d.SessionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MentorWor__Sessi__22CA2527");
        });

        modelBuilder.Entity<PointTransaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__PointTra__55433A6B1712B22E");

            entity.HasIndex(e => e.TransactionId, "UQ__PointTra__55433A6A38F963BD").IsUnique();

            entity.Property(e => e.TransactionId).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(1);

            entity.HasOne(d => d.User).WithMany(p => p.PointTransactions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PointTran__UserI__1B29035F");
        });

        modelBuilder.Entity<ProjectGroup>(entity =>
        {
            entity.HasKey(e => e.GroupId).HasName("PK__ProjectG__149AF36A8B4135B9");

            entity.HasIndex(e => e.GroupId, "UQ__ProjectG__149AF36B87B3F194").IsUnique();

            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.GroupName).HasMaxLength(1);
            entity.Property(e => e.Topic).HasMaxLength(1);
        });

        modelBuilder.Entity<ProjectProgress>(entity =>
        {
            entity.HasKey(e => e.ProgressId).HasName("PK__ProjectP__BAE29CA572CF069E");

            entity.ToTable("ProjectProgress");

            entity.HasIndex(e => e.ProgressId, "UQ__ProjectP__BAE29CA4BF18DE68").IsUnique();

            entity.Property(e => e.Description).HasMaxLength(1);
            entity.Property(e => e.UpdateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Group).WithMany(p => p.ProjectProgresses)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProjectPr__Group__1E05700A");
        });

        modelBuilder.Entity<Skill>(entity =>
        {
            entity.HasKey(e => e.SkillId).HasName("PK__Skills__DFA091877410A8CA");

            entity.HasIndex(e => e.SkillId, "UQ__Skills__DFA09186C47BA9C0").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(1);
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK__Students__32C52B99C300CB67");

            entity.HasIndex(e => e.UserId, "UQ__Students__1788CC4DCBC7FA15").IsUnique();

            entity.HasIndex(e => e.StudentId, "UQ__Students__32C52B9834F7FDAF").IsUnique();

            entity.Property(e => e.StudentId).ValueGeneratedNever();
            entity.Property(e => e.CreateAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<StudentGroup>(entity =>
        {
            entity.HasKey(e => new { e.StudentId, e.GroupId }).HasName("PK__StudentG__838C84AF1467181E");

            entity.HasIndex(e => e.StudentId, "UQ__StudentG__32C52B98C2791AFB").IsUnique();

            entity.Property(e => e.JoinAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Group).WithMany(p => p.StudentGroups)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StudentGr__Group__1D114BD1");

            entity.HasOne(d => d.Student).WithOne(p => p.StudentGroup)
                .HasForeignKey<StudentGroup>(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StudentGr__Stude__1C1D2798");
        });

        modelBuilder.Entity<StudentsPaymentSession>(entity =>
        {
            entity.HasKey(e => new { e.SessionId, e.StudentId }).HasName("PK__Students__4AD8C02925381DBA");

            entity.ToTable("StudentsPaymentSession");

            entity.HasIndex(e => e.SessionId, "UQ__Students__C9F49291A5E5E8B8").IsUnique();

            entity.Property(e => e.PaidAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Session).WithOne(p => p.StudentsPaymentSession)
                .HasForeignKey<StudentsPaymentSession>(d => d.SessionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StudentsP__Sessi__23BE4960");

            entity.HasOne(d => d.Student).WithMany(p => p.StudentsPaymentSessions)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StudentsP__Stude__24B26D99");
        });

        modelBuilder.Entity<UserPoint>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__UserPoin__1788CC4CA819B7DC");

            entity.HasIndex(e => e.UserId, "UQ__UserPoin__1788CC4D6BA81E59").IsUnique();

            entity.Property(e => e.UserId).ValueGeneratedNever();
        });
        modelBuilder.Entity<Mentor>()
                .HasOne(m => m.User)
                .WithOne(m => m.Mentor)
                .HasForeignKey<Mentor>(m => m.MentorId)
                .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Student>()
            .HasOne(s => s.User)
            .WithOne(u => u.Student)
            .HasForeignKey<Student>(s => s.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        // Thiết lập quan hệ 1-1 giữa Users và UserPoint
        modelBuilder.Entity<UserPoint>()
            .HasOne(up => up.User)
            .WithOne(u => u.UserPoint)
            .HasForeignKey<UserPoint>(up => up.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        OnModelCreatingPartial(modelBuilder);
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
