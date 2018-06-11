using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System.Linq;

namespace Data
{
    public class BlogContext : DbContext
    {

        public BlogContext(DbContextOptions<BlogContext> options) : base(options) {}

        public DbSet<Rank> Ranks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Thread> Threads { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<EvaluationValue> EvaluationValues { get; set; }
        public DbSet<Evaluation> Evaluations { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ThreadTag> ThreadTags { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            modelBuilder.Entity<Rank>().ToTable("Rank").HasIndex(r => r.Name).IsUnique();
            modelBuilder.Entity<User>().ToTable("User").HasIndex(u => u.Username).IsUnique();
            modelBuilder.Entity<Thread>().ToTable("Thread");
            modelBuilder.Entity<Comment>().ToTable("Comment");
            modelBuilder.Entity<EvaluationValue>().ToTable("EvaluationValue");
            modelBuilder.Entity<Evaluation>().ToTable("Evaluation");
            modelBuilder.Entity<Tag>().ToTable("Tag");
            modelBuilder.Entity<ThreadTag>().ToTable("ThreadTag").HasKey(t => new {
                t.ThreadId,
                t.TagId
            });
        }

    }
}
