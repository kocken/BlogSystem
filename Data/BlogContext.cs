using Microsoft.EntityFrameworkCore;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    class BlogContext : DbContext
    {
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
            modelBuilder.Entity<PostEvaluation>().ToTable("PostEvaluation");
            modelBuilder.Entity<PostEvaluation>().HasKey(m => new { m.PostId, m.EvaluationId });
        }
    }
}
