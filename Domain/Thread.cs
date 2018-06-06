using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Thread : Post
    {
        public Thread()
        {
            Comments = new List<Post>();
        }

        [Required, StringLength(20, MinimumLength = 1)]
        public string Title { get; set; }
        public List<Post> Comments { get; set; }
    }
}
