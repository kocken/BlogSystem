using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System.Linq;

namespace Data
{
    public class BlogContext : DbContext
    {
        //public static readonly LoggerFactory _loggerFactory
        //    = new LoggerFactory(new[]
        //    {
        //        new ConsoleLoggerProvider((category, level) =>
        //        category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information, true)
        //    });

        public BlogContext(DbContextOptions<BlogContext> options) : base(options) {}

        public DbSet<Rank> Ranks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Thread> Threads { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<EvaluationValue> EvaluationValues { get; set; }
        public DbSet<Evaluation> Evaluations { get; set; }
        public DbSet<CommentEvaluation> CommentEvaluations { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            modelBuilder.Entity<Rank>().ToTable("Rank");
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Thread>().ToTable("Thread");
            modelBuilder.Entity<Comment>().ToTable("Comment");
            modelBuilder.Entity<EvaluationValue>().ToTable("EvaluationValue");
            modelBuilder.Entity<Evaluation>().ToTable("Evaluation");
            modelBuilder.Entity<CommentEvaluation>().ToTable("CommentEvaluation").HasKey(m => new { m.CommentId, m.EvaluationId });
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);
        //    optionsBuilder.EnableSensitiveDataLogging();
        //    optionsBuilder.UseLoggerFactory(_loggerFactory);
        //}

    }
}
