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
            Comments = new List<Post>();
        }

        public string Title { get; set; }
        public List<Post> Comments { get; set; }
    }
}
