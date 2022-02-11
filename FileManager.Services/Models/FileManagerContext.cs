using System;
using FileManagerAPI.Common;
using FileManagerAPI.Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace FileManager.Services.Models
{
    public partial class FileManagerContext : DbContext
    {
        private readonly AppSettings _settings;
        public FileManagerContext(AppSettings settings)
        {
            _settings = settings;
        }
        public FileManagerContext(DbContextOptions<FileManagerContext> options, AppSettings settings)
              : base(options)
        {
            _settings = settings;
        }

        public FileManagerContext(DbContextOptions<FileManagerContext> options)
            : base(options)
        {
         
        }

        public virtual DbSet<MenuPath> MenuPaths { get; set; }
        public virtual DbSet<Upload> Uploads { get; set; }
        public virtual DbSet<UploadItem> UploadItems { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                if (_settings == null)
                {
                    optionsBuilder.UseSqlServer("Server=CEL51\\SQLExpress;Database=FileManager;Trusted_Connection=True;");
                }
                else
                {
                    optionsBuilder.UseSqlServer(_settings.DefaultConnection.ConnectionNode.ConnectionString);
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<MenuPath>(entity =>
            {
                entity.HasKey(e => e.MenuPathsId)
                    .HasName("PK_Menus");

                entity.Property(e => e.MenuPathsId).HasColumnName("MenuPathsID");

                entity.Property(e => e.MenuPath1)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("MenuPath");

                entity.Property(e => e.MenuPathTitle)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<Upload>(entity =>
            {
                entity.Property(e => e.UploadId).HasColumnName("UploadID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.UploadCode)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.UploadDescription)
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.Uploads)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_Uploads_Users");
            });

            modelBuilder.Entity<UploadItem>(entity =>
            {
                entity.Property(e => e.UploadItemId).HasColumnName("UploadItemID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.FileGeneratedName)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.FileOrginalName)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.FileType)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UploadId).HasColumnName("UploadID");

                entity.HasOne(d => d.Upload)
                    .WithMany(p => p.UploadItems)
                    .HasForeignKey(d => d.UploadId)
                    .HasConstraintName("FK_UploadItems_Uploads");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.UserCode)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UserFullName)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UserPassword)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
