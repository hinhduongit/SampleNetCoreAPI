using DataAccess.Model;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.DBContext
{
    public class SampleNetCoreAPIContext : DbContext
    {
        public SampleNetCoreAPIContext(DbContextOptions<SampleNetCoreAPIContext> options)
    : base(options)
        { }

        public virtual DbSet<Blog> Blogs { get; set; }
        public virtual DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blog>(entity =>
            {
                entity.ToTable("blogs");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("posts");

                entity.HasIndex(e => e.BlogId)
                    .HasName("FK_Post_Blog_BlogId_idx");

                entity.HasOne(d => d.Blog)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.BlogId)
                    .HasConstraintName("FK_Post_Blog_BlogId");
            });
        }
    }
}
