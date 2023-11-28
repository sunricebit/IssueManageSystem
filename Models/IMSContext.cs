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

        public virtual DbSet<PrismaMigration> PrismaMigrations { get; set; } = null!;

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

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
