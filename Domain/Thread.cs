using System.Collections.Generic;

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
