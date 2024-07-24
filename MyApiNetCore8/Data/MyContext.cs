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
        public DbSet<TaskModel> TaskModels { get; set; }
        public DbSet<TaskMember> TaskMembers { get; set; }
        public DbSet<ChatGroup> ChatGroups { get; set; }
        public DbSet<ChatGroupMember> ChatGroupMembers { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TaskModel>()
                .HasOne(t => t.ChatGroup)
                .WithOne(cg => cg.TaskModel)
                .HasForeignKey<ChatGroup>(cg => cg.TaskModelId);

            modelBuilder.Entity<TaskMember>()
                .HasKey(tm => new { tm.TaskModelId, tm.UserId });

            modelBuilder.Entity<ChatGroupMember>()
                .HasKey(cgm => new { cgm.ChatGroupId, cgm.UserId });
        }
    }
}
