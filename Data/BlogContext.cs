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
        public DbSet<Post> Courses { get; set; }
    }
}
