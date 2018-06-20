using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Thread : Post
    {
        public Thread()
        {
            Comments = new List<Comment>();
            ThreadTags = new List<ThreadTag>();
        }

        [Key, Required]
        public int Id { get; set; }

        [Required, StringLength(80, MinimumLength = 1)]
        public string Title { get; set; }

        public List<Comment> Comments { get; set; }

        public List<ThreadTag> ThreadTags { get; set; }
    }
}
