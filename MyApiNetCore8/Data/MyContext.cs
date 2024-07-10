using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyApiNetCore8.Model;

namespace MyApiNetCore8.Data
{
    public class MyContext : IdentityDbContext<User>
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options)
        {
        }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<TaskModel> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TaskModel>()
                .HasMany(u => u.Users)
                .WithMany(u => u.TaskModels);
        }
    }
}
