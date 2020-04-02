using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SportCompassRestApi.Models
{
    public partial class dbContext : DbContext
    {
        public dbContext()
        {
        }

        public dbContext(DbContextOptions<dbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<MComments> MComments { get; set; }
        public virtual DbSet<MPosts> MPosts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseMySql("server=localhost;database=sportcompass;uid=root;pwd=;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MComments>(entity =>
            {
                entity.ToTable("m_comments");

                entity.HasIndex(e => e.PostId)
                    .HasName("key_1");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CommentBody)
                    .HasColumnName("comment_body")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.PostId)
                    .HasColumnName("post_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UserEmail)
                    .HasColumnName("user_email")
                    .HasColumnType("varchar(50)");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.MComments)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("key_1");
            });

            modelBuilder.Entity<MPosts>(entity =>
            {
                entity.ToTable("m_posts");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ImageUrl)
                    .HasColumnName("image_url")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.PostBody)
                    .HasColumnName("post_body")
                    .HasColumnType("text");

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasColumnType("varchar(255)");

                entity.Property(e => e.UserEmail)
                    .HasColumnName("user_email")
                    .HasColumnType("varchar(50)");
            });
        }
    }
}
