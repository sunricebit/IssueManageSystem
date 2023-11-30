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

        public virtual DbSet<Contact> Contacts { get; set; } = null!;
        public virtual DbSet<Post> Posts { get; set; } = null!;
        public virtual DbSet<PrismaMigration> PrismaMigrations { get; set; } = null!;
        public virtual DbSet<Setting> Settings { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySQL("server=localhost;uid=root;pwd=123456;database=IMS");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contact>(entity =>
            {
                entity.ToTable("Contact", "IMS");

                entity.HasIndex(e => e.Email, "Contact_Email_key")
                    .IsUnique();

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime(3)")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP(3)'");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.IsValid)
                    .IsRequired()
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.Message).HasColumnType("text");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Phone).HasMaxLength(15);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime(3)");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("Post", "IMS");

                entity.HasIndex(e => e.UserId, "Post_UserId_fkey");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime(3)")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP(3)'");

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.ImageUrl).HasMaxLength(100);

                entity.Property(e => e.Title).HasColumnType("text");

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime(3)");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("Post_UserId_fkey");
            });

            modelBuilder.Entity<PrismaMigration>(entity =>
            {
                entity.ToTable("_prisma_migrations", "IMS");

                entity.Property(e => e.Id)
                    .HasMaxLength(36)
                    .HasColumnName("id");

                entity.Property(e => e.AppliedStepsCount).HasColumnName("applied_steps_count");

                entity.Property(e => e.Checksum)
                    .HasMaxLength(64)
                    .HasColumnName("checksum");

                entity.Property(e => e.FinishedAt)
                    .HasColumnType("datetime(3)")
                    .HasColumnName("finished_at");

                entity.Property(e => e.Logs)
                    .HasColumnType("text")
                    .HasColumnName("logs");

                entity.Property(e => e.MigrationName)
                    .HasMaxLength(255)
                    .HasColumnName("migration_name");

                entity.Property(e => e.RolledBackAt)
                    .HasColumnType("datetime(3)")
                    .HasColumnName("rolled_back_at");

                entity.Property(e => e.StartedAt)
                    .HasColumnType("datetime(3)")
                    .HasColumnName("started_at")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP(3)'");
            });

            modelBuilder.Entity<Setting>(entity =>
            {
                entity.ToTable("Setting", "IMS");

                entity.HasIndex(e => e.Id, "Setting_Id_idx");

                entity.HasIndex(e => new { e.Type, e.Value }, "Setting_Type_Value_idx");

                entity.HasIndex(e => new { e.Type, e.Value }, "Setting_Type_Value_key")
                    .IsUnique();

                entity.Property(e => e.Type).HasMaxLength(20);

                entity.Property(e => e.Value).HasMaxLength(50);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User", "IMS");

                entity.HasIndex(e => e.Email, "User_Email_idx");

                entity.HasIndex(e => e.Email, "User_Email_key")
                    .IsUnique();

                entity.HasIndex(e => e.Id, "User_Id_idx");

                entity.HasIndex(e => e.RoleId, "User_RoleId_fkey");

                entity.Property(e => e.Address).HasMaxLength(191);

                entity.Property(e => e.Avatar).HasMaxLength(100);

                entity.Property(e => e.ConfirmToken).HasMaxLength(64);

                entity.Property(e => e.Email).HasMaxLength(100);

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
