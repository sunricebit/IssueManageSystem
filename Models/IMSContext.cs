using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace IMS.Models
{
    public partial class IMSContext : DbContext
    {
        public IMSContext()
        {
        }

        public IMSContext(DbContextOptions<IMSContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Assignment> Assignments { get; set; } = null!;
        public virtual DbSet<Class> Classes { get; set; } = null!;
        public virtual DbSet<Contact> Contacts { get; set; } = null!;
        public virtual DbSet<Issue> Issues { get; set; } = null!;
        public virtual DbSet<Issuesetting> Issuesettings { get; set; } = null!;
        public virtual DbSet<Message> Messages { get; set; } = null!;
        public virtual DbSet<Milestone> Milestones { get; set; } = null!;
        public virtual DbSet<Permission> Permissions { get; set; } = null!;
        public virtual DbSet<Post> Posts { get; set; } = null!;
        public virtual DbSet<Project> Projects { get; set; } = null!;
        public virtual DbSet<Setting> Settings { get; set; } = null!;
        public virtual DbSet<Subject> Subjects { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySQL("server=localhost;uid=root;pwd=123456789;database=IMS");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Assignment>(entity =>
            {
                entity.ToTable("assignment");

                entity.HasIndex(e => e.Id, "Assignment_Id_idx");

                entity.HasIndex(e => e.SubjectId, "Assignment_SubjectId_fkey");

                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.Assignments)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("Assignment_SubjectId_fkey");
            });

            modelBuilder.Entity<Class>(entity =>
            {
                entity.ToTable("class");

                entity.HasIndex(e => e.Id, "Class_Id_idx");

                entity.HasIndex(e => e.SubjectId, "Class_SubjectId_fkey");

                entity.HasIndex(e => e.TeacherId, "Class_TeacherId_fkey");

                entity.Property(e => e.Description).HasMaxLength(300);

                entity.Property(e => e.Name).HasMaxLength(15);

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.Classes)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("Class_SubjectId_fkey");

                entity.HasOne(d => d.Teacher)
                    .WithMany(p => p.Classes)
                    .HasForeignKey(d => d.TeacherId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("Class_TeacherId_fkey");

                entity.HasMany(d => d.Students)
                    .WithMany(p => p.ClassesNavigation)
                    .UsingEntity<Dictionary<string, object>>(
                        "Classstudent",
                        l => l.HasOne<User>().WithMany().HasForeignKey("StudentId").OnDelete(DeleteBehavior.Restrict).HasConstraintName("ClassStudent_StudentId_fkey"),
                        r => r.HasOne<Class>().WithMany().HasForeignKey("ClassId").OnDelete(DeleteBehavior.Restrict).HasConstraintName("ClassStudent_ClassId_fkey"),
                        j =>
                        {
                            j.HasKey("ClassId", "StudentId").HasName("PRIMARY");

                            j.ToTable("classstudent");

                            j.HasIndex(new[] { "ClassId", "StudentId" }, "ClassStudent_ClassId_StudentId_idx");

                            j.HasIndex(new[] { "StudentId" }, "ClassStudent_StudentId_fkey");
                        });
            });

            modelBuilder.Entity<Contact>(entity =>
            {
                entity.ToTable("contact");

                entity.HasIndex(e => e.ContactTypeId, "Contact_ContactTypeId_fkey");

                entity.HasIndex(e => e.Id, "Contact_Id_idx");

                entity.HasIndex(e => e.CarerId, "Contact_UserId_fkey");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime(3)")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP(3)'");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.IsValid)
                    .IsRequired()
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Phone).HasMaxLength(15);

                entity.HasOne(d => d.Carer)
                    .WithMany(p => p.Contacts)
                    .HasForeignKey(d => d.CarerId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("Contact_CarerId_fkey");

                entity.HasOne(d => d.ContactType)
                    .WithMany(p => p.Contacts)
                    .HasForeignKey(d => d.ContactTypeId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("Contact_ContactTypeId_fkey");
            });

            modelBuilder.Entity<Issue>(entity =>
            {
                entity.ToTable("issue");

                entity.HasIndex(e => e.AssigneeId, "Issue_AssigneeId_fkey");

                entity.HasIndex(e => e.AuthorId, "Issue_AuthorId_fkey");

                entity.HasIndex(e => e.Id, "Issue_Id_idx");

                entity.HasIndex(e => e.MilestoneId, "Issue_MilestoneId_fkey");

                entity.HasIndex(e => e.ProjectId, "Issue_ProjectId_fkey");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.Title).HasColumnType("text");

                entity.HasOne(d => d.Assignee)
                    .WithMany(p => p.IssueAssignees)
                    .HasForeignKey(d => d.AssigneeId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("Issue_AssigneeId_fkey");

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.IssueAuthors)
                    .HasForeignKey(d => d.AuthorId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("Issue_AuthorId_fkey");

                entity.HasOne(d => d.Milestone)
                    .WithMany(p => p.Issues)
                    .HasForeignKey(d => d.MilestoneId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("Issue_MilestoneId_fkey");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Issues)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("Issue_ProjectId_fkey");
            });

            modelBuilder.Entity<Issuesetting>(entity =>
            {
                entity.ToTable("issuesetting");

                entity.HasIndex(e => e.ClassId, "IssueSetting_ClassId_fkey");

                entity.HasIndex(e => e.Id, "IssueSetting_Id_idx");

                entity.HasIndex(e => e.IssueId, "IssueSetting_IssueId_fkey");

                entity.HasIndex(e => e.ProjectId, "IssueSetting_ProjectId_fkey");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasDefaultValueSql("'1'");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.Issuesettings)
                    .HasForeignKey(d => d.ClassId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("IssueSetting_ClassId_fkey");

                entity.HasOne(d => d.Issue)
                    .WithMany(p => p.Issuesettings)
                    .HasForeignKey(d => d.IssueId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("IssueSetting_IssueId_fkey");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Issuesettings)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("IssueSetting_ProjectId_fkey");
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.ToTable("message");

                entity.HasIndex(e => e.ContactId, "Message_ContactId_fkey");

                entity.HasIndex(e => e.Id, "Message_Id_idx");

                entity.Property(e => e.Content).HasMaxLength(100);

                entity.HasOne(d => d.Contact)
                    .WithMany(p => p.Messages)
                    .HasForeignKey(d => d.ContactId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("Message_ContactId_fkey");
            });

            modelBuilder.Entity<Milestone>(entity =>
            {
                entity.ToTable("milestone");

                entity.HasIndex(e => e.AssignmentId, "Milestone_AssignmentId_fkey");

                entity.HasIndex(e => e.ClassId, "Milestone_ClassId_fkey");

                entity.HasIndex(e => e.Id, "Milestone_Id_idx");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.EndDate).HasColumnType("datetime(3)");

                entity.Property(e => e.StartDate).HasColumnType("datetime(3)");

                entity.Property(e => e.Title).HasColumnType("text");

                entity.HasOne(d => d.Assignment)
                    .WithMany(p => p.Milestones)
                    .HasForeignKey(d => d.AssignmentId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("Milestone_AssignmentId_fkey");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.Milestones)
                    .HasForeignKey(d => d.ClassId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("Milestone_ClassId_fkey");
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.ToTable("permission");

                entity.HasIndex(e => new { e.RoleId, e.Page }, "Permission_RoleId_Page_idx");

                entity.HasIndex(e => new { e.RoleId, e.Page }, "Permission_RoleId_Page_key")
                    .IsUnique();

                entity.HasIndex(e => e.RoleId, "Permission_RoleId_key")
                    .IsUnique();

                entity.Property(e => e.Page).HasMaxLength(191);

                entity.HasOne(d => d.Role)
                    .WithOne(p => p.Permission)
                    .HasForeignKey<Permission>(d => d.RoleId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("Permission_RoleId_fkey");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("post");

                entity.HasIndex(e => e.CategoryId, "Post_CategoryId_fkey");

                entity.HasIndex(e => e.Id, "Post_Id_idx");

                entity.HasIndex(e => e.AuthorId, "Post_UserId_fkey");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime(3)")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP(3)'");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.ImageUrl).HasMaxLength(100);

                entity.Property(e => e.Title).HasColumnType("text");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime(3)");

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.AuthorId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("Post_AuthorId_fkey");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("Post_CategoryId_fkey");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.ToTable("project");

                entity.HasIndex(e => e.ClassId, "Project_ClassId_fkey");

                entity.HasIndex(e => e.Id, "Project_Id_idx");

                entity.HasIndex(e => e.LeaderId, "Project_LeaderId_fkey");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.EnglishName).HasMaxLength(100);

                entity.Property(e => e.Status).HasDefaultValueSql("'1'");

                entity.Property(e => e.VietnameseName).HasMaxLength(100);

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.Projects)
                    .HasForeignKey(d => d.ClassId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("Project_ClassId_fkey");

                entity.HasOne(d => d.Leader)
                    .WithMany(p => p.Projects)
                    .HasForeignKey(d => d.LeaderId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("Project_LeaderId_fkey");

                entity.HasMany(d => d.Students)
                    .WithMany(p => p.ProjectsNavigation)
                    .UsingEntity<Dictionary<string, object>>(
                        "Projectstudent",
                        l => l.HasOne<User>().WithMany().HasForeignKey("StudentId").OnDelete(DeleteBehavior.Restrict).HasConstraintName("ProjectStudent_StudentId_fkey"),
                        r => r.HasOne<Project>().WithMany().HasForeignKey("ProjectId").OnDelete(DeleteBehavior.Restrict).HasConstraintName("ProjectStudent_ProjectId_fkey"),
                        j =>
                        {
                            j.HasKey("ProjectId", "StudentId").HasName("PRIMARY");

                            j.ToTable("projectstudent");

                            j.HasIndex(new[] { "ProjectId", "StudentId" }, "ProjectStudent_ProjectId_StudentId_idx");

                            j.HasIndex(new[] { "StudentId" }, "ProjectStudent_StudentId_fkey");
                        });
            });

            modelBuilder.Entity<Setting>(entity =>
            {
                entity.ToTable("setting");

                entity.HasIndex(e => e.Id, "Setting_Id_idx");

                entity.HasIndex(e => new { e.Type, e.Value }, "Setting_Type_Value_idx");

                entity.HasIndex(e => new { e.Type, e.Value }, "Setting_Type_Value_key")
                    .IsUnique();

                entity.Property(e => e.Description).HasMaxLength(400);

                entity.Property(e => e.Type).HasMaxLength(20);

                entity.Property(e => e.Value).HasMaxLength(50);
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.ToTable("subject");

                entity.HasIndex(e => e.Id, "Subject_Id_idx");

                entity.Property(e => e.Code).HasMaxLength(10);

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.HasIndex(e => e.Email, "User_Email_idx");

                entity.HasIndex(e => e.Email, "User_Email_key")
                    .IsUnique();

                entity.HasIndex(e => e.Id, "User_Id_idx");

                entity.HasIndex(e => e.RoleId, "User_RoleId_fkey");

                entity.Property(e => e.Address).HasMaxLength(191);

                entity.Property(e => e.Avatar).HasMaxLength(100);

                entity.Property(e => e.ConfirmToken).HasMaxLength(64);

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.LstAccessTime).HasColumnType("datetime(3)");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(64);

                entity.Property(e => e.Phone).HasMaxLength(15);

                entity.Property(e => e.ResetToken).HasMaxLength(64);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("User_RoleId_fkey");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
