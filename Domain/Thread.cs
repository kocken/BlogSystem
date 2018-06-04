using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Thread : Post
    {
        public Thread()
        {
            Posts = new List<Post>();
        }

        public string Title { get; set; }
        public List<Post> Posts { get; set; }
    }
}
