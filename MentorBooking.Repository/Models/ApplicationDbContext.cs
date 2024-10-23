using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace MentorBooking.Repository.Models
{
    public partial class ApplicationDbContext : IdentityDbContext<
    ApplicationUser,          // Kiểu User tùy chỉnh (GUID làm khóa chính)
    ApplicationRole,           // Kiểu Role tùy chỉnh (int làm khóa chính)
    Guid,                      // Khóa chính của User là GUID
    IdentityUserClaim<Guid>,   // UserClaims ánh xạ theo khóa GUID
    IdentityUserRole<Guid>,     // UserRoles ánh xạ với Role dùng int
    IdentityUserLogin<Guid>,   // UserLogin ánh xạ với khóa GUID
    IdentityRoleClaim<Guid>,    // RoleClaim ánh xạ với khóa int
    IdentityUserToken<Guid>    // UserToken ánh xạ với khóa GUID
>
    {
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

        public virtual DbSet<Skill> Skills { get; set; }

        public virtual DbSet<Student> Students { get; set; }

        public virtual DbSet<StudentGroup> StudentGroups { get; set; }

        public virtual DbSet<StudentsPaymentSession> StudentsPaymentSessions { get; set; }

        public virtual DbSet<UserPoint> UserPoints { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ánh xạ bảng User với tên bảng tùy chỉnh nếu cần
            modelBuilder.Entity<ApplicationUser>()
                .ToTable("Users");

            // Ánh xạ bảng Role với tên bảng tùy chỉnh nếu cần
            modelBuilder.Entity<ApplicationRole>()
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

            modelBuilder.Entity<IdentityUserToken<Guid>>()
                .ToTable("UserTokens");
            modelBuilder.Entity<GroupFeedback>(entity =>
            {
                entity.HasKey(e => e.FeedbackId).HasName("PK__GroupFee__6A4BEDD632E261D8");

                entity.ToTable("GroupFeedback");

                entity.HasIndex(e => e.FeedbackId, "UQ__GroupFee__6A4BEDD718225B07").IsUnique();

                entity.HasIndex(e => e.SessionId, "UQ__GroupFee__C9F49291EC2DA840").IsUnique();

                entity.Property(e => e.FeedbackId).ValueGeneratedNever();
                entity.Property(e => e.Comment).HasMaxLength(1);
                entity.Property(e => e.CreateAt)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Group).WithMany(p => p.GroupFeedbacks)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__GroupFeed__Group__7FEAFD3E");

                entity.HasOne(d => d.Mentor).WithMany(p => p.GroupFeedbacks)
                    .HasForeignKey(d => d.MentorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__GroupFeed__Mento__00DF2177");

                entity.HasOne(d => d.Session).WithOne(p => p.GroupFeedback)
                    .HasForeignKey<GroupFeedback>(d => d.SessionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__GroupFeed__Sessi__01D345B0");
            });

            modelBuilder.Entity<Mentor>(entity =>
            {
                entity.HasKey(e => e.MentorId).HasName("PK__Mentors__053B7E9843339A2F");

                entity.HasIndex(e => e.MentorId, "UQ__Mentors__053B7E99380E4BF5").IsUnique();

                entity.HasIndex(e => e.UserId, "UQ__Mentors__1788CC4DCD90DE59").IsUnique();

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
                            .HasConstraintName("FK__MentorSki__Skill__7755B73D"),
                        l => l.HasOne<Mentor>().WithMany()
                            .HasForeignKey("MentorId")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("FK__MentorSki__Mento__76619304"),
                        j =>
                        {
                            j.HasKey("MentorId", "SkillId").HasName("PK__MentorSk__68C17780A4D1C4A1");
                            j.ToTable("MentorSkills");
                        });
            });

            modelBuilder.Entity<MentorFeedback>(entity =>
            {
                entity.HasKey(e => e.FeedbackId).HasName("PK__MentorFe__6A4BEDD6FFDF27E8");

                entity.ToTable("MentorFeedback");

                entity.HasIndex(e => e.FeedbackId, "UQ__MentorFe__6A4BEDD7EEDD57E4").IsUnique();

                entity.Property(e => e.FeedbackId).ValueGeneratedNever();
                entity.Property(e => e.Comment).HasMaxLength(1);
                entity.Property(e => e.CreateAt)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Mentor).WithMany(p => p.MentorFeedbacks)
                    .HasForeignKey(d => d.MentorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MentorFee__Mento__7D0E9093");

                entity.HasOne(d => d.Session).WithMany(p => p.MentorFeedbacks)
                    .HasForeignKey(d => d.SessionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MentorFee__Sessi__7E02B4CC");

                entity.HasOne(d => d.Student).WithMany(p => p.MentorFeedbacks)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MentorFee__Stude__7EF6D905");
            });

            modelBuilder.Entity<MentorSupportSession>(entity =>
            {
                entity.HasKey(e => e.SessionId).HasName("PK__MentorSu__C9F49290519072FB");

                entity.HasIndex(e => e.SessionId, "UQ__MentorSu__C9F492915AA054B2").IsUnique();

                entity.Property(e => e.SessionId).ValueGeneratedNever();

                entity.HasOne(d => d.Group).WithMany(p => p.MentorSupportSessions)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MentorSup__Group__7849DB76");

                entity.HasOne(d => d.Mentor).WithMany(p => p.MentorSupportSessions)
                    .HasForeignKey(d => d.MentorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MentorSup__Mento__793DFFAF");
            });

            modelBuilder.Entity<MentorWorkSchedule>(entity =>
            {
                entity.HasKey(e => e.ScheduleId).HasName("PK__MentorWo__9C8A5B492399C41F");

                entity.HasIndex(e => e.ScheduleId, "UQ__MentorWo__9C8A5B482E1FDBDE").IsUnique();

                entity.Property(e => e.ScheduleId).ValueGeneratedNever();

                entity.HasOne(d => d.Session).WithMany(p => p.MentorWorkSchedules)
                    .HasForeignKey(d => d.SessionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__MentorWor__Sessi__7A3223E8");
            });

            modelBuilder.Entity<PointTransaction>(entity =>
            {
                entity.HasKey(e => e.TransactionId).HasName("PK__PointTra__55433A6BA0518C5D");

                entity.HasIndex(e => e.TransactionId, "UQ__PointTra__55433A6A1AC580D8").IsUnique();

                entity.Property(e => e.TransactionId).ValueGeneratedNever();
                entity.Property(e => e.CreateAt)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.Description).HasMaxLength(1);

                entity.HasOne(d => d.User).WithMany(p => p.PointTransactions)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PointTran__UserI__70A8B9AE");
            });

            modelBuilder.Entity<ProjectGroup>(entity =>
            {
                entity.HasKey(e => e.GroupId).HasName("PK__ProjectG__149AF36A6F7109E5");

                entity.HasIndex(e => e.GroupId, "UQ__ProjectG__149AF36BA39FD5B0").IsUnique();

                entity.Property(e => e.CreateAt)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.GroupName).HasMaxLength(1);
                entity.Property(e => e.Topic).HasMaxLength(1);
            });

            modelBuilder.Entity<Skill>(entity =>
            {
                entity.HasKey(e => e.SkillId).HasName("PK__Skills__DFA09187CCE92901");

                entity.HasIndex(e => e.SkillId, "UQ__Skills__DFA09186BDE3ACF2").IsUnique();

                entity.Property(e => e.Name).HasMaxLength(1);
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.StudentId).HasName("PK__Students__32C52B99CFB61B90");

                entity.HasIndex(e => e.UserId, "UQ__Students__1788CC4DA0F925B6").IsUnique();

                entity.HasIndex(e => e.StudentId, "UQ__Students__32C52B9835FD26AF").IsUnique();

                entity.Property(e => e.StudentId).ValueGeneratedNever();
                entity.Property(e => e.CreateAt)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<StudentGroup>(entity =>
            {
                entity.HasKey(e => new { e.StudentId, e.GroupId }).HasName("PK__StudentG__838C84AFBF71794B");

                entity.HasIndex(e => e.StudentId, "UQ__StudentG__32C52B98B6ACEFB9").IsUnique();

                entity.Property(e => e.JoinAt)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Group).WithMany(p => p.StudentGroups)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__StudentGr__Group__73852659");

                entity.HasOne(d => d.Student).WithOne(p => p.StudentGroup)
                    .HasForeignKey<StudentGroup>(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__StudentGr__Stude__72910220");
            });

            modelBuilder.Entity<StudentsPaymentSession>(entity =>
            {
                entity.HasKey(e => new { e.SessionId, e.StudentId }).HasName("PK__Students__4AD8C029A16F13A9");

                entity.ToTable("StudentsPaymentSession");

                entity.HasIndex(e => e.SessionId, "UQ__Students__C9F492919764073E").IsUnique();

                entity.Property(e => e.PaidAt)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Session).WithOne(p => p.StudentsPaymentSession)
                    .HasForeignKey<StudentsPaymentSession>(d => d.SessionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__StudentsP__Sessi__7B264821");

                entity.HasOne(d => d.Student).WithMany(p => p.StudentsPaymentSessions)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__StudentsP__Stude__7C1A6C5A");
            });

            modelBuilder.Entity<UserPoint>(entity =>
            {
                entity.HasKey(e => e.UserId).HasName("PK__UserPoin__1788CC4C8920074C");

                entity.HasIndex(e => e.UserId, "UQ__UserPoin__1788CC4D6F73A679").IsUnique();

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

            // Thiết lập quan hệ 1-1 giữa ApplicationUser và UserPoint
            modelBuilder.Entity<UserPoint>()
                .HasOne(up => up.User)
                .WithOne(u => u.UserPoint)
                .HasForeignKey<UserPoint>(up => up.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }

}