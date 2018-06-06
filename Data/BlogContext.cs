using Domain;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class BlogContext : DbContext
    {

        public BlogContext(DbContextOptions<BlogContext> options) : base(options) {}

        public DbSet<User> Users { get; set; }
        public DbSet<Thread> Threads { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Evaluation> Evaluations { get; set; }
        public DbSet<PostEvaluation> PostEvaluations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Thread>().ToTable("Thread");
            modelBuilder.Entity<Post>().ToTable("Post");
            modelBuilder.Entity<Evaluation>().ToTable("Evaluation");
            modelBuilder.Entity<PostEvaluation>().ToTable("PostEvaluation").HasKey(m => new { m.PostId, m.EvaluationId });
        }

        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server = (localdb)\\mssqllocaldb; Database = MKBlog; Trusted_Connection = True;");
        }*/

    }
}
