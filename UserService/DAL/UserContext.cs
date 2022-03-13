using UserService.Models;
using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore.Extensions;

namespace UserService.DAL
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options): base(options)
        {
            
        }
        public DbSet<User> Users { get; set; }
        public DbSet<FriendsLink> FriendsLinks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging(true);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
            modelBuilder.Entity<FriendsLink>().ToTable("FriendsLink");
            modelBuilder.Entity<FriendsLink>(entity =>
            {
                entity.HasOne<User>().WithMany().HasForeignKey(f => f.UserFollowerId);
                entity.HasOne<User>().WithMany().HasForeignKey(f => f.UserFollowingId);
            });
        }
    }
}
