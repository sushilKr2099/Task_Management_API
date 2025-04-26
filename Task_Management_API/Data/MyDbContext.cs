using Microsoft.EntityFrameworkCore;
using Task_Management_API.Models;

namespace TaskManagementAPI.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
            Console.WriteLine(Database.GetDbConnection().ConnectionString);
        }
        public DbSet<User> Users { get; set; }
        public DbSet<TaskModel> Tasks { get; set; }
        public DbSet<TaskComment> TaskComments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskModel>()
                .HasOne(t => t.AssignedToUser)
                .WithMany(u => u.Tasks)
                .HasForeignKey(t => t.AssignedToUserId);

            modelBuilder.Entity<TaskComment>()
                .HasOne(c => c.Task)
                .WithMany(t => t.Comments)
                .HasForeignKey(c => c.TaskId);

            modelBuilder.Entity<TaskComment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}