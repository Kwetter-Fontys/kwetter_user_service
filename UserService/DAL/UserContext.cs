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
        //public DbSet<Followers> Followers { get; set; }
        //public DbSet<Following> Following { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
        }
    }
}
