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
    }
}
