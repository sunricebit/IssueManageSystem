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
        public virtual DbSet<IssueSetting> IssueSettings { get; set; } = null!;
        public virtual DbSet<Message> Messages { get; set; } = null!;
        public virtual DbSet<Milestone> Milestones { get; set; } = null!;
        public virtual DbSet<Permission> Permissions { get; set; } = null!;
        public virtual DbSet<Post> Posts { get; set; } = null!;
        public virtual DbSet<Project> Projects { get; set; } = null!;
        public virtual DbSet<Report> Reports { get; set; } = null!;
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
                entity.ToTable("Assignment", "IMS");

                entity.HasIndex(e => e.Id, "Assignment_Id_idx");

                entity.HasIndex(e => e.SubjectId, "Assignment_SubjectId_fkey");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime(3)")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP(3)'");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.Assignments)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("Assignment_SubjectId_fkey");
            });

            modelBuilder.Entity<Class>(entity =>
            {
                entity.ToTable("Class", "IMS");

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
                        "ClassStudent",
                        l => l.HasOne<User>().WithMany().HasForeignKey("StudentId").OnDelete(DeleteBehavior.Restrict).HasConstraintName("ClassStudent_StudentId_fkey"),
                        r => r.HasOne<Class>().WithMany().HasForeignKey("ClassId").OnDelete(DeleteBehavior.Restrict).HasConstraintName("ClassStudent_ClassId_fkey"),
                        j =>
                        {
                            j.HasKey("ClassId", "StudentId").HasName("PRIMARY");

                            j.ToTable("ClassStudent", "IMS");

                            j.HasIndex(new[] { "ClassId", "StudentId" }, "ClassStudent_ClassId_StudentId_idx");

                            j.HasIndex(new[] { "StudentId" }, "ClassStudent_StudentId_fkey");
                        });
            });

            modelBuilder.Entity<Contact>(entity =>
            {
                entity.ToTable("Contact", "IMS");

                entity.HasIndex(e => e.CarerId, "Contact_CarerId_fkey");

                entity.HasIndex(e => e.ContactTypeId, "Contact_ContactTypeId_fkey");

                entity.HasIndex(e => e.Id, "Contact_Id_idx");

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
                entity.ToTable("Issue", "IMS");

                entity.HasIndex(e => e.AssigneeId, "Issue_AssigneeId_fkey");

                entity.HasIndex(e => e.AuthorId, "Issue_AuthorId_fkey");

                entity.HasIndex(e => e.Id, "Issue_Id_idx");

                entity.HasIndex(e => e.MilestoneId, "Issue_MilestoneId_fkey");

                entity.HasIndex(e => e.ParentIssueId, "Issue_ParentIssueId_fkey");

                entity.HasIndex(e => e.ProcessId, "Issue_ProcessId_fkey");

                entity.HasIndex(e => e.ProjectId, "Issue_ProjectId_fkey");

                entity.HasIndex(e => e.StatusId, "Issue_StatusId_fkey");

                entity.HasIndex(e => e.TypeId, "Issue_TypeId_fkey");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime(3)")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP(3)'");

                entity.Property(e => e.FileName).HasMaxLength(100);

                entity.Property(e => e.FileNameUrl).HasMaxLength(100);

                entity.Property(e => e.Title).HasColumnType("text");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime(3)")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP(3)'");

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

                entity.HasOne(d => d.ParentIssue)
                    .WithMany(p => p.InverseParentIssue)
                    .HasForeignKey(d => d.ParentIssueId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("Issue_ParentIssueId_fkey");

                entity.HasOne(d => d.Process)
                    .WithMany(p => p.IssueProcesses)
                    .HasForeignKey(d => d.ProcessId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("Issue_ProcessId_fkey");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Issues)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("Issue_ProjectId_fkey");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.IssueStatuses)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("Issue_StatusId_fkey");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.IssueTypes)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("Issue_TypeId_fkey");
            });

            modelBuilder.Entity<IssueSetting>(entity =>
            {
                entity.ToTable("IssueSetting", "IMS");

                entity.HasIndex(e => e.ClassId, "IssueSetting_ClassId_fkey");

                entity.HasIndex(e => e.Id, "IssueSetting_Id_idx");

                entity.HasIndex(e => e.ProjectId, "IssueSetting_ProjectId_fkey");

                entity.Property(e => e.Color)
                    .HasMaxLength(10)
                    .HasDefaultValueSql("'#009966'");

                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.Type).HasMaxLength(20);

                entity.Property(e => e.Value).HasMaxLength(100);

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.IssueSettings)
                    .HasForeignKey(d => d.ClassId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("IssueSetting_ClassId_fkey");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.IssueSettings)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("IssueSetting_ProjectId_fkey");
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.ToTable("Message", "IMS");

                entity.HasIndex(e => e.ContactId, "Message_ContactId_fkey");

                entity.HasIndex(e => e.Id, "Message_Id_idx");

                entity.Property(e => e.Content).HasMaxLength(100);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime(3)")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP(3)'");

                entity.HasOne(d => d.Contact)
                    .WithMany(p => p.Messages)
                    .HasForeignKey(d => d.ContactId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("Message_ContactId_fkey");
            });

            modelBuilder.Entity<Milestone>(entity =>
            {
                entity.ToTable("Milestone", "IMS");

                entity.HasIndex(e => e.AssignmentId, "Milestone_AssignmentId_fkey");

                entity.HasIndex(e => e.ClassId, "Milestone_ClassId_fkey");

                entity.HasIndex(e => e.Id, "Milestone_Id_idx");

                entity.HasIndex(e => e.ProjectId, "Milestone_ProjectId_fkey");

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
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("Milestone_ClassId_fkey");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Milestones)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("Milestone_ProjectId_fkey");
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.ToTable("Permission", "IMS");

                entity.HasIndex(e => e.PageId, "Permission_PageId_fkey");

                entity.HasIndex(e => new { e.RoleId, e.PageId }, "Permission_RoleId_PageId_idx");

                entity.HasIndex(e => new { e.RoleId, e.PageId }, "Permission_RoleId_PageId_key")
                    .IsUnique();

                entity.HasOne(d => d.Page)
                    .WithMany(p => p.PermissionPages)
                    .HasForeignKey(d => d.PageId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("Permission_PageId_fkey");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.PermissionRoles)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("Permission_RoleId_fkey");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("Post", "IMS");

                entity.HasIndex(e => e.AuthorId, "Post_AuthorId_fkey");

                entity.HasIndex(e => e.CategoryId, "Post_CategoryId_fkey");

                entity.HasIndex(e => e.Id, "Post_Id_idx");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime(3)")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP(3)'");

                entity.Property(e => e.Excerpt).HasColumnType("text");

                entity.Property(e => e.ImageUrl).HasMaxLength(300);

                entity.Property(e => e.Title).HasColumnType("text");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime(3)")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP(3)'");

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
                entity.ToTable("Project", "IMS");

                entity.HasIndex(e => e.ClassId, "Project_ClassId_fkey");

                entity.HasIndex(e => e.Id, "Project_Id_idx");

                entity.HasIndex(e => e.LeaderId, "Project_LeaderId_fkey");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.GroupName).HasMaxLength(255);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Status).HasDefaultValueSql("'1'");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.Projects)
                    .HasForeignKey(d => d.ClassId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("Project_ClassId_fkey");

                entity.HasOne(d => d.Leader)
                    .WithMany(p => p.Projects)
                    .HasForeignKey(d => d.LeaderId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("Project_LeaderId_fkey");

                entity.HasMany(d => d.Students)
                    .WithMany(p => p.ProjectsNavigation)
                    .UsingEntity<Dictionary<string, object>>(
                        "ProjectStudent",
                        l => l.HasOne<User>().WithMany().HasForeignKey("StudentId").OnDelete(DeleteBehavior.Restrict).HasConstraintName("ProjectStudent_StudentId_fkey"),
                        r => r.HasOne<Project>().WithMany().HasForeignKey("ProjectId").OnDelete(DeleteBehavior.Restrict).HasConstraintName("ProjectStudent_ProjectId_fkey"),
                        j =>
                        {
                            j.HasKey("ProjectId", "StudentId").HasName("PRIMARY");

                            j.ToTable("ProjectStudent", "IMS");

                            j.HasIndex(new[] { "ProjectId", "StudentId" }, "ProjectStudent_ProjectId_StudentId_idx");

                            j.HasIndex(new[] { "StudentId" }, "ProjectStudent_StudentId_fkey");
                        });
            });

            modelBuilder.Entity<Report>(entity =>
            {
                entity.ToTable("Report", "IMS");

                entity.HasIndex(e => e.PostId, "Report_PostId_fkey");

                entity.HasIndex(e => e.ReporterId, "Report_ReporterId_fkey");

                entity.Property(e => e.Content).HasMaxLength(300);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime(3)")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP(3)'");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Reports)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("Report_PostId_fkey");

                entity.HasOne(d => d.Reporter)
                    .WithMany(p => p.Reports)
                    .HasForeignKey(d => d.ReporterId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("Report_ReporterId_fkey");
            });

            modelBuilder.Entity<Setting>(entity =>
            {
                entity.ToTable("Setting", "IMS");

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
                entity.ToTable("Subject", "IMS");

                entity.HasIndex(e => e.Id, "Subject_Id_idx");

                entity.HasIndex(e => e.SubjectManagerId, "Subject_SubjectManagerId_fkey");

                entity.Property(e => e.Code).HasMaxLength(10);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime(3)")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP(3)'");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.HasOne(d => d.SubjectManager)
                    .WithMany(p => p.Subjects)
                    .HasForeignKey(d => d.SubjectManagerId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("Subject_SubjectManagerId_fkey");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User", "IMS");

                entity.HasIndex(e => e.Email, "User_Email_key")
                    .IsUnique();

                entity.HasIndex(e => e.Id, "User_Id_idx");

                entity.HasIndex(e => e.RoleId, "User_RoleId_fkey");

                entity.Property(e => e.Address).HasMaxLength(191);

                entity.Property(e => e.Avatar).HasMaxLength(300);

                entity.Property(e => e.ConfirmToken).HasMaxLength(64);

                entity.Property(e => e.Description).HasMaxLength(255);

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
