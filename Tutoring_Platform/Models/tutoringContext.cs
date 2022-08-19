using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Tutoring_Platform.Models
{
    public partial class tutoringContext : DbContext
    {
        public tutoringContext()
        {
        }

        public tutoringContext(DbContextOptions<tutoringContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AdminReply> AdminReplies { get; set; } = null!;
        public virtual DbSet<AppointConfirm> AppointConfirms { get; set; } = null!;
        public virtual DbSet<AppointRequest> AppointRequests { get; set; } = null!;
        public virtual DbSet<AppointSlot> AppointSlots { get; set; } = null!;
        public virtual DbSet<DaysAvailable> DaysAvailables { get; set; } = null!;
        public virtual DbSet<HelpQuery> HelpQueries { get; set; } = null!;
        public virtual DbSet<ReportAccount> ReportAccounts { get; set; } = null!;
        public virtual DbSet<Statistic> Statistics { get; set; } = null!;
        public virtual DbSet<StudTutorInfo> StudTutorInfos { get; set; } = null!;
        public virtual DbSet<TutorCourse> TutorCourses { get; set; } = null!;
        public virtual DbSet<TutorInfo> TutorInfos { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=(localdb)\\ProjectModels;Database=tutoring;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdminReply>(entity =>
            {
                entity.ToTable("admin_replies");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.AdminId).HasColumnName("admin_id");

                entity.Property(e => e.Message)
                    .IsUnicode(false)
                    .HasColumnName("message");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Admin)
                    .WithMany(p => p.AdminReplies)
                    .HasForeignKey(d => d.AdminId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_admin_replies_user");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AdminReplies)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_admin_replies_stud_tutor_info");
            });

            modelBuilder.Entity<AppointConfirm>(entity =>
            {
                entity.ToTable("appoint_confirm");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.MeetingLink)
                    .IsUnicode(false)
                    .HasColumnName("meeting_link");

                entity.Property(e => e.PaypalLink)
                    .IsUnicode(false)
                    .HasColumnName("paypal_link");

                entity.Property(e => e.SlotId).HasColumnName("slot_id");

                entity.HasOne(d => d.Slot)
                    .WithMany(p => p.AppointConfirms)
                    .HasForeignKey(d => d.SlotId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_appoint_confirm_appoint_slots");
            });

            modelBuilder.Entity<AppointRequest>(entity =>
            {
                entity.ToTable("appoint_requests");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Course)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("course");

                entity.Property(e => e.StudId).HasColumnName("stud_id");

                entity.Property(e => e.TutorId).HasColumnName("tutor_id");

                entity.HasOne(d => d.Stud)
                    .WithMany(p => p.AppointRequests)
                    .HasForeignKey(d => d.StudId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_appoint_requests_stud_tutor_info");

                entity.HasOne(d => d.Tutor)
                    .WithMany(p => p.AppointRequests)
                    .HasForeignKey(d => d.TutorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_appoint_requests_tutor_info");
            });

            modelBuilder.Entity<AppointSlot>(entity =>
            {
                entity.ToTable("appoint_slots");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.RequestId).HasColumnName("request_id");

                entity.Property(e => e.Selected).HasColumnName("selected");

                entity.Property(e => e.Slot)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("slot");

                entity.HasOne(d => d.Request)
                    .WithMany(p => p.AppointSlots)
                    .HasForeignKey(d => d.RequestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_appoint_slots_appoint_requests");
            });

            modelBuilder.Entity<DaysAvailable>(entity =>
            {
                entity.ToTable("days_available");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Friday).HasColumnName("friday");

                entity.Property(e => e.Monday).HasColumnName("monday");

                entity.Property(e => e.Saturday).HasColumnName("saturday");

                entity.Property(e => e.Sunday).HasColumnName("sunday");

                entity.Property(e => e.Thursday).HasColumnName("thursday");

                entity.Property(e => e.Tuesday).HasColumnName("tuesday");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.Wednesday).HasColumnName("wednesday");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.DaysAvailables)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_days_available_tutor_info");
            });

            modelBuilder.Entity<HelpQuery>(entity =>
            {
                entity.ToTable("help_queries");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Query)
                    .IsUnicode(false)
                    .HasColumnName("query");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.HelpQueries)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_help_queries_stud_tutor_info");
            });

            modelBuilder.Entity<ReportAccount>(entity =>
            {
                entity.ToTable("report_account");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.AccountId).HasColumnName("account_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ReportAccounts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_report_account_stud_tutor_info");
            });

            modelBuilder.Entity<Statistic>(entity =>
            {
                entity.ToTable("statistics");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.AdminId).HasColumnName("admin_id");

                entity.Property(e => e.Data)
                    .IsUnicode(false)
                    .HasColumnName("data");

                entity.Property(e => e.Name)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.HasOne(d => d.Admin)
                    .WithMany(p => p.Statistics)
                    .HasForeignKey(d => d.AdminId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_statistics_user");
            });

            modelBuilder.Entity<StudTutorInfo>(entity =>
            {
                entity.ToTable("stud_tutor_info");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Address)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("address");

                entity.Property(e => e.City)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("city");

                entity.Property(e => e.PostalCode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("postalCode");

                entity.Property(e => e.Program)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("program");

                entity.Property(e => e.Province)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("province");

                entity.Property(e => e.Role)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("role");

                entity.Property(e => e.School)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("school");

                entity.Property(e => e.Semester).HasColumnName("semester");

                entity.Property(e => e.StudyField)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("study_field");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.StudTutorInfos)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_stud_tutor_info_user");
            });

            modelBuilder.Entity<TutorCourse>(entity =>
            {
                entity.ToTable("tutor_courses");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Course)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("course");

                entity.Property(e => e.TutorId).HasColumnName("tutor_id");

                entity.HasOne(d => d.Tutor)
                    .WithMany(p => p.TutorCourses)
                    .HasForeignKey(d => d.TutorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tutor_courses_tutor_info");
            });

            modelBuilder.Entity<TutorInfo>(entity =>
            {
                entity.ToTable("tutor_info");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("status");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.Wage).HasColumnName("wage");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TutorInfos)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tutor_info_stud_tutor_info");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("first_name");

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("last_name");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.Role)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("role");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
