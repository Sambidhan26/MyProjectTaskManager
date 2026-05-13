using Microsoft.EntityFrameworkCore;
using TaskManager.API.Models;

namespace TaskManager.API.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Priority> Priorities { get; set; }
        public DbSet<Comment> Comments { get; set; }

        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure relationships and constraints if needed

            //TaskItems => Category
            modelBuilder.Entity<TaskItem>()
                .HasOne(u => u.Category)
                .WithMany(u => u.TaskItems)
                .HasForeignKey(u => u.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            //TaskItems => Priority
            modelBuilder.Entity<TaskItem>()
                .HasOne(u => u.Priority)
                .WithMany(u => u.TaskItems)
                .HasForeignKey(u => u.PriorityId)
                .OnDelete(DeleteBehavior.SetNull);

            //TaskItems => Comments
            modelBuilder.Entity<Comment>()
                .HasOne(u => u.TaskItem)
                .WithMany(u => u.Comments)
                .HasForeignKey(u => u.TaskItemId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
