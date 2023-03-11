using Conduit.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Infrastructure
{
    public class ConduitDbContext : DbContext
    {
        public DbSet<Article> Article { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<Follow> Follow { get; set; }
        public DbSet<UserArticle> UserArticle { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Data Source = (localdb)\\MSSQLLocalDB; Initial Catalog = Conduit");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Follow>()
                .HasKey(k => new { k.FollowerId, k.FolloweeId });

            modelBuilder.Entity<Follow>()
                .HasOne(u => u.Followee)
                .WithMany(u => u.Followers)
                .HasForeignKey(u => u.FolloweeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Follow>()
                .HasOne(u => u.Follower)
                .WithMany(u => u.Followings)
                .HasForeignKey(u => u.FollowerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Article>()
                .HasOne(a => a.User)
                .WithMany(u => u.Articles);

            modelBuilder.Entity<UserArticle>()
                .HasKey(f => new { f.ArticleId, f.UserId });

            modelBuilder.Entity<UserArticle>()
                .HasOne(f => f.Article)
                .WithMany(a => a.Favorites)
                .HasForeignKey(f => f.ArticleId);

            modelBuilder.Entity<UserArticle>()
                .HasOne(f => f.User)
                .WithMany(u => u.Favorites)
                .HasForeignKey(f => f.UserId);
        }
    }
}
